using cakeDelivery.Entities;
using FluentValidation;
using MongoDB.Bson;

namespace cakeDelivery.Validation;

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(item => item.OrderItemId)
            .NotEmpty().WithMessage("ID is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid ID format.");

        RuleFor(item => item.OrderId)
            .NotEmpty().WithMessage("Order ID is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Order ID format.");

        RuleFor(item => item.CakeId)
            .NotEmpty().WithMessage("Cake ID is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Cake ID format.");

        RuleFor(item => item.SizeId)
            .NotEmpty().WithMessage("Size ID is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Size ID format.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(item => item.PricePerItem)
            .GreaterThan(0).WithMessage("Price per item must be greater than zero.");
        
    }
}

