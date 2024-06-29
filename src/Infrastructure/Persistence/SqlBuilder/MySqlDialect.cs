using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Web;

namespace csumathboy.Infrastructure.Persistence.SqlBuilder;

public class MySqlDialect : SqlDialectBase
{
    public override char OpenQuote
    {
        get { return '`'; }
    }

    public override char CloseQuote
    {
        get { return '`'; }
    }

    public override string GetIdentitySql(string tableName)
    {
        return "SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS ID";
    }

    public override string GetPagingSql(string sql, int page, int resultsPerPage, string partitionsby = "")
    {
        return GetSetSql(sql, GetStartValue(page, resultsPerPage), resultsPerPage);
    }

    public override string GetSetSql(string sql, int firstResult, int maxResults)
    {
        if (string.IsNullOrEmpty(sql))
            throw new ArgumentNullException(nameof(sql), $"{nameof(sql)} cannot be null.");
        if (!IsSelectSql(sql))
            throw new ArgumentException($"{nameof(sql)} must be a SELECT statement.", nameof(sql));

        string result = string.Format("{0} LIMIT {1} OFFSET {2}", sql, firstResult, maxResults);

        return result;
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

    public override string GetCountSql(string sql)
    {
        string countSQL = base.GetCountSql(sql);

        int count = Regex.Matches(sql.ToUpperInvariant(), "SELECT").Count;

        if (count > 1)
        {
            return $"{countSQL} AS {OpenQuote}Total{CloseQuote}";
        }

        return countSQL;
    }
}
