using cakeDelivery.Entities;
using FluentValidation;
using MongoDB.Bson;

namespace cakeDelivery.Validation;

public class PaymentValidator : AbstractValidator<Payment>
{
    public PaymentValidator()
    {
        RuleFor(item => item.OrderId)
            .NotEmpty().WithMessage("Order ID is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Order ID format.");

        RuleFor(payment => payment.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.")
            .MaximumLength(20).WithMessage("Payment method cannot exceed 20 characters.");

        RuleFor(payment => payment.PaymentDate)
            .NotEmpty().WithMessage("Payment date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Payment date cannot be in the future.");

        RuleFor(payment => payment.AmountPaid)
            .GreaterThan(0).WithMessage("Amount paid must be greater than zero.");

        RuleFor(payment => payment.PaymentStatus)
            .NotEmpty().WithMessage("Payment status is required.")
            .Must(status => new[] { "Pending", "Completed", "Failed" }.Contains(status))
            .WithMessage("Invalid payment status. Must be: Pending, Completed, or Failed.");
    }
}