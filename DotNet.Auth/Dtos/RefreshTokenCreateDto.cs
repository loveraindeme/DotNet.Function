namespace DotNet.Auth.Dtos
{
    public class RefreshTokenCreateDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
