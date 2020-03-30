using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace RestApiServer.Routing
{
    internal class RestApiRouteResolver : IRestApiRouteResolver
    {
        private readonly RestApiServerOptions _options;

        public RestApiRouteResolver(RestApiServerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public bool Resolve(HttpContext context, out RouteParams routeParams)
        {
            routeParams = null;

            if (_options.UsePrefix && !context.Request.Path.StartsWithSegments(_options.Prefix))
            {
                return false;
            }

            var prefixSegmentsCount = _options.UsePrefix ? _options.Prefix.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length : 0;
            var pathSegments = GetPathSegments(context.Request, prefixSegmentsCount);

            if (pathSegments.Length == 0 || pathSegments.Length > 2)
            {
                return false;
            }

            // TODO check for predefined endpoint
            var restSetName = pathSegments[0];
            string id = null;
            if (pathSegments.Length == 2)
            {
                id = pathSegments[1];
            }

            routeParams = new RouteParams()
            {
                ResourceId = id,
                RestSetName = restSetName,
            };

            return true;
        }

        private static string[] GetPathSegments(HttpRequest request, int prefixSegmentsCount)
        {
            var pathSegments = request.Path.ToUriComponent().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Skip(prefixSegmentsCount);
            return pathSegments.ToArray();
        }
    }
}
