using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace DotNet.FluentValidation
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            // 多个属性的多个规则的级联行为Stop，在验证属性的规则失败时，不继续验证其他属性的规则
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
            // 单个属性的多个规则的级联行为Stop，在验证属性的规则失败时，不继续验证当前属性的其他规则
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            services.AddFluentValidationAutoValidation(fv =>
            {
                fv.OverrideDefaultResultFactoryWith<FluentValidationResultFactory>();
            });

            return services;
        }
    }
}
