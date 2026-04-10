using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace DotNet.FluentValidation
{
    public class FluentValidationResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public Task<IActionResult?> CreateActionResult(ActionExecutingContext context, ValidationProblemDetails validationProblemDetails, IDictionary<IValidationContext, ValidationResult> validationResults)
        {
            // 获取FluentValidation校验器中的错误信息
            // 当前示例中仅返回第一个错误信息
            var message = validationResults
                .SelectMany(kv => kv.Value.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? string.Empty;

            // 如果FluentValidation校验器中没有错误信息，则尝试从ModelState中获取错误信息
            // 用于兼容DataAnnotations和FluentValidation混合使用的场景以及模型绑定错误的场景
            // 当请求的参数类型不正确时，例如将"abc"字符串绑定到int类型属性中，模型绑定会失败并产生错误
            // 此时FluentValidation的校验器不会被执行，需要从ModelState中获取错误信息
            if (string.IsNullOrEmpty(message) && validationProblemDetails?.Errors?.Any() == true)
            {
                message = validationProblemDetails.Errors
                    .SelectMany(e => e.Value)
                    .FirstOrDefault() ?? string.Empty;
            }

            // 自定义返回结果
            var result = new
            {
                Code = 1,
                Msg = message
            };

            return Task.FromResult<IActionResult?>(new JsonResult(result));
        }
    }
}
