using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RestApiServer.Database
{
    internal class DbSetExecutorFactory : IDbSetExecutorFactory
    {
        private readonly IRestApiServerSource _restApiServerSource;

        public DbSetExecutorFactory(IRestApiServerSource restApiServerSource)
        {
            _restApiServerSource = restApiServerSource ?? throw new ArgumentNullException(nameof(restApiServerSource));
        }

        public IDbSetExecuter Create(DbContext dbContext, string dbSetName)
        {
            var restSet = _restApiServerSource.RestSets[dbContext.GetType().Name]
                .FirstOrDefault(x => x.Name.Equals(dbSetName, StringComparison.OrdinalIgnoreCase));

            if (restSet == null)
            {
                return null;
            }

            return new DbSetExecuter(dbContext, restSet.EntityType, restSet.SetMethodInfo);
        }
    }
}
