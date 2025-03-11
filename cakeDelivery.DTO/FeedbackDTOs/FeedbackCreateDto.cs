namespace cakeDelivery.DTO.FeedbackDTOs;
    public record FeedbackCreateDto(
        string CustomerID,
        string Feedback,
        DateTime FeedbackDate,
        int Rating
    );
