using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace csumathboy.Infrastructure.Persistence.SqlBuilder;

public class SqlServerDialect : SqlDialectBase
{
    public override char OpenQuote
    {
        get { return '['; }
    }

    public override char CloseQuote
    {
        get { return ']'; }
    }

    public override string GetIdentitySql(string tableName)
    {
        return string.Format("SELECT CAST(SCOPE_IDENTITY()  AS BIGINT) AS [Id]");
    }

    public override string GetPagingSql(string sql, int page, int resultsPerPage, string partitionBy)
    {
        return GetSetSql(sql, GetStartValue(page, resultsPerPage), resultsPerPage);
    }

    public override string GetSetSql(string sql, int firstResult, int maxResults)
    {
        if (string.IsNullOrEmpty(sql))
            throw new ArgumentNullException(nameof(sql), $"{nameof(sql)} cannot be null.");

        if (!IsSelectSql(sql))
            throw new ArgumentException($"{nameof(sql)} must be a SELECT statement.", nameof(sql));

        if (string.IsNullOrEmpty(GetOrderByClause(sql)))
            sql = $"{sql} ORDER BY CURRENT_TIMESTAMP";

        string result = $"{sql} OFFSET (" + firstResult + ") ROWS FETCH NEXT " + maxResults + " ROWS ONLY";
 
        return result;
    }

    protected static string GetOrderByClause(string sql)
    {
        int orderByIndex = sql.LastIndexOf(" ORDER BY ", StringComparison.InvariantCultureIgnoreCase);
        if (orderByIndex == -1)
        {
            return string.Empty;
        }

        string result = sql.Substring(orderByIndex).Trim();

        int whereIndex = result.IndexOf(" WHERE ", StringComparison.InvariantCultureIgnoreCase);
        if (whereIndex == -1)
        {
            return result;
        }

        return result.Substring(0, whereIndex).Trim();
    }

    public override string GetDatabaseFunctionString(DatabaseFunction databaseFunction, string columnName, string functionParameters = "")
    {
        return databaseFunction switch
        {
            DatabaseFunction.NullValue => $"IsNull({columnName}, {functionParameters})",
            DatabaseFunction.Truncate => $"Truncate({columnName})",
            _ => columnName,
        };
    }

    [ExcludeFromCodeCoverage]
    public override void EnableCaseInsensitive(IDbConnection connection)
    {
    }
}
