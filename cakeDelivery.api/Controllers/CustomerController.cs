using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.CustomerDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cakeDelivery.api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[SwaggerTag("Manage customer operations")]
public class CustomerController : BaseController
{
    private readonly CustomerService _customerService;
    private static readonly string[] SearchFields = new[] { "FirstName", "LastName", "Email"};

    public CustomerController(ILogger<CustomerController> logger, CustomerService customerService)
        : base(logger)
    {
        _customerService = customerService;
    }

    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all customers with pagination",
        Description = "Retrieves a paginated list of customers. Results are ordered according to specified criteria."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved customers", typeof(IEnumerable<CustomerDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAll(
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
            var (data, totalCount, totalPages) = await _customerService.GetAllCustomersAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all customers");
    }

    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get customer by ID",
        Description = "Retrieves a specific customer by their unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved customer", typeof(CustomerDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
    public async Task<ActionResult<CustomerDTO>> GetById(
        [SwaggerParameter("The ID of the customer to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _customerService.GetCustomerByIdAsync(id),
            $"Successfully retrieved customer with ID {id}");
    }

    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search customers",
        Description = "Search for customers using specified criteria. Available search fields: FirstName, LastName, Email"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<CustomerDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<CustomerDTO>>> Search(
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
            async () => await _customerService.SearchCustomerAsync(criteria),
            "Successfully performed search");
    }

    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check customer existence",
        Description = "Check if a customer exists based on specified criteria like: FirstName, LastName, Email"
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
            async () => await _customerService.ExistsCustomerByAsync(criteria),
            "Successfully checked existence");
    }

    
    [HttpPost]
    [RequirePermission(Permissions.ManageCustomers)]
    [SwaggerOperation(
        Summary = "Create a new customer",
        Description = "Creates a new customer with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Customer created successfully", typeof(CustomerDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid customer data")]
    public async Task<ActionResult<CustomerDTO>> Create(
        [FromBody] [SwaggerRequestBody("Customer creation data")]
        CustomerCreateDTO createCustomerDto)
    {
        return await HandleResponse(
            async () => await _customerService.AddCustomerAsync(createCustomerDto),
            "Successfully created new customer");
    }

    
    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageCustomers)]
    [SwaggerOperation(
        Summary = "Update a customer",
        Description = "Updates an existing customer with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Customer updated successfully", typeof(CustomerDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid customer data")]
    public async Task<ActionResult<CustomerDTO>> Update(
        [SwaggerParameter("The ID of the customer to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated customer data")]
        CustomerDTO customerDto)
    {
        return await HandleResponse(
            async () => await _customerService.UpdateCustomerAsync(id, customerDto),
            $"Successfully updated customer with ID {id}");
    }

    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageCustomers)]
    [SwaggerOperation(
        Summary = "Delete a customer",
        Description = "Permanently deletes a customer from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Customer deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the customer to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _customerService.DeleteCustomerAsync(id),
            $"Successfully deleted customer with ID {id}");
    }
}