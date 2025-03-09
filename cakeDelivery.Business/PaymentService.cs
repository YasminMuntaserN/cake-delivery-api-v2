using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DTO;
using cakeDelivery.DTO.PaymentDTOs;
using cakeDelivery.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace cakeDelivery.Business;

public class PaymentService : BaseService<Payment, PaymentDTO>
{
    private readonly ILogger<PaymentService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<Payment> _validator;

    public PaymentService(
        IMongoDatabase database,
        ILogger<PaymentService> logger,
        IMapper mapper,
        IValidator<Payment> validator)
        : base(database, "payments", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<PaymentDTO> AddPaymentAsync(PaymentCreateDTO createPaymentDto)
        => await AddAsync(createPaymentDto, "Payment");

    public async Task<PaymentDTO?> UpdatePaymentAsync(string id, PaymentDTO paymentDTO)
        => await UpdateAsync(id, paymentDTO, "Payment");

    public async Task<PaymentDTO?> GetPaymentByIdAsync(string id)
        => await FindBy(p => p.PaymentId == id);

    public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<PaymentDTO> Data, long TotalCount, int TotalPages)> GetAllAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<Payment, object>> orderBy = paginationDto.OrderBy switch
        {
            "PaymentDate" => payment => payment.PaymentDate,
            "AmountPaid" => payment => payment.AmountPaid,
            "PaymentStatus" => payment => payment.PaymentStatus,
            _ => payment => payment.PaymentId
        };

        return await GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize, orderBy, paginationDto.Ascending);
    }

    public async Task<bool> DeletePaymentAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeletePaymentByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Payment, bool>> predicate = criteria.Field.ToLower() switch
        {
            "orderId" => p => p.OrderId == criteria.Value,
            "id" => p => p.PaymentId == criteria.Value,
            _ => null
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsPaymentAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsPaymentByAsync(SearchCriteriaDto criteria)
        => criteria.Field.ToLower() switch
        {
            "orderId" => await ExistsByAsync(p => p.OrderId == criteria.Value),
            "id" => await ExistsByAsync(p => p.PaymentId == criteria.Value),
            _ => false
        };

    public async Task<IEnumerable<PaymentDTO>> SearchPaymentAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Payment, bool>> predicate = criteria.Field.ToLower() switch
        {
            "orderId" => p => p.OrderId == criteria.Value,
            "paymentMethod" => p => p.PaymentMethod == criteria.Value,
            "paymentStatus" => p => p.PaymentStatus == criteria.Value,
            "id" => p => p.PaymentId == criteria.Value,
            _ => null
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<PaymentDTO>();
    }
}
