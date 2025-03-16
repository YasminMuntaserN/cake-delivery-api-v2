using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DTO;
using cakeDelivery.DTO.DeliveryDTO;
using cakeDelivery.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace cakeDelivery.Business;

public class DeliveryService : BaseService<Delivery, DeliveryDTO>
{
    private readonly ILogger<DeliveryService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<Delivery> _validator;

    public DeliveryService(
        IMongoDatabase database,
        ILogger<DeliveryService> logger,
        IMapper mapper,
        IValidator<Delivery> validator)
        : base(database, "deliveries", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<DeliveryDTO> AddDeliveryAsync(DeliveryCreateDTO createDeliveryDto)
        => await AddAsync(createDeliveryDto, "Delivery");

    public async Task<DeliveryDTO?> UpdateDeliveryAsync(string id, DeliveryDTO deliveryDTO)
        => await UpdateAsync(id, deliveryDTO, "Delivery");

    public async Task<DeliveryDTO?> GetDeliveryByIdAsync(string id)
        => await FindBy(d => d.DeliveryId == id);

    public async Task<IEnumerable<DeliveryDTO>> GetAllDeliveriesAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<DeliveryDTO> Data, long TotalCount, int TotalPages)> GetAllAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<Delivery, object>> orderBy = paginationDto.OrderBy switch
        {
            "DeliveryDate" => delivery => delivery.DeliveryDate,
            "DeliveryStatus" => delivery => delivery.DeliveryStatus,
            "DeliveryCity" => delivery => delivery.DeliveryCity,
            _ => delivery => delivery.DeliveryId
        };

        return await GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize, orderBy, paginationDto.Ascending);
    }

    public async Task<bool> DeleteDeliveryAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeleteDeliveryByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Delivery, bool>> predicate = criteria.Field.ToLower() switch
        {
            "orderId" => d => d.OrderId == criteria.Value,
            "id" => d => d.DeliveryId == criteria.Value,
            _ => null
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsDeliveryAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsDeliveryByAsync(SearchCriteriaDto criteria)
        => criteria.Field.ToLower() switch
        {
            "orderId" => await ExistsByAsync(d => d.OrderId == criteria.Value),
            "id" => await ExistsByAsync(d => d.DeliveryId == criteria.Value),
            _ => false
        };

    public async Task<IEnumerable<DeliveryDTO>> SearchDeliveryAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Delivery, bool>> predicate = criteria.Field switch
        {
            "orderId" => d => d.OrderId == criteria.Value,
            "deliveryCity" => d => d.DeliveryCity.Contains(criteria.Value),
            "deliveryStatus" => d => d.DeliveryStatus == criteria.Value,
            "id" => d => d.DeliveryId == criteria.Value,
            _ => null
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<DeliveryDTO>();
    }
}
