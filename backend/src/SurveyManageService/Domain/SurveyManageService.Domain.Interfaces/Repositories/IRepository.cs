using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
