using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RestApiServer
{
    public class DbSetExecuter : IDbSetExecuter
    {
        private readonly DbContext _dbContext;
        private readonly MethodInfo _setMethod;

        public DbSetExecuter(DbContext dbContext, Type entityType, MethodInfo setMethod)
        {
            _dbContext = dbContext;
            EntityType = entityType;
            _setMethod = setMethod;
        }

        public Type EntityType { get; private set; }

        public async Task<object> CreateAsync(object createObject)
        {
            await _dbContext.AddAsync(createObject);
            await _dbContext.SaveChangesAsync();
            return createObject;
        }

        public Task<object> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetCollectionAsync()
        {
            var set = (IQueryable<object>)_setMethod?.Invoke(_dbContext, null);
            var collection = await set.ToListAsync();
            return collection;
        }

        public async Task<object> GetSingleAsync(string id)
        {
            var entity = await _dbContext.FindAsync(EntityType, long.Parse(id));
            return entity;
        }

        public Task<object> UpdateAsync(string id, object updateObject)
        {
            throw new NotImplementedException();
        }
    }
}
