using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace RestApiServer
{
    public static class DbSetExecutorFactory
    {
        public static IDbSetExecuter Create<TDbContext>(TDbContext dbContext, string dbSetName) where TDbContext : DbContext
        {
            var restSet = DbContextSchemaSource.Cache[typeof(TDbContext).Name]
                .RestSets.FirstOrDefault(x => x.Name.Equals(dbSetName, StringComparison.OrdinalIgnoreCase));

            if (restSet == null)
            {
                return null;
            }

            return new DbSetExecuter(dbContext, restSet.EntityType, restSet.SetMethodInfo);
        }
    }
}
