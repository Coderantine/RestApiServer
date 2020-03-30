namespace RestApiServer.Requests
{
    internal interface IRestApiRequestHandlerFactory
    {
        IRestApiRequestHandler Create(RestApiRequestContext restApiRequestContext);
    }
}