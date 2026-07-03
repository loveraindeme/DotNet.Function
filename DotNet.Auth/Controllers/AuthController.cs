using DotNet.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtOptions _jwtOptions;
        private readonly RefreshTokenContainer _refreshTokenContainer;
        private readonly UserPermissionContainer _userPermissionContainer;

        public AuthController(IOptions<JwtOptions> jwtOptions,
            RefreshTokenContainer refreshTokenContainer,
            UserPermissionContainer userPermissionContainer)
        {
            _jwtOptions = jwtOptions.Value;
            _refreshTokenContainer = refreshTokenContainer;
            _userPermissionContainer = userPermissionContainer;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<TokenDto> LoginAsync(LoginCreateDto input)
        {
            if (input.Account != input.Password)
            {
                throw new BadHttpRequestException("无效的登录凭证");
            }

            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Account = input.Account
            };

            var (refreshTokenId, refreshToken) = GeneratedRefreshToken(user);
            var accessToken = GeneratedAccessToken(user, refreshTokenId);

            // 模拟获取用户权限，并存储在内存缓存中
            _userPermissionContainer.Set($"{user.Id}&permission", ["user:list"]);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        [HttpPost("logout")]
        public async Task LogoutAsync()
        {
            var userId = User.FindFirst(AuthClaimTypes.UserId)?.Value;
            var refreshTokenId = User.FindFirst(AuthClaimTypes.RefreshTokenId)?.Value;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(refreshTokenId))
            {
                _refreshTokenContainer.Remove($"{userId}&{refreshTokenId}");
                _userPermissionContainer.Remove($"{userId}&permission");
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenCreateDto input)
        {
            var claimsPrincipal = ValidateAndAnalysisToken(input.AccessToken);
            if (claimsPrincipal == null)
            {
                throw new BadHttpRequestException("无效的访问令牌");
            }

            var userId = claimsPrincipal.FindFirst(AuthClaimTypes.UserId)?.Value;
            var refreshTokenId = claimsPrincipal.FindFirst(AuthClaimTypes.RefreshTokenId)?.Value;
            var account = claimsPrincipal.FindFirst(AuthClaimTypes.UserAccount)?.Value;

            var refreshTokeKey = $"{userId}&{refreshTokenId}";
            var canRefresh = _refreshTokenContainer.TryGet(refreshTokeKey, out string? refreshToken);
            if (!canRefresh)
            {
                throw new BadHttpRequestException("刷新令牌已过期");
            }
            if (refreshToken != input.RefreshToken)
            {
                throw new BadHttpRequestException("无效的刷新令牌");
            }

            _refreshTokenContainer.Remove(refreshTokeKey);

            var user = new UserDto
            {
                Id = Guid.Parse(userId!),
                Account = account!
            };

            var (newRefreshTokenId, newRefreshToken) = GeneratedRefreshToken(user);
            var newAccessToken = GeneratedAccessToken(user, newRefreshTokenId);

            return new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        private (string refreshTokenId, string refreshToken) GeneratedRefreshToken(UserDto user)
        {
            var refreshTokenId = Guid.NewGuid().ToString();
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            _refreshTokenContainer.Set($"{user.Id}&{refreshTokenId}", refreshToken);

            return (refreshTokenId, refreshToken);
        }

        private string GeneratedAccessToken(UserDto user, string refreshTokenId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(AuthClaimTypes.UserId, user.Id.ToString()),
                    new Claim(AuthClaimTypes.UserAccount, user.Account),
                    new Claim(AuthClaimTypes.RefreshTokenId, refreshTokenId)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessMinutes),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                EncryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.EncryptSecretKey)), JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes128CbcHmacSha256),
                SigningCredentials = CreateSigningCredentials()
            };

            var securityToken = tokenHandler.CreateToken(tokenDescripor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        private SigningCredentials CreateSigningCredentials()
        {
            if (string.Equals(_jwtOptions.SigningAlgorithm, SecurityAlgorithms.RsaSha256, StringComparison.OrdinalIgnoreCase))
            {
                var rsa = RsaHelper.CreateFromPrivateKeyPem(_jwtOptions.RsaPrivateKeyPem);
                return new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            }

            var symmetricSecurityKey = Encoding.UTF8.GetBytes(_jwtOptions.SignureSecretKey);
            return new SigningCredentials(new SymmetricSecurityKey(symmetricSecurityKey), SecurityAlgorithms.HmacSha256);
        }

        private ClaimsPrincipal? ValidateAndAnalysisToken(string token)
        {
            try
            {
                var signureSecurityKey = Encoding.UTF8.GetBytes(_jwtOptions.SignureSecretKey);
                var rsa = RsaHelper.CreateFromPublicKeyPem(_jwtOptions.RsaPublicKeyPem);
                var encryptSecurityKey = Encoding.UTF8.GetBytes(_jwtOptions.EncryptSecretKey);

                var securityTokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = securityTokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidTypes = [JwtConstants.HeaderType],
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Aes128CbcHmacSha256],

                    ValidateIssuerSigningKey = true,
                    // IssuerSigningKey = new SymmetricSecurityKey(signureSecurityKey),
                    IssuerSigningKeys = [new SymmetricSecurityKey(signureSecurityKey), new RsaSecurityKey(rsa)],

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptSecurityKey),

                    ValidateAudience = true,
                    ValidAudience = _jwtOptions.Audience,

                    ValidateIssuer = true,
                    ValidIssuer = _jwtOptions.Issuer,

                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.FromMinutes(5),

                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                }, out SecurityToken _);

                return claimsPrincipal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
