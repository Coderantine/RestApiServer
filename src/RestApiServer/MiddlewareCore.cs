using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Database;
using RestApiServer.Requests;
using RestApiServer.Routing;

namespace RestApiServer
{
    internal class MiddlewareCore<TDbContext>
        where TDbContext : DbContext
    {
        private readonly RequestDelegate _next;
        private readonly RestApiServerOptions _options;
        private readonly IRestApiServerSource _source;
        private readonly IRestApiRouteResolver _restApiRouteResolver;
        private readonly IRestApiRequestHandlerFactory _restApiRequestHandlerFactory;
        private readonly IDbSetExecutorFactory _dbSetExecutorFactory;

        public MiddlewareCore(
            RequestDelegate next,
            RestApiServerOptions options,
            IRestApiServerSource source,
            IRestApiRouteResolver restApiRouteResolver,
            IRestApiRequestHandlerFactory restApiRequestHandlerFactory,
            IDbSetExecutorFactory dbSetExecutorFactory)
        {
            _next = next;
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _restApiRouteResolver = restApiRouteResolver ?? throw new ArgumentNullException(nameof(restApiRouteResolver));
            _restApiRequestHandlerFactory = restApiRequestHandlerFactory ?? throw new ArgumentNullException(nameof(restApiRequestHandlerFactory));
            _dbSetExecutorFactory = dbSetExecutorFactory ?? throw new ArgumentNullException(nameof(IDbSetExecutorFactory));
        }

        public async Task InvokeAsync(HttpContext context, TDbContext dbContext)
        {
            if (!_restApiRouteResolver.Resolve(context, out var routeParams))
            {
                await _next(context);
                return;
            }

            var restApiRequestContext = new RestApiRequestContext()
            {
                HttpContext = context,
                DbContext = dbContext,
                RouteParams = routeParams,
                Options = _options,
                Source = _source,
                DbSetExecutorFactory = _dbSetExecutorFactory,
            };

            var handler = _restApiRequestHandlerFactory.Create(restApiRequestContext);
            if (handler == null)
            {
                await _next(context);
                return;
            }

            if (!await handler.TryHandleAsync(restApiRequestContext))
            {
                await _next(context);
                return;
            }
        }
    }
}
