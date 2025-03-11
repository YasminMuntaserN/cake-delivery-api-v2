using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DTO;
using cakeDelivery.DTO.CakeDTOs;
using cakeDelivery.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace cakeDelivery.Business;

public class CakeService : BaseService<Cake, CakeDTO>
{
    private readonly ILogger<CakeService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<Cake> _validator;

    public CakeService(
        IMongoDatabase database,
        ILogger<CakeService> logger,
        IMapper mapper,
        IValidator<Cake> validator)
        : base(database, "cakes", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CakeDTO> AddCakeAsync(CakeCreateDto cakeCreateDto)
        => await AddAsync(cakeCreateDto, "Cake");

    public async Task<CakeDTO?> UpdateCakeAsync(string id, CakeDTO cakeDetailsDto)
        => await UpdateAsync(id, cakeDetailsDto, "Cake");

    public async Task<CakeDTO?> GetCakeByIdAsync(string id)
        => await FindBy(c => c.CakeId == id);

    public async Task<IEnumerable<CakeDTO>> GetAllCakesAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<CakeDTO> Data, long TotalCount, int TotalPages)> GetAllAsync(PaginationDto paginationDto)
    {
        Expression<Func<Cake, object>> orderBy = paginationDto.OrderBy?.ToLower() switch
        {
            "cakename" => cake => cake.CakeName,
            "createdat" => cake => cake.CreatedAt,
            "stockquantity" => cake => cake.StockQuantity,
            _ => cake => cake.CakeId
        };

        return await GetAllAsync(
            paginationDto.PageNumber,
            paginationDto.PageSize,
            orderBy,
            paginationDto.Ascending);
    }

    public async Task<bool> HardDeleteCakeAsync(string cakeId)
        => await HardDeleteAsync(cakeId);

    public async Task<bool> HardDeleteCakeByAsync(SearchCriteriaDto criteria)
    {
        var filter = criteria.Field.ToLower() switch
        {
            "cakename" => Builders<Cake>.Filter.Eq(c => c.CakeName, criteria.Value),
            "id" => Builders<Cake>.Filter.Eq("_id", ObjectId.Parse(criteria.Value)),
            _ => null
        };

        if (filter != null)
        {
            return await HardDeleteByAsync(c => filter.Inject());
        }

        return false;
    }

    public async Task<bool> ExistsCakeAsync(string cakeId)
        => await ExistsAsync(cakeId);

    public async Task<bool> ExistsCakeByAsync(SearchCriteriaDto criteria)
    {
        var filter = criteria.Field.ToLower() switch
        {
            "cakename" => Builders<Cake>.Filter.Eq(c => c.CakeName, criteria.Value),
            _ => Builders<Cake>.Filter.Eq("_id", ObjectId.Parse(criteria.Value)),
        };

        if (filter != null)
        {
            return await ExistsByAsync(c => filter.Inject());
        }

        return false;
    }

    public async Task<IEnumerable<CakeDTO>> SearchCakeAsync(SearchCriteriaDto criteria)
    {
        var filter = criteria.Field.ToLower() switch
        {
            "cakename" => Builders<Cake>.Filter.Regex(
                c => c.CakeName,
                new MongoDB.Bson.BsonRegularExpression(criteria.Value, "i")),
            _ =>  Builders<Cake>.Filter.Eq("_id", ObjectId.Parse(criteria.Value)),
        };

        if (filter != null)
        {
            return await SearchAsync(c => filter.Inject());
        }

        return Enumerable.Empty<CakeDTO>();
    }
}