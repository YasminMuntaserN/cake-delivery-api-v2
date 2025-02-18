using AutoMapper;
using cakeDelivery.DTO.DeliveryDTO;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class DeliveryMappingProfile : Profile
{
    public DeliveryMappingProfile()
    {
        CreateMap<DeliveryCreateDTO,Delivery>();

        CreateMap<DeliveryDTO,Delivery>();

        CreateMap<Delivery, DeliveryDTO>();
    }
}