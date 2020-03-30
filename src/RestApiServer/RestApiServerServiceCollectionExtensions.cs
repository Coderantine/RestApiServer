using Microsoft.Extensions.DependencyInjection;
using System;

namespace RestApiServer
{
    public static class RestApiServerServiceCollectionExtensions
    {
        public static IServiceCollection AddRestApiServer(this IServiceCollection services, Action<RestApiServerOptions> configure = null)
        {
            var restApiServerOptions = new RestApiServerOptions();

            configure?.Invoke(restApiServerOptions);

            services.AddSingleton(restApiServerOptions);

            return services;
        }
    }
}
