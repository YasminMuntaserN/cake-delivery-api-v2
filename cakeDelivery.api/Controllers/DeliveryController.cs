using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.DeliveryDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[SwaggerTag("Manage delivery operations")]
public class DeliveryController : BaseController
{
    private readonly DeliveryService _deliveryService;
    private static readonly string[] SearchFields = new[] { "DeliveryId", "OrderId", "DeliveryCity", "DeliveryStatus" };

    public DeliveryController(ILogger<DeliveryController> logger, DeliveryService deliveryService)
        : base(logger)
    {
        _deliveryService = deliveryService;
    }

    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all deliveries with pagination",
        Description = "Retrieves a paginated list of deliveries with sorting options. Available fields: DeliveryId, OrderId, DeliveryCity, DeliveryStatus."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved deliveries", typeof(IEnumerable<DeliveryDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<DeliveryDTO>>> GetAll(
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
            var (data, totalCount, totalPages) = await _deliveryService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all deliveries");
    }

    
    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get delivery by ID",
        Description = "Retrieves a specific delivery by its unique identifier."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved delivery", typeof(DeliveryDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Delivery not found")]
    public async Task<ActionResult<DeliveryDTO>> GetById(
        [SwaggerParameter("The ID of the delivery to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _deliveryService.GetDeliveryByIdAsync(id),
            $"Successfully retrieved delivery with ID {id}");
    }

    
    
    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search deliveries",
        Description = "Search for deliveries using specified criteria. Available fields: DeliveryId, OrderId, DeliveryCity, DeliveryStatus."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<DeliveryDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<DeliveryDTO>>> Search(
        [FromBody] [SwaggerRequestBody("Search criteria")]
        SearchCriteriaDto criteria)
    {
        if (string.IsNullOrEmpty(criteria.Field) || string.IsNullOrEmpty(criteria.Value))
        {
            return BadRequest(new { message = "Search field and value are required" });
        }

        if (!SearchFields.Contains(criteria.Field))
        {
            return BadRequest(new { message = $"Search field must be one of: {string.Join(", ", SearchFields)}" });
        }

        return await HandleResponse(
            async () => await _deliveryService.SearchDeliveryAsync(criteria),
            "Successfully performed search");
    }

    
    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check delivery existence",
        Description = "Check if a delivery exists based on specified criteria."
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
            async () => await _deliveryService.ExistsDeliveryByAsync(criteria),
            "Successfully checked existence");
    }

    
    [HttpPost]
    [RequirePermission(Permissions.ManageDeliveries)]
    [SwaggerOperation(
        Summary = "Create a new delivery",
        Description = "Creates a new delivery with the provided details."
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Delivery created successfully", typeof(DeliveryDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid delivery data")]
    public async Task<ActionResult<DeliveryDTO>> Create(
        [FromBody] [SwaggerRequestBody("Delivery creation data")]
        DeliveryCreateDTO createDeliveryDto)
    {
        return await HandleResponse(
            async () => await _deliveryService.AddDeliveryAsync(createDeliveryDto),
            "Successfully created new delivery");
    }

    
    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageDeliveries)]
    [SwaggerOperation(
        Summary = "Update a delivery",
        Description = "Updates an existing delivery with the provided details."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Delivery updated successfully", typeof(DeliveryDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Delivery not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid delivery data")]
    public async Task<ActionResult<DeliveryDTO>> Update(
        [SwaggerParameter("The ID of the delivery to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated delivery data")]
        DeliveryDTO deliveryDto)
    {
        return await HandleResponse(
            async () => await _deliveryService.UpdateDeliveryAsync(id, deliveryDto),
            $"Successfully updated delivery with ID {id}");
    }

    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageDeliveries)]
    [SwaggerOperation(
        Summary = "Delete a delivery",
        Description = "Permanently deletes a delivery from the system."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Delivery deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Delivery not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the delivery to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _deliveryService.DeleteDeliveryAsync(id),
            $"Successfully deleted delivery with ID {id}");
    }
    
}