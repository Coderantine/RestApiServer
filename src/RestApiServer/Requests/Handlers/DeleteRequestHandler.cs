using System.Threading.Tasks;

namespace RestApiServer.Requests.Handlers
{
    internal class DeleteRequestHandler : IRestApiRequestHandler
    {
        public async Task<bool> TryHandleAsync(RestApiRequestContext context)
        {
            var executor = context.DbSetExecutorFactory.Create(context.DbContext, context.RouteParams.RestSetName);
            if (executor == null)
            {
                return false;
            }

            if (context.RouteParams.ResourceId is null)
            {
                return false;
            }

            await executor.DeleteAsync(context.RouteParams.ResourceId);
            context.HttpContext.Response.StatusCode = 204;
            return true;
        }
    }
}
