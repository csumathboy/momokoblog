﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace csumathboy.Infrastructure.Persistence.SqlBuilder;

public interface ISqlDialect
{
  char OpenQuote { get; }
  char CloseQuote { get; }
  string BatchSeperator { get; }
  bool SupportsMultipleStatements { get; }
  bool SupportsCountOfSubquery { get; }
  char ParameterPrefix { get; }
  string EmptyExpression { get; }
  string GetTableName(string schemaName, string tableName, string alias);
  string GetColumnName(string prefix, string columnName, string alias);
  string GetIdentitySql(string tableName);
  string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters, string partitionBy);
  string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters);
  bool IsQuoted(string value);
  string QuoteString(string value);
  string GetDatabaseFunctionString(DatabaseFunction databaseFunction, string columnName, string functionParameters = "");
  void EnableCaseInsensitive(IDbConnection connection);
  string GetCountSql(string sql);
}

public abstract class SqlDialectBase : ISqlDialect
{
  public virtual char OpenQuote
  {
    get { return '"'; }
  }

  public virtual char CloseQuote
  {
    get { return '"'; }
  }

  public virtual string BatchSeperator
  {
    get { return ";" + Environment.NewLine; }
  }

  public virtual bool SupportsMultipleStatements
  {
    get { return true; }
  }

  public virtual char ParameterPrefix
  {
    get
    {
      return '@';
    }
  }

  public virtual string EmptyExpression
  {
    get
    {
      return "1=1";
    }
  }

  public virtual bool SupportsCountOfSubquery => true;

  public virtual string GetTableName(string schemaName, string tableName, string alias)
  {
    if (string.IsNullOrWhiteSpace(tableName))
    {
      throw new ArgumentNullException(nameof(tableName), $"{nameof(tableName)} cannot be null or empty.");
    }

    var result = new StringBuilder();
    if (!string.IsNullOrWhiteSpace(schemaName))
    {
      result.AppendFormat(QuoteString(schemaName) + ".");
    }

    result.AppendFormat(QuoteString(tableName));

    if (!string.IsNullOrWhiteSpace(alias))
    {
      result.AppendFormat(" {0}", QuoteString(alias));
    }
    return result.ToString();
  }

  public virtual string GetColumnName(string prefix, string columnName, string? alias)
  {
    if (string.IsNullOrEmpty(columnName))
      throw new ArgumentNullException(nameof(columnName), $"{nameof(columnName)} cannot be null or empty.");

    if (string.IsNullOrWhiteSpace(columnName))
      throw new ArgumentNullException(nameof(columnName), $"{nameof(columnName)} cannot be null or empty.");

    var result = new StringBuilder();
    if (!string.IsNullOrWhiteSpace(prefix))
    {
      result.AppendFormat((IsQuoted(prefix) ? prefix : QuoteString(prefix)) + ".");
    }

    result.AppendFormat(QuoteString(columnName));

    if (!string.IsNullOrWhiteSpace(alias))
    {
      result.AppendFormat(" AS {0}", QuoteString(alias));
    }

    return result.ToString();
  }

  protected virtual int GetStartValue(int page, int resultsPerPage)
  {
    return (((page == 0 ? 1 : page) - 1) * resultsPerPage);
  }

  public abstract string GetIdentitySql(string tableName);
  public abstract string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters, string partitionBy);
  public abstract string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters);

  public virtual bool IsQuoted(string value)
  {
    if (value.Trim()[0] == OpenQuote)
    {
      return value.Trim().Last() == CloseQuote;
    }

    return false;
  }

  public virtual string QuoteString(string value)
  {
    if (IsQuoted(value) || value == "*")
    {
      return value;
    }
    return string.Format("{0}{1}{2}", OpenQuote, value.Trim(), CloseQuote);
  }

  public virtual string UnQuoteString(string value)
  {
    return IsQuoted(value) ? value.Substring(1, value.Length - 2) : value;
  }

  public abstract string GetDatabaseFunctionString(DatabaseFunction databaseFunction, string columnName, string functionParameters = "");

  public abstract void EnableCaseInsensitive(IDbConnection connection);

  public virtual string GetCountSql(string sql)
  {
    if (string.IsNullOrEmpty(sql))
      throw new ArgumentNullException(nameof(sql), $"{nameof(sql)} cannot be null or empty.");

    if (string.IsNullOrWhiteSpace(sql))
      throw new ArgumentNullException(nameof(sql), $"{nameof(sql)} cannot be null or empty.");

    return $"SELECT COUNT(*) AS {OpenQuote}Total{CloseQuote} FROM {sql}";
  }

  protected virtual bool IsSelectSql(string sql)
  {
    return sql.Trim().StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase);
  }
}
