using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Infrastructure.Persistence.SqlBuilder;
public interface ISqlBuilderService
{
    public string GetDataFromSqlBuilder(string sourceSql, int page, int pagesize);

    public string GetCountFromSqlBuilder(string sourceSql);
}
