using System.Security.Cryptography;

namespace DotNet.Auth
{
    public sealed record RsaKeyPair(string PrivateKeyPem, string PublicKeyPem);

    public static class RsaHelper
    {
        private static RSA? publicRsa;
        private static RSA? privateRsa;

        public static RsaKeyPair GenerateKeyPair(int keySize = 2048)
        {
            if (keySize < 2048)
            {
                throw new ArgumentOutOfRangeException(nameof(keySize), "RSA秘钥长度不能小于2048");
            }

            using var rsa = RSA.Create(keySize);
            return new RsaKeyPair(rsa.ExportPkcs8PrivateKeyPem(), rsa.ExportSubjectPublicKeyInfoPem());
        }

        public static RSA CreateFromPrivateKeyPem(string privateKeyPem)
        {
            if (privateRsa == null)
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(privateKeyPem);
                privateRsa = rsa;
            }

            return privateRsa;
        }

        public static RSA CreateFromPublicKeyPem(string publicKeyPem)
        {
            if (publicRsa == null)
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(publicKeyPem);
                publicRsa = rsa;
            }

            return publicRsa;
        }
    }
}
