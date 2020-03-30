using Microsoft.EntityFrameworkCore;

namespace RestApiServer.Database
{
    internal interface IDbSetExecutorFactory
    {
        IDbSetExecuter Create(DbContext dbContext, string dbSetName);
    }
}