using cakeDelivery.Entities;
using cakeDelivery.Validation;
using FluentValidation;

namespace UserDelivery.Validation;

public class UserValidator : AbstractValidator<User>
{
    private readonly UniqueValidatorService _uniqueValidatorService;

    public UserValidator(UniqueValidatorService uniqueValidatorService)
    {
        _uniqueValidatorService = uniqueValidatorService;
     /*   RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .Length(5, 100).WithMessage("Email must be between 5 and 100 characters.")
            .MustAsync(async (email, cancellation) => await _uniqueValidatorService.IsEmailUniqueAsync(email)).WithMessage("Email is already in use.");

        RuleFor(user => user.PasswordHash)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 255).WithMessage("Password must be at least 8 characters long.");
        
        RuleFor(User => User.Role)
            .Must(role => new[] { "Admin", "Manager", "User"}.Contains(role))
            .MaximumLength(100).WithMessage("User name cannot exceed 100 characters.");*/
    }
}
