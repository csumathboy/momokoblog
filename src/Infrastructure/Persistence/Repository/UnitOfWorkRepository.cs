using Ardalis.Specification.EntityFrameworkCore;
using csumathboy.Application.Common.Persistence;
using csumathboy.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Infrastructure.Persistence.Repository;
 
public class UnitOfWorkRepository<T> : RepositoryBase<T>, IUnitOfWorkRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _transaction;
    public UnitOfWorkRepository(ApplicationDbContext dbContext)
      : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);
        if (saveChanges)
        {
            await SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public async Task<T> UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Update(entity);
        if (saveChanges)
        {
            await SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public async Task<T> DeleteAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        if (saveChanges)
        {
            await SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public void BeginTransaction()
    {
        if (_transaction != null)
        {
            return;
        }

        _transaction = _dbContext.Database.BeginTransaction();
    }

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public void Commit()
    {
        if (_transaction == null)
        {
            return;
        }

        _transaction.Commit();
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task SaveAndCommitAsync()
    {
        await SaveChangesAsync();
        Commit();
    }

    public void Rollback()
    {
        if (_transaction == null)
        {
            return;
        }

        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        if (_transaction == null)
        {
            return;
        }

        _transaction.Dispose();
        _transaction = null;
    }
}
