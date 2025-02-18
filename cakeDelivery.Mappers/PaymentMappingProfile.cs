using AutoMapper;
using cakeDelivery.DTO.PaymentDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<PaymentCreateDTO, Payment>();

        CreateMap<PaymentDTO, Payment>();

        CreateMap<Payment, PaymentDTO>();
    }
}