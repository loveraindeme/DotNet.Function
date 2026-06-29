namespace DotNet.Auth
{
    public class JwtOptions
    {
        public const string JwtOption = "JWT";

        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// 加密秘钥
        /// </summary>
        public string EncryptSecretKey { get; set; } = string.Empty;

        /// <summary>
        /// 签名算法
        /// </summary>
        public string SigningAlgorithm { get; set; } = string.Empty;

        /// <summary>
        /// 默认签名秘钥
        /// </summary>
        public string SignureSecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Rsa公钥
        /// </summary>
        public string RsaPublicKeyPem { get; set; } = string.Empty;

        /// <summary>
        /// Rsa私钥
        /// </summary>
        public string RsaPrivateKeyPem { get; set; } = string.Empty;

        /// <summary>
        /// 访问Token过期时间（单位：分钟）
        /// </summary>
        public int AccessMinutes { get; set; }

        /// <summary>
        /// 刷新Token过期时间（单位：小时）
        /// </summary>
        public int RefreshHours { get; set; }
    }
}
