using csumathboy.Infrastructure.Common;

namespace csumathboy.Infrastructure.Persistence.SqlBuilder;
public class SqlBuilderFactory
{
    private static DatabaseSettings? _dbSettings;
    public SqlBuilderFactory(DatabaseSettings dbSettings)
    {
        _dbSettings = dbSettings;
    }

    /// <summary>
    /// Get Paging Sql.
    /// </summary>
    /// <param name="sourceSql"></param>
    /// <param name="page"></param>
    /// <param name="pagesize"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetDataFromSqlBuilder(string sourceSql, int page, int pagesize, IDictionary<string, object> parameters)
    {

        string sql = _dbSettings.DBProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql => new PostgreSqlDialect().GetPagingSql(sourceSql, page, pagesize, parameters, string.Empty),
            DbProviderKeys.SqlServer => new SqlServerDialect().GetPagingSql(sourceSql, page, pagesize, parameters, string.Empty),
            DbProviderKeys.MySql => new MySqlDialect().GetPagingSql(sourceSql, page, pagesize, parameters, string.Empty),
            DbProviderKeys.Oracle => new OracleDialect().GetPagingSql(sourceSql, page, pagesize, parameters, string.Empty),
            DbProviderKeys.SqLite => new SqliteDialect().GetPagingSql(sourceSql, page, pagesize, parameters, string.Empty),
            _ => throw new InvalidOperationException("DB Provider is not supported."),
        };
        return sql;
    }

    /// <summary>
    /// Get Count Sql.
    /// </summary>
    /// <param name="sourceSql"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetCountFromSqlBuilder(string sourceSql)
    {

        string sql = _dbSettings.DBProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql => new PostgreSqlDialect().GetCountSql(sourceSql),
            DbProviderKeys.SqlServer => new SqlServerDialect().GetCountSql(sourceSql),
            DbProviderKeys.MySql => new MySqlDialect().GetCountSql(sourceSql),
            DbProviderKeys.Oracle => new OracleDialect().GetCountSql(sourceSql),
            DbProviderKeys.SqLite => new SqliteDialect().GetCountSql(sourceSql),
            _ => throw new InvalidOperationException("DB Provider is not supported."),
        };
        return sql;
    }
}
