using System.Linq.Expressions;

namespace cakeDelivery.Business.Interfaces;

public interface IBaseService<TEntity, TDto> where TEntity : class where TDto : class
{
    Task<TDto?> FindBy(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<(IEnumerable<TDto> Data, int TotalCount, int TotalPages)> GetAllAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, object>> orderBy,
        bool ascending = true);
    Task<TDto> AddAsync<TCreateDto>(TCreateDto createDto, string entityName);
    Task<TDto?> UpdateAsync(int id, TDto dto, string entityName );
    Task<bool> SoftDeleteAsync(int id , string propertyName);
    Task<bool> HardDeleteAsync(int id, string propertyName);
    Task<bool> HardDeleteByAsync(string propertyName, Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TDto>> SearchAsync(Expression<Func<TEntity, bool>> filter);
}