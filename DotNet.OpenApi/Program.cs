using DotNet.OpenApi.Enums;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNet.OpenApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            builder.Services.AddOutputCache(options =>
            {
                options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(60)));
            });

            builder.Services.AddOpenApi("internal", options =>
            {
                options.ShouldInclude = (description) =>
                {
                    return description.GroupName == "internal";
                };
            });
            builder.Services.AddOpenApi("external");
            builder.Services.AddOpenApi("common", options =>
            {
                options.ShouldInclude = (description) =>
                {
                    return description.GroupName == null;
                };
            });

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(
                    new JsonStringEnumConverter<Ball>());
                options.SerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.AllowTrailingCommas = true;
                options.SerializerOptions.PropertyNamingPolicy =
                    JsonNamingPolicy.CamelCase;

            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter<Ball>());
                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseOutputCache();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi("/openapi/{documentName}.json").CacheOutput();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/openapi/internal.json", "internal");
                    c.SwaggerEndpoint("/openapi/external.json", "external");
                    c.SwaggerEndpoint("/openapi/common.json", "common");
                });
            }

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }
}
