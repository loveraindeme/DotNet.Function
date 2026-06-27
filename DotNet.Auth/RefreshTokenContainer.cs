using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DotNet.Auth
{
    public class RefreshTokenContainer
    {
        private readonly IMemoryCache _cache;
        private readonly JwtOptions _jwtOptions;

        public RefreshTokenContainer(IMemoryCache cache,
            IOptions<JwtOptions> jwtOptions)
        {
            _cache = cache;
            _jwtOptions = jwtOptions.Value;
        }

        public void Set(string refreshTokenkey, string refreshTokenValue)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_jwtOptions.RefreshHours)
            };
            _cache.Set(refreshTokenkey, refreshTokenValue, options);
        }

        public bool TryGet(string refreshTokenkey, out string? refreshTokenValue)
        {
            return _cache.TryGetValue(refreshTokenkey, out refreshTokenValue);
        }

        public void Remove(string refreshTokenkey)
        {
            _cache.Remove(refreshTokenkey);
        }
    }
}
