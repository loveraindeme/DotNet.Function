using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DotNet.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton<RefreshTokenContainer>();
            builder.Services.AddSingleton<CustomJwtBearerEvents>();
            builder.Services.AddSingleton<UserPermissionContainer>();

            ConfigureAuthentication(builder.Services, builder.Configuration);
            ConfigureAuthorization(builder.Services);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API");
                });
            }

            app.UseStaticFiles();

            app.UseRouting();

            // Configure the HTTP request pipeline.

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers().RequireAuthorization();

            app.Run();
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection(JwtOptions.JwtOption);
            services.Configure<JwtOptions>(jwtConfig);
            var jwtOptions = new JwtOptions();
            jwtConfig.Bind(jwtOptions);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidTypes = [JwtConstants.HeaderType],
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Aes128CbcHmacSha256],

                    ValidateIssuerSigningKey = true,
                    // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignureSecretKey)),
                    IssuerSigningKeys = [new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignureSecretKey)), new RsaSecurityKey(RsaHelper.CreateFromPublicKeyPem(jwtOptions.RsaPublicKeyPem))],

                    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.EncryptSecretKey)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerValidator = (issuer, securityToken, validationParameters) =>
                    {
                        return issuer;
                    },

                    ValidateAudience = true,
                    RequireAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidAudiences = [jwtOptions.Audience],
                    AudienceValidator = (audiences, securityToken, validationParameters) =>
                    {
                        return audiences.Contains(jwtOptions.Audience);
                    },

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5),

                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                };

                jwtBearerOptions.EventsType = typeof(CustomJwtBearerEvents);
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnForbidden = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnChallenge = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = (context) =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization();

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationResultHandler>();
        }
    }
}
