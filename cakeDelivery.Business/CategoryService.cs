using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DTO;
using cakeDelivery.DTO.CategoryDTOs;
using cakeDelivery.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace cakeDelivery.Business;

public class CategoryService : BaseService<Category, CategoryDTO>
{
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<Category> _validator;

    public CategoryService(
        IMongoDatabase database,
        ILogger<CategoryService> logger,
        IMapper mapper,
        IValidator<Category> validator)
        : base(database, "categories", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CategoryDTO> AddCategoryAsync(CategoryCreateDto createCategoryDto)
        => await AddAsync(createCategoryDto, "Category");

    public async Task<CategoryDTO?> UpdateCategoryAsync(string id, CategoryDTO categoryDetailsDto)
        => await UpdateAsync(id, categoryDetailsDto, "Category");

    public async Task<CategoryDTO?> GetCategoryByIdAsync(string id)
        => await FindBy(c => c.CategoryId == id);

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<CategoryDTO> Data, long TotalCount, int TotalPages)> GetAllAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<Category, object>> orderBy = paginationDto.OrderBy switch
        {
            "categoryName" => category => category.CategoryName,
            _ => category => category.CategoryId
        };

        return await GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize, orderBy, paginationDto.Ascending);
    }

    public async Task<bool> DeleteCategoryAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeleteCategoryByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Category, bool>> predicate = criteria.Field.ToLower() switch
        {
            "categoryName" => c => c.CategoryName == criteria.Value,
            _ => c => c.CategoryId == criteria.Value,
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsCategoryAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsCategoryByAsync(SearchCriteriaDto criteria)
        => criteria.Field.ToLower() switch
        {
            "categoryName" => await ExistsByAsync(c => c.CategoryName == criteria.Value),
            _ =>  await ExistsByAsync(c => c.CategoryId == criteria.Value),
        };

    public async Task<IEnumerable<CategoryDTO>> SearchCategoryAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Category, bool>> predicate = criteria.Field switch
        {
            "categoryName" => c => c.CategoryName.Contains(criteria.Value),
            _ => c => c.CategoryId == criteria.Value,
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<CategoryDTO>();
    }
}
