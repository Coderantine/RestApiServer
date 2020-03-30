using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestApiServer.Database;
using RestApiServer.Requests;
using RestApiServer.Routing;

namespace RestApiServer
{
    public static class RestApiServerServiceCollectionExtensions
    {
        public static IServiceCollection AddRestApiServer<TDbContext>(this IServiceCollection services, Action<RestApiServerOptions> configure = null)
            where TDbContext : DbContext
        {
            var restApiServerOptions = new RestApiServerOptions();

            configure?.Invoke(restApiServerOptions);

            var builder = new RestApiServerSourceBuilder();
            builder.AddDbContext<TDbContext>();
            var source = builder.Build();

            services.AddSingleton(source);
            services.AddSingleton(restApiServerOptions);
            services.AddSingleton<IRestApiRouteResolver, RestApiRouteResolver>();
            services.AddSingleton<IRestApiRequestHandlerFactory, RestApiRequestHandlerFactory>();
            services.AddSingleton<IDbSetExecutorFactory, DbSetExecutorFactory>();

            return services;
        }
    }
}
