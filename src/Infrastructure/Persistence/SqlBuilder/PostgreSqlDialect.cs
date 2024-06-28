using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace csumathboy.Infrastructure.Persistence.SqlBuilder;

public class PostgreSqlDialect : SqlDialectBase
{
  public override string GetIdentitySql(string tableName)
  {
    return "SELECT LASTVAL() AS Id";
  }

  public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters, string partitionBy)
  {
    return GetSetSql(sql, GetStartValue(page, resultsPerPage), resultsPerPage, parameters);
  }

  public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
  {
    if (string.IsNullOrEmpty(sql))
      throw new ArgumentNullException(nameof(sql), $"{nameof(sql)} cannot be null.");

    if (!IsSelectSql(sql))
      throw new ArgumentException($"{nameof(sql)} must be a SELECT statement.", nameof(sql));

    string result = string.Format("{0} LIMIT {1} OFFSET {2}", sql, maxResults, firstResult);
 
    return result;
  }

  public override string GetColumnName(string prefix, string columnName, string? alias)
  {
    return base.GetColumnName(prefix, columnName, alias).ToLower();
  }

  public override string GetTableName(string schemaName, string tableName, string alias)
  {
    return base.GetTableName(schemaName, tableName, alias).ToLower();
  }

  public override string GetDatabaseFunctionString(DatabaseFunction databaseFunction, string columnName, string functionParameters = "")
  {
    return databaseFunction switch
    {
      DatabaseFunction.NullValue => $"coalesce({columnName}, {functionParameters})",
      DatabaseFunction.Truncate => $"Truncate({columnName})",
      _ => columnName,
    };
  }

  [ExcludeFromCodeCoverage]
  public override void EnableCaseInsensitive(IDbConnection connection)
  {
  }
}
