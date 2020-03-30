using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RestApiServer.Requests.Handlers
{
    internal class GetRequestHandler : IRestApiRequestHandler
    {
        public async Task<bool> TryHandleAsync(RestApiRequestContext context)
        {
            var executor = context.DbSetExecutorFactory.Create(context.DbContext, context.RouteParams.RestSetName);
            if (executor == null)
            {
                return false;
            }

            if (context.RouteParams.ResourceId != null)
            {
                var entity = await executor.GetSingleAsync(context.RouteParams.ResourceId);
                if (entity != null)
                {
                    var json = JsonConvert.SerializeObject(entity);
                    await context.HttpContext.Response.WriteAsync(json);
                    return true;
                }
            }
            else
            {
                var entities = await executor.GetCollectionAsync();
                if (entities != null)
                {
                    var json = JsonConvert.SerializeObject(entities);
                    await context.HttpContext.Response.WriteAsync(json);
                    return true;
                }
            }

            return false;
        }
    }
}
