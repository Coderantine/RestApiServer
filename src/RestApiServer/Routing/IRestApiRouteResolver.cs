using Microsoft.AspNetCore.Http;

namespace RestApiServer.Routing
{
    internal interface IRestApiRouteResolver
    {
        bool Resolve(HttpContext context, out RouteParams routeParams);
    }
}