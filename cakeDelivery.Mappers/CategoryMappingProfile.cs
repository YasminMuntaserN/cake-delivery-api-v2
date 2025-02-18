using AutoMapper;
using cakeDelivery.DTO.CategoryDTOs;
using cakeDelivery.Entities;

namespace cakeDelivery.Mappers;

public class CategoryMappingProfile: Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CategoryCreateDto, Category>();

        CreateMap<CategoryDTO, Category>();

        CreateMap<Category, CategoryDTO>();
    }
}