using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Database;
using RestApiServer.Routing;

namespace RestApiServer.Requests
{
    internal class RestApiRequestContext
    {
        public DbContext DbContext { get; set; }

        public RouteParams RouteParams { get; set; }

        public HttpContext HttpContext { get; set; }

        public IRestApiServerSource Source { get; set; }

        public IDbSetExecutorFactory DbSetExecutorFactory { get; set; }

        public RestApiServerOptions Options { get; set; }
    }
}
