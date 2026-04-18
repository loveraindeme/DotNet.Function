using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Localization;

namespace DotNet.Internationalization
{
    public static class InternationalizationExtension
    {
        public static void AddInternationalization(this IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            services.AddLocalization();
            services.AddSingleton<IStringLocalizer>((sp) =>
            {
                var localizer = sp.GetRequiredService<IStringLocalizer<MultiLanguage>>();
                return localizer;
            });
            // 配置数据注解的本地化资源文件来源
            mvcBuilder.AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(MultiLanguage));
            });
        }

        public static IApplicationBuilder UseInternationalization(this IApplicationBuilder app)
        {
            var supportLanguages = new[] { "zh-CN", "en-US" };

            // 设置文化和区域的默认值和支持的文化和区域列表
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportLanguages[0])
                .AddSupportedCultures(supportLanguages)
                .AddSupportedUICultures(supportLanguages);

            // 将当前文化信息自动添加到响应头中
            localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

            // 启用文化和区域确认中间件
            // 1.从查询字符串参数中的culture键和ui-culture键获取文化（默认）
            // 2.从cookie中的.AspNetCore.Culture键获取文化（默认）
            // 3.从Accept-Language请求头获取文化（默认）
            // 4.从路由数据中的键（可自定义名称，以下示例为culture和ui-culture）获取文化（可选）
            // 将获取到的文化分别赋值给CultureInfo.CurrentCulture和CultureInfo.CurrentUICulture
            // 以便在IStringLocalizer.GetString方法中根据当前文化返回对应的本地化内容
            localizationOptions.RequestCultureProviders.Add(new RouteDataRequestCultureProvider()
            {
                Options = localizationOptions,
                RouteDataStringKey = "culture",
                UIRouteDataStringKey = "ui-culture"
            });
            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }

    public class AppSettingsRequestCultureProvider : RequestCultureProvider
    {
        public string CultureKey { get; set; } = "Localization:Culture";

        public string UICultureKey { get; set; } = "Localization:UICulture";

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException();
            }

            string? culture = null;
            string? uiCulture = null;
            var configuration = httpContext.RequestServices.GetService<IConfigurationRoot>();
            if (configuration != null)
            {
                culture = configuration[CultureKey];
                uiCulture = configuration[UICultureKey];
            }
            
            if (culture == null && uiCulture == null)
            {
                return Task.FromResult((ProviderCultureResult?)null);
            }

            if (culture != null && uiCulture == null)
            {
                uiCulture = culture;
            }

            if (culture == null && uiCulture != null)
            {
                culture = uiCulture;
            }

            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult((ProviderCultureResult?)providerResultCulture);
        }
    }
}
