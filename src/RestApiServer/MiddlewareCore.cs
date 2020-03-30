using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiServer
{
    public class MiddlewareCore<TDbContext> where TDbContext : DbContext
    {
        private readonly RequestDelegate _next;
        private readonly RestApiServerOptions _options;

        public MiddlewareCore(
            RequestDelegate next,
            RestApiServerOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context, TDbContext dbContext)
        {
            if (_options.UsePrefix && !context.Request.Path.StartsWithSegments(_options.Prefix))
            {
                await _next(context);
                return;
            }

            var prefixSegmentsCount = _options.UsePrefix ? _options.Prefix.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length : 0;
            var pathSegments = GetPathSegments(context.Request, prefixSegmentsCount);

            if (pathSegments.Length == 0 || pathSegments.Length > 2)
            {
                await _next(context);
                return;
            }

            // TODO check for predefined endpoint

            var restSetName = pathSegments[0];
            string id = null;
            if (pathSegments.Length == 2)
            {
                id = pathSegments[1];
            }

            var executor = DbSetExecutorFactory.Create(dbContext, restSetName);
            if (executor == null)
            {
                await _next(context);
                return;
            }

            switch (context.Request.Method)
            {
                case "POST":

                    if (context.Request.ContentType != "application/json")
                    {
                        await _next(context);
                        return;
                    }

                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        var body = await reader.ReadToEndAsync();
                        var entity = JsonConvert.DeserializeObject(body, executor.EntityType);
                        if (entity != null)
                        {
                            await executor.CreateAsync(entity);
                            var json = JsonConvert.SerializeObject(entity);
                            await context.Response.WriteAsync(json);
                        }

                    }

                    break;
                case "GET":
                    if (id != null)
                    {
                        var entity = await executor.GetSingleAsync(id);
                        if (entity != null)
                        {
                            var json = JsonConvert.SerializeObject(entity);
                            await context.Response.WriteAsync(json);
                        }
                        else
                        {
                            await _next(context);
                            return;
                        }
                    }
                    else
                    {
                        var entities = await executor.GetCollectionAsync();
                        if (entities != null)
                        {
                            var json = JsonConvert.SerializeObject(entities);
                            await context.Response.WriteAsync(json);
                        }
                        else
                        {
                            await _next(context);
                            return;
                        }
                    }
                    break;
            }
        }

        public static string[] GetPathSegments(HttpRequest request, int prefixSegmentsCount)
        {
            var pathSegments = request.Path.ToUriComponent().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Skip(prefixSegmentsCount);
            return pathSegments.ToArray();
        }

    }
}
