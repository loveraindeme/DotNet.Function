using SugarSqlCore;

namespace DotNet.SugarSqlCore.Middlewares
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 认证逻辑，从请求中提取用户Id并设置到CurrentUserAmbient中
                // todo 
                CurrentUserAmbient.Set(Guid.NewGuid());
                await _next(context);
            }
            finally
            {
                CurrentUserAmbient.Clear();
            }
        }
    }

    public static class AuthorizeMiddlewareExtension
    {
        public static IApplicationBuilder UseAuthorize(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizeMiddleware>();
        }
    }
}
