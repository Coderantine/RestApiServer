using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public async ValueTask<object> CreateAsync(object createObject)
        {
            await _dbContext.AddAsync(createObject);
            await _dbContext.SaveChangesAsync();
            return createObject;
        }

        public async ValueTask DeleteAsync(string id)
        {
            var entity = await _dbContext.FindAsync(EntityType, BuildKey(EntityType, id));
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<object>> GetCollectionAsync()
        {
            var set = (IQueryable<object>)_setMethod?.Invoke(_dbContext, null);
            var collection = await set.ToListAsync();
            return collection;
        }

        public ValueTask<object> GetSingleAsync(string id)
        {
            return _dbContext.FindAsync(EntityType, BuildKey(EntityType, id));
        }

        public async ValueTask UpdateAsync(string id, object updateObject)
        {
            var entity = await _dbContext.FindAsync(EntityType, BuildKey(EntityType, id));
            _dbContext.Entry(entity).CurrentValues.SetValues(updateObject);
            await _dbContext.SaveChangesAsync();
        }

        private object BuildKey(Type entity, string id)
        {
            var keyNames = _dbContext.Model.FindEntityType(entity).FindPrimaryKey().Properties
                .Select(x => x.Name);

            if(keyNames.Count() == 1)
            {
                var propertyType = entity.GetProperty(keyNames.Single()).PropertyType;
                return TypeDescriptor.GetConverter(propertyType).ConvertFromInvariantString(id);
            }

            throw new NotImplementedException();
        }
    }
}
