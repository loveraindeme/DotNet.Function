using DotNet.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.Auth.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class RsaController
    {
        public RsaController()
        {

        }

        [HttpPost("rsa-key-pair")]
        public async Task<RsaKeyPairDto> GenerateRsaKeyPairAsync()
        {
            var rsaKeyPair = RsaHelper.GenerateKeyPair();
            return new RsaKeyPairDto
            {
                PublicKey = rsaKeyPair.PublicKeyPem,
                PrivateKey = rsaKeyPair.PrivateKeyPem
            };
        }
    }
}
