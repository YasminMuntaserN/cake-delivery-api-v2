using cakeDelivery.Entities;
using FluentValidation;

namespace cakeDelivery.Validation;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(category => category.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(255).WithMessage("Category name cannot exceed 255 characters.");

        RuleFor(category => category.CategoryImageUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(category => !string.IsNullOrEmpty(category.CategoryImageUrl))
            .WithMessage("Category image URL must be a valid URL.");
    }
}
