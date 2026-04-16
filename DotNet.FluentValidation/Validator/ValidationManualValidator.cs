using DotNet.FluentValidation.Dtos;
using FluentValidation;

namespace DotNet.FluentValidation.Validator
{
    public class ValidationManualValidator : AbstractValidator<ValidationManualDto>
    {
        public ValidationManualValidator()
        {
            RuleFor(x => x.Remark)
                .MaximumLength(256).WithMessage("Remark cannot exceed 256 characters.");
        }
    }
}
