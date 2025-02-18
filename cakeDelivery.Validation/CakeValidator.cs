using cakeDelivery.Entities;
using FluentValidation;

namespace cakeDelivery.Validation;

public class CakeValidator : AbstractValidator<Cake>
{
    public CakeValidator()
    {
        RuleFor(cake => cake.CakeName)
            .NotEmpty().WithMessage("Cake name is required.")
            .MaximumLength(100).WithMessage("Cake name cannot exceed 100 characters.");

        RuleFor(cake => cake.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(cake => !string.IsNullOrEmpty(cake.Description));

        RuleFor(cake => cake.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(cake => cake.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

        RuleFor(cake => cake.CategoryId)
            .GreaterThan(0).WithMessage("Category ID must be a positive integer.");

        RuleFor(cake => cake.ImageUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(cake => !string.IsNullOrEmpty(cake.ImageUrl))
            .WithMessage("Image URL must be a valid URL.");

        RuleFor(cake => cake.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Created date cannot be in the future.");
    }
}
