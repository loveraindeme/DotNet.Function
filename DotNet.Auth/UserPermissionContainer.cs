using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DotNet.Auth
{
    public class UserPermissionContainer
    {
        private readonly IMemoryCache _cache;
        private readonly JwtOptions _jwtOptions;

        public UserPermissionContainer(IMemoryCache cache,
            IOptions<JwtOptions> jwtOptions)
        {
            _cache = cache;
            _jwtOptions = jwtOptions.Value;
        }

        public void Set(string userId, List<string> permissionCodes)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_jwtOptions.RefreshHours)
            };
            _cache.Set(userId, permissionCodes, options);
        }

        public bool TryGet(string userId, out List<string>? permissionCodes)
        {
            return _cache.TryGetValue(userId, out permissionCodes);
        }

        public void Remove(string userId)
        {
            _cache.Remove(userId);
        }
    }
}
