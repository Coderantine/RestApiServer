using System.Threading.Tasks;

namespace RestApiServer.Requests
{
    internal interface IRestApiRequestHandler
    {
        Task<bool> TryHandleAsync(RestApiRequestContext context);
    }
}