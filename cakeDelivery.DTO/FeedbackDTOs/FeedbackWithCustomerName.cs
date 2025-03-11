namespace cakeDelivery.DTO.FeedbackDTOs;
    public record FeedbackWithCustomerName(
        string FeedbackID,
        string CustomerName,
        string Feedback,
        DateTime FeedbackDate,
        int Rating
    );