using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.OrderItemDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cakeDelivery.api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[SwaggerTag("Manage order item operations")]
public class OrderItemController : BaseController
{
    private readonly OrderItemService _orderItemService;
    private static readonly string[] SearchFields = new[] { "OrderId", "CakeId", "SizeId", "OrderItemId", "Quantity" };

    public OrderItemController(ILogger<OrderItemController> logger, OrderItemService orderItemService)
        : base(logger)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all order items with pagination",
        Description = "Retrieves a paginated list of order items. Sortable fields: OrderId, CakeId, SizeId, OrderItemId, Quantity"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved order items", typeof(IEnumerable<OrderItemDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetAll(
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
            var (data, totalCount, totalPages) = await _orderItemService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all order items");
    }

    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get order item by ID",
        Description = "Retrieves a specific order item by its unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved order item", typeof(OrderItemDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order item not found")]
    public async Task<ActionResult<OrderItemDTO>> GetById(
        [SwaggerParameter("The ID of the order item to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _orderItemService.GetOrderItemByIdAsync(id),
            $"Successfully retrieved order item with ID {id}");
    }

    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search order items",
        Description = "Search for order items using specified criteria. Available search fields: OrderId, CakeId, SizeId, OrderItemId, Quantity"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<OrderItemDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<OrderItemDTO>>> Search(
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
            async () => await _orderItemService.SearchOrderItemAsync(criteria),
            "Successfully performed search");
    }

    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check order item existence",
        Description = "Check if an order item exists based on specified criteria"
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
            async () => await _orderItemService.ExistsOrderItemByAsync(criteria),
            "Successfully checked existence");
    }

    
    [HttpPost]
    [RequirePermission(Permissions.ManageOrders)]
    [SwaggerOperation(
        Summary = "Create a new order item",
        Description = "Creates a new order item with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Order item created successfully", typeof(OrderItemDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid order item data")]
    public async Task<ActionResult<OrderItemDTO>> Create(
        [FromBody] [SwaggerRequestBody("Order item creation data")]
        OrderItemCreateDTO createOrderItemDto)
    {
        return await HandleResponse(
            async () => await _orderItemService.AddOrderItemAsync(createOrderItemDto),
            "Successfully created new order item");
    }

    
    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageOrders)]
    [SwaggerOperation(
        Summary = "Update an order item",
        Description = "Updates an existing order item with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Order item updated successfully", typeof(OrderItemDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order item not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid order item data")]
    public async Task<ActionResult<OrderItemDTO>> Update(
        [SwaggerParameter("The ID of the order item to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated order item data")]
        OrderItemDTO orderItemDto)
    {
        return await HandleResponse(
            async () => await _orderItemService.UpdateOrderItemAsync(id, orderItemDto),
            $"Successfully updated order item with ID {id}");
    }

    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageOrders)]
    [SwaggerOperation(
        Summary = "Delete an order item",
        Description = "Permanently deletes an order item from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Order item deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order item not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the order item to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _orderItemService.DeleteOrderItemAsync(id),
            $"Successfully deleted order item with ID {id}");
    }
}