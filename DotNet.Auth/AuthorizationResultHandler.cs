using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace DotNet.Auth
{
    public class AuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            // 可自定义认证和授权失败的返回结果
            if (authorizeResult.Challenged)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var resultModel = new
                {
                    Code = 1,
                    Message = "未登录或登录已过期"
                };
                await context.Response.WriteAsJsonAsync(resultModel);
                return;
            }

            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                var resultModel = new
                {
                    Code = 1,
                    Message = "无权限访问，请授权并重新登录"
                };
                await context.Response.WriteAsJsonAsync(resultModel);
                return;
            }

            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
