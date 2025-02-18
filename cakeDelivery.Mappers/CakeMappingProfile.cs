using AutoMapper;
using cakeDelivery.DTO.CakeDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class CakeMappingProfile : Profile
{
    public CakeMappingProfile()
    {
        CreateMap<CakeCreateDto, Cake>();

        CreateMap<CakeDTO, Cake>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Cake, CakeDTO>();
    }
}