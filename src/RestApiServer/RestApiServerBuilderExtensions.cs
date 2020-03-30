using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace RestApiServer
{
    public static class RestApiServerBuilderExtensions
    {
        public static IApplicationBuilder UseRestApiServer<TDbContext>(this IApplicationBuilder app)
            where TDbContext : DbContext
        {
            app.UseMiddleware<MiddlewareCore<TDbContext>>();
            return app;
        }
    }
}
