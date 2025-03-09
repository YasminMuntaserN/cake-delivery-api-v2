using cakeDelivery.Entities;
using FluentValidation;

namespace cakeDelivery.Validation;

public class CustomerFeedbackValidator : AbstractValidator<CustomerFeedback>
{
    public CustomerFeedbackValidator()
    {

        RuleFor(feedback => feedback.Feedback)
            .NotEmpty().WithMessage("Feedback content is required.");

        RuleFor(feedback => feedback.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(feedback => feedback.FeedbackDate)
            .NotEmpty().WithMessage("Feedback date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Feedback date cannot be in the future.");
    }
}