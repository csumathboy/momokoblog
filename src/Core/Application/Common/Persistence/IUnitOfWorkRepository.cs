using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Application.Common.Persistence;
public interface IUnitOfWorkRepository<T> : IRepositoryBase<T>
    where T : class
{
    Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    void BeginTransaction();
    void Commit();
    void Rollback();

    Task<int> SaveChangesAsync();
    Task SaveAndCommitAsync();

}