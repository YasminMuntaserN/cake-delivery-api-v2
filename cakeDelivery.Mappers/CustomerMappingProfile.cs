using AutoMapper;
using cakeDelivery.DTO.CustomerDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<CustomerCreateDTO, Customer>();

        CreateMap<CustomerDTO, Customer>();

        CreateMap<Customer, CustomerDTO>();
    }
}