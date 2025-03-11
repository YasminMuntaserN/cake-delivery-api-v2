namespace cakeDelivery.DTO.FeedbackDTOs;
    public record FeedbackDto(
        string FeedbackID,
        string CustomerID,
        string Feedback,
        DateTime FeedbackDate,
        int Rating
    );