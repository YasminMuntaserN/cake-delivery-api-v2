using AutoMapper;
using cakeDelivery.DTO.OrderItemDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class OrderItemMappingProfile : Profile
{
    public OrderItemMappingProfile()
    {
        CreateMap<OrderItemCreateDTO, OrderItem>();

        CreateMap<OrderItemDTO, OrderItem>();

        CreateMap<OrderItem, OrderItemDTO>();
    }
}