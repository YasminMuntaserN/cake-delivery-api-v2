using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DTO;
using cakeDelivery.DTO.OrderItemDTOs;
using cakeDelivery.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace cakeDelivery.Business;

public class OrderItemService : BaseService<OrderItem, OrderItemDTO>
{
    private readonly ILogger<OrderItemService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<OrderItem> _validator;

    public OrderItemService(
        IMongoDatabase database,
        ILogger<OrderItemService> logger,
        IMapper mapper,
        IValidator<OrderItem> validator)
        : base(database, "orderItems", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<OrderItemDTO> AddOrderItemAsync(OrderItemCreateDTO createOrderItemDto)
        => await AddAsync(createOrderItemDto, "OrderItem");

    public async Task<OrderItemDTO?> UpdateOrderItemAsync(string id, OrderItemDTO orderItemDTO)
        => await UpdateAsync(id, orderItemDTO, "OrderItem");

    public async Task<OrderItemDTO?> GetOrderItemByIdAsync(string id)
        => await FindBy(o => o.OrderItemId == id);

    public async Task<IEnumerable<OrderItemDTO>> GetAllOrderItemsAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<OrderItemDTO> Data, long TotalCount, int TotalPages)> GetAllAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<OrderItem, object>> orderBy = paginationDto.OrderBy switch
        {
            "Quantity" => orderItem => orderItem.Quantity,
            "SizeId" => orderItem => orderItem.SizeId,
            "CakeId" => orderItem => orderItem.CakeId,
            "OrderId" => orderItem => orderItem.OrderId,
            _ => orderItem => orderItem.OrderItemId
        };

        return await GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize, orderBy, paginationDto.Ascending);
    }

    public async Task<bool> DeleteOrderItemAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeleteOrderItemByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<OrderItem, bool>> predicate = criteria.Field switch
        {
            "CakeId" => o => o.CakeId == criteria.Value,
            "OrderId" => o => o.OrderId == criteria.Value,
            "Id" => o => o.OrderItemId == criteria.Value,
            _ => null
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsOrderItemAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsOrderItemByAsync(SearchCriteriaDto criteria)
        => criteria.Field switch
        {
            "CakeId" => await ExistsByAsync(o => o.CakeId == criteria.Value),
            "OrderId" => await ExistsByAsync(o => o.OrderId == criteria.Value),
            "Id" => await ExistsByAsync(o => o.OrderItemId == criteria.Value),
            _ => false
        };

    public async Task<IEnumerable<OrderItemDTO>> SearchOrderItemAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<OrderItem, bool>> predicate = criteria.Field switch
        {
            "CakeId" => o => o.CakeId == criteria.Value,
            "OrderId" => o => o.OrderId == criteria.Value,
            "Id" => o => o.OrderItemId == criteria.Value,
            _ => null
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<OrderItemDTO>();
    }
}
