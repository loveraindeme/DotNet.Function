using Microsoft.AspNetCore.Authorization;

namespace DotNet.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizeAttribute>
    {
        private readonly UserPermissionContainer _userPermissionContainer;

        public PermissionAuthorizationHandler(UserPermissionContainer userPermissionContainer)
        {
            _userPermissionContainer = userPermissionContainer;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizeAttribute requirement)
        {
            var userId = context.User.FindFirst(AuthClaimTypes.UserId)?.Value;
            var refreshTokenId = context.User.FindFirst(AuthClaimTypes.RefreshTokenId)?.Value;
            if (userId == null || !Guid.TryParse(userId, out _))
            {
                return;
            }

            // 模拟获取用户的权限码列表
            if (!_userPermissionContainer.TryGet($"{userId}&{refreshTokenId}", out List<string>? permissionCodes))
            {
                return;
            }

            if (permissionCodes == null)
            {
                return;
            }

            // 可自定义权限判断逻辑
            if (HasPermission(requirement.Permissions, permissionCodes))
            {
                context.Succeed(requirement);
            }
        }

        private static bool HasPermission(string[] permissions, List<string> permissionCodes)
        {
            if (permissions.Length == 0)
            {
                return true;
            }

            foreach (var permission in permissions)
            {
                if (permissionCodes.Contains(permission))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
