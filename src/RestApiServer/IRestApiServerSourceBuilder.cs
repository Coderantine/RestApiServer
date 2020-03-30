using Microsoft.EntityFrameworkCore;

namespace RestApiServer
{
    internal interface IRestApiServerSourceBuilder
    {
        IRestApiServerSourceBuilder AddDbContext<TDbContext>()
            where TDbContext : DbContext;

        IRestApiServerSource Build();
    }
}
