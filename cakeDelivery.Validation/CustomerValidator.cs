using cakeDelivery.Entities;
using FluentValidation;

namespace cakeDelivery.Validation;

public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z ]*$").WithMessage("First name can only contain letters and spaces.");

            RuleFor(customer => customer.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z ]*$").WithMessage("Last name can only contain letters and spaces.");

            RuleFor(customer => customer.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(customer => customer.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

            RuleFor(customer => customer.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(customer => customer.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

            RuleFor(customer => customer.PostalCode)
                .NotEmpty().WithMessage("Postal code is required.")
                .MaximumLength(10).WithMessage("Postal code cannot exceed 10 characters.");

            RuleFor(customer => customer.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");

            RuleFor(customer => customer.CreatedAt)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Created date cannot be in the future.");
        }
    }