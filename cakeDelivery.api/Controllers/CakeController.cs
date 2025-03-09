using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.CakeDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cakeDelivery.api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Manage cake operations")]
public class CakeController : BaseController
{
    private readonly CakeService _cakeService;
    private static readonly string[] SearchFields = new[] { "CakeName", "CreatedAt", "StockQuantity", "Price" };

    public CakeController(ILogger<CakeController> logger, CakeService cakeService)
        : base(logger)
    {
        _cakeService = cakeService;
    }


    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all cakes with pagination",
        Description = "Retrieves a paginated list of cakes. Results are ordered according to specified criteria. for example it must be[\"CakeName\", \"CreatedAt\", \"StockQuantity\", \"Price\" ]  "
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved cakes", typeof(IEnumerable<CakeDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<CakeDTO>>> GetAll(
        [FromQuery] [SwaggerParameter("Pagination parameters")]
        PaginationDto pagination)
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
            var (data, totalCount, totalPages) = await _cakeService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all cakes");
    }


    
    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get cake by ID",
        Description = "Retrieves a specific cake by its unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved cake", typeof(CakeDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Cake not found")]
    public async Task<ActionResult<CakeDTO>> GetById(
        [SwaggerParameter("The ID of the cake to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _cakeService.GetCakeByIdAsync(id),
            $"Successfully retrieved cake with ID {id}");
    }


    
    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search cakes",
        Description = "Search for cakes using specified criteria. Available search fields: CakeName, CakeId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<CakeDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<CakeDTO>>> Search(
        [FromBody] [SwaggerRequestBody("Search criteria")]
        SearchCriteriaDto criteria)
    {
        if (string.IsNullOrEmpty(criteria.Field) || string.IsNullOrEmpty(criteria.Value))
        {
            return BadRequest(new { message = "Search field and value are required" });
        }

        if (!SearchFields.Contains(criteria.Field))
        {
            return BadRequest(new { message = $"Search field must be one of: CakeName, CakeId" });
        }

        return await HandleResponse(
            async () => await _cakeService.SearchCakeAsync(criteria),
            "Successfully performed search");
    }


    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check cake existence",
        Description = "Check if a cake exists based on specified criteria like: CakeName, CakeId"
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

        return await HandleResponse(
            async () => await _cakeService.ExistsCakeByAsync(criteria),
            "Successfully checked existence");
    }


    
    [HttpPost]
    [RequirePermission(Permissions.ManageCakes)]
    [SwaggerOperation(
        Summary = "Create a new cake",
        Description = "Creates a new cake with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Cake created successfully", typeof(CakeDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid cake data")]
    public async Task<ActionResult<CakeDTO>> Create(
        [FromBody] [SwaggerRequestBody("Cake creation data")]
        CakeCreateDto createCakeDto)
    {
        return await HandleResponse(
            async () => await _cakeService.AddCakeAsync(createCakeDto),
            "Successfully created new cake");
    }


    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageCakes)]
    [SwaggerOperation(
        Summary = "Update a cake",
        Description = "Updates an existing cake with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Cake updated successfully", typeof(CakeDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Cake not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid cake data")]
    public async Task<ActionResult<CakeDTO>> Update(
        [SwaggerParameter("The ID of the cake to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated cake data")]
        CakeDTO cakeDto)
    {
        return await HandleResponse(
            async () => await _cakeService.UpdateCakeAsync(id, cakeDto),
            $"Successfully updated cake with ID {id}");
    }


    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageCakes)]
    [SwaggerOperation(
        Summary = "Delete a cake",
        Description = "Permanently deletes a cake from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Cake deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Cake not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the cake to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _cakeService.HardDeleteCakeAsync(id),
            $"Successfully deleted cake with ID {id}");
    }
    
}