using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Runtime.Loader;

namespace DotNet.FluentValidation
{
    public static class ValidationExtension
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            // 多个规则的多个校验者的级联行为Stop，在规则中有校验者执行失败时，不继续执行其他规则的校验者
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
            // 单个规则的多个校验者的级联行为Stop，在规则中有校验者执行失败时，不继续执行当前规则的其他校验者
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            services.AddFluentValidationAutoValidation(fv =>
            {
                fv.OverrideDefaultResultFactoryWith<FluentValidationResultFactory>();
            });

            return services;
        }

        /// <summary>
        /// 从指定校验器所在的程序集扫描并注册所有FluentValidation校验器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFluentValidatorFromAssemblyContaining<T>(this IServiceCollection services) where T : class, new()
        {
            services.AddValidatorsFromAssemblyContaining<T>();
            return services;
        }

        /// <summary>
        /// 从指定程序集名称列表中扫描并注册所有FluentValidation校验器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyNames">程序集名称列表</param>
        /// <returns></returns>
        public static IServiceCollection AddFluentValidatorFromAssemblies(this IServiceCollection services, params string[] assemblyNames)
        {
            var assemblies = assemblyNames
                .Select(name => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(name)))
                .ToArray();
            services.AddValidatorsFromAssemblies(assemblies);
            return services;
        }

        /// <summary>
        /// 从指定程序集中扫描并注册所有FluentValidation校验器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies">程序集列表</param>
        /// <returns></returns>
        public static IServiceCollection AddFluentValidatorFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddValidatorsFromAssemblies(assemblies);
            return services;
        }
    }
}
