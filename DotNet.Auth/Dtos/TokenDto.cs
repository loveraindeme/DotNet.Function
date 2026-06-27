namespace DotNet.Auth.Dtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken {  get; set; } = string.Empty;
    }
}
