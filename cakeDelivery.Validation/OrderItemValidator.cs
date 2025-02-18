using cakeDelivery.Entities;
using FluentValidation;

namespace cakeDelivery.Validation;

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(item => item.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be a positive integer.");

        RuleFor(item => item.CakeId)
            .GreaterThan(0).WithMessage("Cake ID must be a positive integer.");

        RuleFor(item => item.SizeId)
            .GreaterThan(0).WithMessage("Size ID must be a positive integer.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(item => item.PricePerItem)
            .GreaterThan(0).WithMessage("Price per item must be greater than zero.");
    }
}
