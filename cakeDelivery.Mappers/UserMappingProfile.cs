using AutoMapper;
using cakeDelivery.DTO.UserDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserCreateDTO, User>();

        CreateMap<UserDTO, User>();

        CreateMap<User, UserDTO>();
    }
}