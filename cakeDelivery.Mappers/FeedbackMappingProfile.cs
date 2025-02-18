using AutoMapper;
using cakeDelivery.DTO.FeedbackDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class FeedbackMappingProfile : Profile
{
    public FeedbackMappingProfile()
    {
        CreateMap<FeedbackCreateDto, CustomerFeedback>();

        CreateMap<FeedbackDto, CustomerFeedback>();

        CreateMap<CustomerFeedback, FeedbackDto>();
    }
}