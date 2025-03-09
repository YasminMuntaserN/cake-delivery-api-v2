using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.PaymentDTOs;
using cakeDelivery.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cakeDelivery.api.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Manage payment operations")]
public class PaymentController : BaseController
{
    private readonly PaymentService _paymentService;
    private static readonly string[] SearchFields = new[] { "OrderId", "PaymentMethod", "PaymentStatus", "PaymentId", "PaymentDate" };

    public PaymentController(ILogger<PaymentController> logger, PaymentService paymentService)
        : base(logger)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all payments with pagination",
        Description = "Retrieves a paginated list of payments. Sortable fields: PaymentDate, AmountPaid, PaymentStatus, PaymentId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved payments", typeof(IEnumerable<PaymentDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAll(
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
            var (data, totalCount, totalPages) = await _paymentService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all payments");
    }

    
    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get payment by ID",
        Description = "Retrieves a specific payment by its unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved payment", typeof(PaymentDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Payment not found")]
    public async Task<ActionResult<PaymentDTO>> GetById(
        [SwaggerParameter("The ID of the payment to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _paymentService.GetPaymentByIdAsync(id),
            $"Successfully retrieved payment with ID {id}");
    }

    
    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search payments",
        Description = "Search for payments using specified criteria. Available search fields: OrderId, PaymentMethod, PaymentStatus, PaymentId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<PaymentDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<PaymentDTO>>> Search(
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
            async () => await _paymentService.SearchPaymentAsync(criteria),
            "Successfully performed search");
    }

    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check payment existence",
        Description = "Check if a payment exists based on specified criteria"
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
            async () => await _paymentService.ExistsPaymentByAsync(criteria),
            "Successfully checked existence");
    }

    
    [HttpPost]
    [RequirePermission(Permissions.ManagePayments)]
    [SwaggerOperation(
        Summary = "Create a new payment",
        Description = "Creates a new payment with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Payment created successfully", typeof(PaymentDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payment data")]
    public async Task<ActionResult<PaymentDTO>> Create(
        [FromBody] [SwaggerRequestBody("Payment creation data")]
        PaymentCreateDTO createPaymentDto)
    {
        return await HandleResponse(
            async () => await _paymentService.AddPaymentAsync(createPaymentDto),
            "Successfully created new payment");
    }

    
    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManagePayments)]
    [SwaggerOperation(
        Summary = "Update a payment",
        Description = "Updates an existing payment with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Payment updated successfully", typeof(PaymentDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Payment not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payment data")]
    public async Task<ActionResult<PaymentDTO>> Update(
        [SwaggerParameter("The ID of the payment to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated payment data")]
        PaymentDTO paymentDto)
    {
        return await HandleResponse(
            async () => await _paymentService.UpdatePaymentAsync(id, paymentDto),
            $"Successfully updated payment with ID {id}");
    }

    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManagePayments)]
    [SwaggerOperation(
        Summary = "Delete a payment",
        Description = "Permanently deletes a payment from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Payment deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Payment not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the payment to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _paymentService.DeletePaymentAsync(id),
            $"Successfully deleted payment with ID {id}");
    }
}