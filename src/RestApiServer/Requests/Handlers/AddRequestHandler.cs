using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RestApiServer.Requests.Handlers
{
    internal class AddRequestHandler : IRestApiRequestHandler
    {
        public async Task<bool> TryHandleAsync(RestApiRequestContext context)
        {
            var executor = context.DbSetExecutorFactory.Create(context.DbContext, context.RouteParams.RestSetName);
            if (executor == null)
            {
                return false;
            }

            if (context.HttpContext.Request.ContentType != "application/json")
            {
                return false;
            }

            if (context.RouteParams.ResourceId != null)
            {
                return false;
            }

            using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                var entity = JsonConvert.DeserializeObject(body, executor.EntityType);
                if (entity != null)
                {
                    await executor.CreateAsync(entity);
                    var json = JsonConvert.SerializeObject(entity);
                    await context.HttpContext.Response.WriteAsync(json);
                    return true;
                }
            }

            return false;
        }
    }
}
