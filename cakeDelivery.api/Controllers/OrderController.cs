using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cakeDelivery.api.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
[SwaggerTag("Manage orders and related operations")]
public class OrderController : BaseController
{
    private readonly OrderService _orderService;
    private static readonly string[] SearchFields = new[] { "OrderId", "CustomerId", "OrderDate", "TotalAmount", "PaymentStatus", "DeliveryStatus" };

    public OrderController(ILogger<OrderController> logger, OrderService orderService)
        : base(logger)
    {
        _orderService = orderService;
    }


    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all orders with pagination",
        Description = "Retrieves a paginated list of orders. Sortable fields: OrderDate, TotalAmount, PaymentStatus, DeliveryStatus, OrderId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved orders", typeof(IEnumerable<OrderDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll(
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
            var (data, totalCount, totalPages) = await _orderService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all orders");
    }
    
    
    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get order by ID",
        Description = "Retrieves a specific order by its unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved order", typeof(OrderDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found")]
    public async Task<ActionResult<OrderDTO>> GetById(
        [SwaggerParameter("The ID of the order to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _orderService.GetOrderByIdAsync(id),
            $"Successfully retrieved order with ID {id}");
    }
    

    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search orders",
        Description = "Search for orders using specified criteria. Available search fields: OrderId, CustomerId, PaymentStatus, DeliveryStatus"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<OrderDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> Search(
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
            async () => await _orderService.SearchOrderAsync(criteria),
            "Successfully performed search");
    }
    
    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check order existence",
        Description = "Check if an order exists based on specified criteria"
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
            async () => await _orderService.ExistsOrderByAsync(criteria),
            "Successfully checked existence");
    }

    
    [HttpPost]
    [RequirePermission(Permissions.ManageOrders)]
    [SwaggerOperation(
        Summary = "Create a new order",
        Description = "Creates a new order with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Order created successfully", typeof(OrderDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid order data")]
    public async Task<ActionResult<OrderDTO>> Create(
        [FromBody] [SwaggerRequestBody("Order creation data")]
        OrderCreateDTO createOrderDto)
    {
        return await HandleResponse(
            async () => await _orderService.AddOrderAsync(createOrderDto),
            "Successfully created new order");
    }

    
    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageOrders)]
    [SwaggerOperation(
        Summary = "Update an order",
        Description = "Updates an existing order with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Order updated successfully", typeof(OrderDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid order data")]
    public async Task<ActionResult<OrderDTO>> Update(
        [SwaggerParameter("The ID of the order to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated order data")]
        OrderDTO orderDto)
    {
        return await HandleResponse(
            async () => await _orderService.UpdateOrderAsync(id, orderDto),
            $"Successfully updated order with ID {id}");
    }
    
    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageOrders)]
    [SwaggerOperation(
        Summary = "Delete an order",
        Description = "Permanently deletes an order from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Order deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the order to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _orderService.DeleteOrderAsync(id),
            $"Successfully deleted order with ID {id}");
    }
}