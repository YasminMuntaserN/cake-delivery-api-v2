using AutoMapper;
using cakeDelivery.DTO.OrderDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderCreateDTO, Order>();

        CreateMap<OrderDTO, Order>();

        CreateMap<Order, OrderDTO>();
    }
}