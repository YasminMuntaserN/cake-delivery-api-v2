using cakeDelivery.api.Authorization;
using cakeDelivery.api.Controllers.Base;
using cakeDelivery.Business.Authorization;
using cakeDelivery.DTO;
using cakeDelivery.DTO.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserDelivery.Business;


namespace UserDelivery.api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[SwaggerTag("Manage User operations")]
public class UserController : BaseController
{
    private readonly UserService _UserService;
    private static readonly string[] SearchFields = new[] { "Email", "UserId"};

    public UserController(ILogger<UserController> logger, UserService UserService)
        : base(logger)
    {
        _UserService = UserService;
    }


    [HttpGet]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get all Users with pagination",
        Description = "Retrieves a paginated list of Users. Results are ordered according to specified criteria. for example it must be [UserId ,Email]  "
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved Users", typeof(IEnumerable<UserDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid pagination parameters")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll(
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
            var (data, totalCount, totalPages) = await _UserService.GetAllAsync(pagination);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return data;
        }, "Successfully retrieved all Users");
    }


    
    [HttpGet("{id}")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Get User by ID",
        Description = "Retrieves a specific User by its unique identifier"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved User", typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<ActionResult<UserDTO>> GetById(
        [SwaggerParameter("The ID of the User to retrieve")]
        string id)
    {
        return await HandleResponse(
            async () => await _UserService.GetUserByIdAsync(id),
            $"Successfully retrieved User with ID {id}");
    }

    
    [HttpPost("search")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Search Users",
        Description = "Search for Users using specified criteria. Available search fields: [UserId ,Email] "
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully performed search", typeof(IEnumerable<UserDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search criteria")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> Search(
        [FromBody] [SwaggerRequestBody("Search criteria")]
        SearchCriteriaDto criteria)
    {
        if (string.IsNullOrEmpty(criteria.Field) || string.IsNullOrEmpty(criteria.Value))
        {
            return BadRequest(new { message = "Search field and value are required" });
        }

        if (!SearchFields.Contains(criteria.Field))
        {
            return BadRequest(new { message = $"Search field must be one of:  [UserId ,Email] " });
        }

        return await HandleResponse(
            async () => await _UserService.SearchUserAsync(criteria),
            "Successfully performed search");
    }
    
    
    [HttpPost("exists")]
    [RequirePermission(Permissions.View)]
    [SwaggerOperation(
        Summary = "Check User existence",
        Description = "Check if a User exists based on specified criteria like:  [UserId ,Email] "
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
            async () => await _UserService.ExistsUserByAsync(criteria),
            "Successfully checked existence");
    }

    
    [HttpPost]
  //  [RequirePermission(Permissions.ManageUsers)]
    [AllowAnonymous]

    [SwaggerOperation(
        Summary = "Create a new User",
        Description = "Creates a new User with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "User created successfully", typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid User data")]
    public async Task<ActionResult<UserDTO>> Create(
        [FromBody] [SwaggerRequestBody("User creation data")]
        UserCreateDTO createUserDto)
    {
        return await HandleResponse(
            async () => await _UserService.AddUserAsync(createUserDto),
            "Successfully created new User");
    }


    [HttpPut("{id}")]
    [RequirePermission(Permissions.ManageUsers)]
    [SwaggerOperation(
        Summary = "Update a User",
        Description = "Updates an existing User with the provided details"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "User updated successfully", typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid User data")]
    public async Task<ActionResult<UserDTO>> Update(
        [SwaggerParameter("The ID of the User to update")]
        string id,
        [FromBody] [SwaggerRequestBody("Updated User data")]
        UserDTO UserDto)
    {
        return await HandleResponse(
            async () => await _UserService.UpdateUserAsync(id, UserDto),
            $"Successfully updated User with ID {id}");
    }


    
    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageUsers)]
    [SwaggerOperation(
        Summary = "Delete a User",
        Description = "Permanently deletes a User from the system"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<ActionResult<bool>> Delete(
        [SwaggerParameter("The ID of the User to delete")]
        string id)
    {
        return await HandleResponse(
            async () => await _UserService.DeleteUserAsync(id),
            $"Successfully deleted User with ID {id}");
    }
    
}