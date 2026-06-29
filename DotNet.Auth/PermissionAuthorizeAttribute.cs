using Microsoft.AspNetCore.Authorization;

namespace DotNet.Auth
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    {
        /// <summary>
        /// 功能权限编码
        /// </summary>
        public string[] Permissions { get; }

        public PermissionAuthorizeAttribute(params string[] permissions)
        {
            Permissions = NormalizePermissions(permissions);
        }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return this;
        }

        private static string[] NormalizePermissions(string[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
            {
                return [];
            }

            return permissions
                .SelectMany(permission => permission.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Where(permission => !string.IsNullOrEmpty(permission))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }
    }
}
