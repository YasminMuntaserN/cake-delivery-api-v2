using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DTO;
using cakeDelivery.DTO.OrderDTOs;
using cakeDelivery.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace cakeDelivery.Business;

public class OrderService : BaseService<Order, OrderDTO>
{
    private readonly ILogger<OrderService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<Order> _validator;

    public OrderService(
        IMongoDatabase database,
        ILogger<OrderService> logger,
        IMapper mapper,
        IValidator<Order> validator)
        : base(database, "orders", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<OrderDTO> AddOrderAsync(OrderCreateDTO createOrderDto)
        => await AddAsync(createOrderDto, "Order");

    public async Task<OrderDTO?> UpdateOrderAsync(string id, OrderDTO orderDTO)
        => await UpdateAsync(id, orderDTO, "Order");

    public async Task<OrderDTO?> GetOrderByIdAsync(string id)
        => await FindBy(o => o.OrderId == id);

    public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<OrderDTO> Data, long TotalCount, int TotalPages)> GetAllAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<Order, object>> orderBy = paginationDto.OrderBy switch
        {
            "OrderDate" => order => order.OrderDate,
            "TotalAmount" => order => order.TotalAmount,
            "PaymentStatus" => order => order.PaymentStatus,
            "DeliveryStatus" => order => order.DeliveryStatus,
            _ => order => order.OrderId
        };

        return await GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize, orderBy, paginationDto.Ascending);
    }

    public async Task<bool> DeleteOrderAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeleteOrderByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Order, bool>> predicate = criteria.Field.ToLower() switch
        {
            "customerId" => o => o.CustomerId == criteria.Value,
            "id" => o => o.OrderId == criteria.Value,
            _ => null
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsOrderAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsOrderByAsync(SearchCriteriaDto criteria)
        => criteria.Field.ToLower() switch
        {
            "customerId" => await ExistsByAsync(o => o.CustomerId == criteria.Value),
            "id" => await ExistsByAsync(o => o.OrderId == criteria.Value),
            _ => false
        };

    public async Task<IEnumerable<OrderDTO>> SearchOrderAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Order, bool>> predicate = criteria.Field.ToLower() switch
        {
            "customerId" => o => o.CustomerId == criteria.Value,
            "paymentStatus" => o => o.PaymentStatus == criteria.Value,
            "deliveryStatus" => o => o.DeliveryStatus == criteria.Value,
            "id" => o => o.OrderId == criteria.Value,
            _ => null
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<OrderDTO>();
    }
}
