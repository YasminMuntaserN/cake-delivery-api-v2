using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cakeDelivery.api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[SwaggerTag("Manage Category operations")]
public class CategoryController : BaseController
{
    private readonly CategoryService _categoryService;
    private static readonly string[] SearchFields = new[] { "categoryName", "categoryId" };

    public CategoryController(ILogger<CategoryController> logger, CategoryService categoryService)
        : base(logger)
    {
        _categoryService = categoryService;
    }


    
    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all Categories with pagination",
        Description = "Retrieves a paginated list of Categories. Results are ordered according to specified criteria. for example it must be[\"categoryName\", \"categoryId\"  ]  "
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved cakes", typeof(IEnumerable<CategoryDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll([FromQuery] PaginationDto pagination)
    {
        if (pagination.PageNumber < 1 || pagination.PageSize < 1)
        {
            return BadRequest(new { message = "Page number and page size must be greater than 0" });
        }

        if (!string.IsNullOrEmpty(pagination.OrderBy) && !SearchFields.Contains(pagination.OrderBy))
        {
            return BadRequest(new { message = $"OrderBy must be one of: {string.Join(", ", SearchFields)}" });
        }
        
        return await HandleResponse(async () =>
        {
            var (data, totalCount, totalPages) = await _categoryService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all categories");
    }

    
    
    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get Category by ID",
        Description = "Retrieves a specific Category by its unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved Category", typeof(CategoryDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "CategoryDTO not found")]
    public async Task<ActionResult<CategoryDTO>> GetById(string id)
    {
        return await HandleResponse(
            async () => await _categoryService.GetCategoryByIdAsync(id),
            $"Successfully retrieved category with ID {id}");
    }
    
    
    
    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search Categories",
        Description = "Search for Categories using specified criteria. Available search fields: categoryName, categoryId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<CategoryDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Search( 
        [FromBody] [SwaggerRequestBody("Search criteria")]
        SearchCriteriaDto criteria)
    {
        if (string.IsNullOrEmpty(criteria.Field) || string.IsNullOrEmpty(criteria.Value))
        {
            return BadRequest(new { message = "Search field and value are required" });
        }

        if (!SearchFields.Contains(criteria.Field))
        {
            return BadRequest(new { message = $"Search field must be one of: categoryName, categoryId" });
        }
        return await HandleResponse(
            async () => await _categoryService.SearchCategoryAsync(criteria),
            "Successfully performed search");
    }

    
    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check category existence",
        Description = "Check if a category exists based on specified criteria like: categoryName, categoryId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully checked existence", typeof(bool))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<bool>> Exists(
        [FromBody] [SwaggerRequestBody("Search criteria")]
        SearchCriteriaDto criteria)
    {
        if (string.IsNullOrEmpty(criteria.Field) || string.IsNullOrEmpty(criteria.Value))
        {
            return BadRequest(new { message = "Search field and value are required" });
        }

        if (!SearchFields.Contains(criteria.Field))
        {
            return BadRequest(new { message = $"Search field must be one of: categoryName, categoryId" });
        }
        return await HandleResponse(
            async () => await _categoryService.ExistsCategoryByAsync(criteria),
            "Successfully checked existence");
    }
    

    
    [HttpPost]
    [RequirePermission(Permissions.ManageCategories)]
    [SwaggerOperation(
        Summary = "Create a new criteria",
        Description = "Creates a new criteria with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "criteria created successfully", typeof(CategoryDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid cake data")]
    public async Task<ActionResult<CategoryDTO>> Create(
        [FromBody][SwaggerRequestBody("Category creation data")]
        CategoryCreateDto createCategoryDto)
    {
        return await HandleResponse(
            async () => await _categoryService.AddCategoryAsync(createCategoryDto),
            "Successfully created new category");
    }

    
    
    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageCategories)]
    [SwaggerOperation(
        Summary = "Update a category",
        Description = "Updates an existing category with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Category updated successfully", typeof(CategoryDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Category data")]
    public async Task<ActionResult<CategoryDTO>> Update(string id, [FromBody] CategoryDTO categoryDto)
    {
        return await HandleResponse(
            async () => await _categoryService.UpdateCategoryAsync(id, categoryDto),
            $"Successfully updated category with ID {id}");
    }

    
    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageCategories)]
    [SwaggerOperation(
        Summary = "Delete a category",
        Description = "Permanently deletes a category from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Category deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the category to delete")]string id)
    {
        return await HandleResponse(
            async () => await _categoryService.DeleteCategoryAsync(id),
            $"Successfully deleted category with ID {id}");
    }

}