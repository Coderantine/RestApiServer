using System.Collections.Generic;
using RestApiServer.Requests.Handlers;

namespace RestApiServer.Requests
{
    internal class RestApiRequestHandlerFactory : IRestApiRequestHandlerFactory
    {
        private static Dictionary<string, IRestApiRequestHandler> _handlersMap = new Dictionary<string, IRestApiRequestHandler>()
        {
            { "GET", new GetRequestHandler() },
            { "POST", new AddRequestHandler() },
            { "PUT", new UpdateRequestHandler() },
            { "DELETE", new DeleteRequestHandler() },
        };

        public IRestApiRequestHandler Create(RestApiRequestContext restApiRequestContext)
        {
            var httpMethod = restApiRequestContext.HttpContext.Request.Method;
            if (_handlersMap.ContainsKey(httpMethod))
            {
                return _handlersMap[httpMethod];
            }

            return null;
        }
    }
}
