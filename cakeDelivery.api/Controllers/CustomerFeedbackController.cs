using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.FeedbackDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cakeDelivery.api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[SwaggerTag("Manage customer feedback operations")]
public class CustomerFeedbackController : BaseController
{
    private readonly CustomerFeedbackService _feedbackService;
    private static readonly string[] SearchFields = new[] { "CustomerId", "Rating", "Feedback", "FeedbackId", "FeedbackDate" };

    public CustomerFeedbackController(ILogger<CustomerFeedbackController> logger, CustomerFeedbackService feedbackService)
        : base(logger)
    {
        _feedbackService = feedbackService;
    }

    
    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all feedback with pagination",
        Description = "Retrieves a paginated list of customer feedbacks. Sortable fields: FeedbackDate, Rating, FeedbackId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved feedbacks", typeof(IEnumerable<FeedbackDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetAll(
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
            var (data, totalCount, totalPages) = await _feedbackService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all feedbacks");
    }

    
    
    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get feedback by ID",
        Description = "Retrieves a specific feedback by its unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved feedback", typeof(FeedbackDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Feedback not found")]
    public async Task<ActionResult<FeedbackDto>> GetById(
        [SwaggerParameter("The ID of the feedback to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _feedbackService.GetFeedbackByIdAsync(id),
            $"Successfully retrieved feedback with ID {id}");
    }

    
    
    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search feedback",
        Description = "Search for feedbacks using specified criteria. Available search fields: CustomerId, Rating, Feedback, FeedbackId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<FeedbackDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<FeedbackDto>>> Search(
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
            async () => await _feedbackService.SearchFeedbackAsync(criteria),
            "Successfully performed search");
    }

    
    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check feedback existence",
        Description = "Check if a feedback exists based on specified criteria"
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
            async () => await _feedbackService.ExistsFeedbackByAsync(criteria),
            "Successfully checked existence");
    }

    
    
    [HttpPost]
    [RequirePermission(Permissions.ManageCustomers)]
    [SwaggerOperation(
        Summary = "Create a new feedback",
        Description = "Creates a new customer feedback with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Feedback created successfully", typeof(FeedbackDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid feedback data")]
    public async Task<ActionResult<FeedbackDto>> Create(
        [FromBody] [SwaggerRequestBody("Feedback creation data")]
        FeedbackCreateDto createFeedbackDto)
    {
        return await HandleResponse(
            async () => await _feedbackService.AddFeedbackAsync(createFeedbackDto),
            "Successfully created new feedback");
    }
    
    
    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageCustomers)]
    [SwaggerOperation(
        Summary = "Update a feedback",
        Description = "Updates an existing feedback with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Feedback updated successfully", typeof(FeedbackDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Feedback not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid feedback data")]
    public async Task<ActionResult<FeedbackDto>> Update(
        [SwaggerParameter("The ID of the feedback to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated feedback data")]
        FeedbackDto feedbackDto)
    {
        return await HandleResponse(
            async () => await _feedbackService.UpdateFeedbackAsync(id, feedbackDto),
            $"Successfully updated feedback with ID {id}");
    }

    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageCustomers)]
    [SwaggerOperation(
        Summary = "Delete a feedback",
        Description = "Permanently deletes a customer feedback from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Feedback deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Feedback not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the feedback to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _feedbackService.DeleteFeedbackAsync(id),
            $"Successfully deleted feedback with ID {id}");
    }
}
