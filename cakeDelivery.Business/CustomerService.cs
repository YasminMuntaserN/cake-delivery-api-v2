using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DataAccess.Data;
using cakeDelivery.DTO;
using cakeDelivery.DTO.CustomerDTOs;
using cakeDelivery.Entities;
using cakeDelivery.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace cakeDelivery.Business;

public class CustomerService : BaseService<Customer, CustomerDTO>
{
    private readonly ILogger<CustomerService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<Customer> _validator;

    public CustomerService(
        IMongoDatabase database,
        ILogger<CustomerService> logger,
        IMapper mapper,
        IValidator<Customer> validator)
        : base(database, "customers", logger, mapper, validator) 
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CustomerDTO> AddCustomerAsync(CustomerCreateDTO createCustomerDto)
        => await AddAsync(createCustomerDto, "Customer");

    public async Task<CustomerDTO?> UpdateCustomerAsync(string id, CustomerDTO customerDetailsDto)
        => await UpdateAsync(id, customerDetailsDto, "Customer");

    public async Task<CustomerDTO?> GetCustomerByIdAsync(string id)
        => await FindBy(c => c.CustomerId== id);

    public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<CustomerDTO> Data, long TotalCount, int TotalPages)> GetAllCustomersAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<Customer, object>> orderBy = paginationDto.OrderBy switch
        {
            "FirstName" => customer => customer.FirstName,
            "LastName" => customer => customer.LastName,
            "Email" => customer => customer.Email,
            _ => customer => customer.CustomerId
        };

        return await GetAllAsync(
            paginationDto.PageNumber, 
            paginationDto.PageSize, 
            orderBy, 
            paginationDto.Ascending);
    }

    public async Task<bool> DeleteCustomerAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeleteCustomerByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Customer, bool>> predicate = criteria.Field.ToLower() switch
        {
            "email" => c => c.Email == criteria.Value,
            "id" => c => c.CustomerId == criteria.Value,
            _ => null
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsCustomerAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsCustomerByAsync(SearchCriteriaDto criteria)
        => criteria.Field.ToLower() switch
        {
            "email" => await ExistsByAsync(c => c.Email == criteria.Value),
            "id" => await ExistsByAsync(c => c.CustomerId == criteria.Value),
            _ => false
        };

    public async Task<IEnumerable<CustomerDTO>> SearchCustomerAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<Customer, bool>> predicate = criteria.Field.ToLower() switch
        {
            "email" => c => c.Email.Contains(criteria.Value),
            "firstname" => c => c.FirstName.Contains(criteria.Value),
            "lastname" => c => c.LastName.Contains(criteria.Value),
            "id" => c => c.CustomerId == criteria.Value,
            _ => null
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<CustomerDTO>();
    }
}
