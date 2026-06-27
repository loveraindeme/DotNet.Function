using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DotNet.Auth
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        public override Task MessageReceived(MessageReceivedContext context)
        {
            return Task.CompletedTask;
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            return Task.CompletedTask;
        }

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            return Task.CompletedTask;
        }

        public override Task Forbidden(ForbiddenContext context)
        {
            return Task.CompletedTask;
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
