using cakeDelivery.Entities;
using FluentValidation;
using MongoDB.Bson;

namespace cakeDelivery.Validation;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(item => item.CustomerId)
            .NotEmpty().WithMessage("Customer Id is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Customer Id format.");
        
        RuleFor(order => order.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than zero.");

        RuleFor(order => order.OrderDate)
            .NotEmpty().WithMessage("Order date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future.");

        RuleFor(order => order.PaymentStatus)
            .NotEmpty().WithMessage("Payment status is required.")
            .Must(status => new[] { "Pending", "Completed", "Failed" }.Contains(status))
            .WithMessage("Invalid payment status. Must be: Pending, Completed, or Failed.");

        RuleFor(order => order.DeliveryStatus)
            .NotEmpty().WithMessage("Delivery status is required.")
            .Must(status => new[] { "Pending", "In Transit", "Delivered", "Cancelled" }.Contains(status))
            .WithMessage("Invalid delivery status. Must be: Pending, In Transit, Delivered, or Cancelled.");

        RuleFor(order => order.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item.");
    }
}
