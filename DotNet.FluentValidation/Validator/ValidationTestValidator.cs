using DotNet.FluentValidation.Dtos;
using FluentValidation;

namespace DotNet.FluentValidation.Validator
{
    public class ValidationTestValidator : AbstractValidator<ValidationTestDto>
    {
        public ValidationTestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(64).WithMessage("Name cannot exceed 64 characters.")
                .Matches(@"^[\u4e00-\u9fa5A-Za-z0-9_ ]+$").WithMessage("Name must not contain special characters.");
            RuleFor(x => x.Age)
                .InclusiveBetween(0, 150).WithMessage("Age must be between 0 and 150.");
        }
    }
}
