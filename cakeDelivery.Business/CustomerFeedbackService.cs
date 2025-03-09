using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DataAccess.Data;
using cakeDelivery.DTO;
using cakeDelivery.DTO.FeedbackDTOs;
using cakeDelivery.Entities;
using cakeDelivery.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace cakeDelivery.Business;

public class CustomerFeedbackService : BaseService<CustomerFeedback, FeedbackDto>
{
    private readonly ILogger<CustomerFeedbackService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<CustomerFeedback> _validator;

    public CustomerFeedbackService(
        IMongoDatabase database,
        ILogger<CustomerFeedbackService> logger,
        IMapper mapper,
        IValidator<CustomerFeedback> validator)
        : base(database, "customerFeedbacks", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<FeedbackDto> AddFeedbackAsync(FeedbackCreateDto createFeedbackDto)
        => await AddAsync(createFeedbackDto, "CustomerFeedback");

    public async Task<FeedbackDto?> UpdateFeedbackAsync(string id, FeedbackDto feedbackDetailsDto)
        => await UpdateAsync(id, feedbackDetailsDto, "CustomerFeedback");

    public async Task<FeedbackDto?> GetFeedbackByIdAsync(string id)
        => await FindBy(f => f.FeedbackId == id);

    public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<FeedbackDto> Data, long TotalCount, int TotalPages)> GetAllAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<CustomerFeedback, object>> orderBy = paginationDto.OrderBy switch
        {
            "FeedbackDate" => feedback => feedback.FeedbackDate,
            "Rating" => feedback => feedback.Rating,
            _ => feedback => feedback.FeedbackId
        };

        return await GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize, orderBy, paginationDto.Ascending);
    }

    public async Task<bool> DeleteFeedbackAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeleteFeedbackByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<CustomerFeedback, bool>> predicate = criteria.Field.ToLower() switch
        {
            "customerId" => f => f.CustomerId == criteria.Value,
            "id" => f => f.FeedbackId == criteria.Value,
            _ => null
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsFeedbackAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsFeedbackByAsync(SearchCriteriaDto criteria)
        => criteria.Field.ToLower() switch
        {
            "customerId" => await ExistsByAsync(f => f.CustomerId == criteria.Value),
            "id" => await ExistsByAsync(f => f.FeedbackId == criteria.Value),
            _ => false
        };

    public async Task<IEnumerable<FeedbackDto>> SearchFeedbackAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<CustomerFeedback, bool>> predicate = criteria.Field.ToLower() switch
        {
            "customerId" => f => f.CustomerId == criteria.Value,
            "rating" when int.TryParse(criteria.Value, out int rating) => f => f.Rating == rating,
            "feedback" => f => f.Feedback.Contains(criteria.Value),
            "id" => f => f.FeedbackId == criteria.Value,
            _ => null
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<FeedbackDto>();
    }
}
