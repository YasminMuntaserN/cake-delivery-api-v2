using cakeDelivery.Entities;
using FluentValidation;
using MongoDB.Bson;

namespace cakeDelivery.Validation;

public class DeliveryValidator : AbstractValidator<Delivery>
{
    public DeliveryValidator()
    {
        RuleFor(item => item.OrderId)
            .NotEmpty().WithMessage("Order ID is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Order ID format.");
        
        RuleFor(delivery => delivery.DeliveryAddress)
            .NotEmpty().WithMessage("Delivery address is required.")
            .MaximumLength(50).WithMessage("Delivery address cannot exceed 50 characters.");

        RuleFor(delivery => delivery.DeliveryCity)
            .NotEmpty().WithMessage("Delivery city is required.")
            .MaximumLength(50).WithMessage("Delivery city cannot exceed 50 characters.");

        RuleFor(delivery => delivery.DeliveryPostalCode)
            .NotEmpty().WithMessage("Delivery postal code is required.")
            .MaximumLength(10).WithMessage("Delivery postal code cannot exceed 10 characters.");

        RuleFor(delivery => delivery.DeliveryCountry)
            .NotEmpty().WithMessage("Delivery country is required.")
            .MaximumLength(50).WithMessage("Delivery country cannot exceed 50 characters.");

        RuleFor(delivery => delivery.DeliveryDate)
            .NotEmpty().WithMessage("Delivery date is required.");

        RuleFor(delivery => delivery.DeliveryStatus)
            .NotEmpty().WithMessage("Delivery status is required.")
            .Must(status => new[] { "Scheduled", "In Transit", "Delivered", "Cancelled" }.Contains(status))
            .WithMessage("Invalid delivery status. Must be: Scheduled, In Transit, Delivered, or Cancelled.");
    }
}
