using System.Data;
using Dapper;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using csumathboy.Application.Common.Persistence;
using csumathboy.Domain.Common.Contracts;
using csumathboy.Infrastructure.Persistence.Context;

namespace csumathboy.Infrastructure.Persistence.Repository;

/// <summary>
/// For high performance SearchData using Dapper instead of EF.
/// </summary>
public class DapperRepository : IDapperRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DapperRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);
        return (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
             .AsList();
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);

        return await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);

        return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}