using KnowledgeSpace.BackendServer.Helpers;
using Microsoft.AspNetCore.Builder;

namespace KnowledgeSpace.BackendServer.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorWrapping(this IApplicationBuilder builder)
        {
            return builder.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseMiddleware<ErrorWrappingMiddleware>();
            });
            //return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
