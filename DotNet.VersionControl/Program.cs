using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;

namespace DotNet.VersionControl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddApiVersioning(options =>
            {
                // 默认版本为1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // 当请求未指定版本时，使用默认版本
                options.AssumeDefaultVersionWhenUnspecified = true;
                // 在响应头中添加Api-Supported-Versions和Api-Deprecated-Versions属性以明确支持与弃用的版本信息
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    // 从查询字符串中确认版本
                    new QueryStringApiVersionReader("api-version"),
                    // 从请求头中属性ApiVersion确认版本
                    new HeaderApiVersionReader("ApiVersion"),
                    // 从请求的媒体类型中确认版本，例如：application/json;version=1.0
                    new MediaTypeApiVersionReader("version"),
                    // 从路由中确认版本
                    new UrlSegmentApiVersionReader());
            }).AddApiExplorer(options =>
            {
                // 文档组名格式
                options.GroupNameFormat = "'v'VVV";
                // 文档中将版本占位符替换成具体版本号 
                options.SubstituteApiVersionInUrl = true;
                // 文档中版本占位符替换格式
                options.SubstitutionFormat = "VVV";
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var versionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var versionDescription in versionDescriptionProvider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(versionDescription.GroupName, new OpenApiInfo
                    {
                        Title = $"WebApi {versionDescription.GroupName}",
                        Version = versionDescription.ApiVersion.ToString(),
                        Summary = ".NET WebApi documentation",
                        Description = ".NET WebApi documentation",
                        Contact = new OpenApiContact
                        {
                            Name = "loveraindeme",
                            Email = "614043899@qq.com",
                            Url = new Uri("https://www.cnblogs.com/rainseason")
                        }
                    });
                }
                Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    c.IncludeXmlComments(file, true);
                });
                c.OperationFilter<RemoveVersionOperationFilter>();
                // c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // c.OrderActionsBy(apiDescription => apiDescription.RelativePath);
                //c.DocInclusionPredicate((groupName, apiDescription) =>
                //{
                //    if (groupName.Equals(apiDescription.GroupName))
                //    {
                //        IEnumerable<string>? paths = apiDescription!.RelativePath
                //            ?.Split('/')
                //            .Select(v => v.Replace("v{version}", apiDescription.GroupName));
                //        apiDescription.RelativePath = string.Join("/", paths ?? []);
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //});
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var versionDescriptions in versionDescriptionProvider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{versionDescriptions.GroupName}/swagger.json", $"{versionDescriptions.GroupName}");
                    }
                });
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
