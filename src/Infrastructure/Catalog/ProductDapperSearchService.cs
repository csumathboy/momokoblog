using csumathboy.Application.Catalog.Interfaces;
using csumathboy.Application.Catalog.Products;
using csumathboy.Application.Common.Models;
using csumathboy.Application.Common.Persistence;
using csumathboy.Infrastructure.Persistence.SqlBuilder;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Infrastructure.Catalog;
public class ProductDapperSearchService : IProductDapperSearchService
{
    private readonly IDapperRepository _repository;
    public ProductDapperSearchService(IDapperRepository repository) => _repository = repository;

    public async Task<ProductDto?> GetProductById(Guid Id, CancellationToken cancellationToken)
    {
        string sqlTextGetProductById = $"SELECT a.*,b.Name FROM Catalog.\"Product\" as a inner join Catalog.\"Brand\" as b on a.BrandId=b.Id  WHERE \"a.Id\"  = '@Id' AND \"TenantId\" = '@tenant'";
        return await _repository.QueryFirstOrDefaultAsync<ProductDto>(sqlTextGetProductById, new { Id = Id }, cancellationToken: cancellationToken);
    }

    public async Task<PaginationResponse<ProductDto>> SearchProductByName(string name, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        string sqlTextSearchProductByName = $"SELECT a.*,b.Name FROM Catalog.\"Product\" as a inner join Catalog.\"Brand\" as b on a.BrandId=b.Id   WHERE \"Name\"  like '%@Name%' AND \"TenantId\" = '@tenant'";
        var list = await _repository.QueryAsync<ProductDto>(SqlBuilderFactory.GetDataFromSqlBuilder(sqlTextSearchProductByName, pageNumber, pageSize), new { Name = name }, cancellationToken: cancellationToken);
        int count = await _repository.QuerySingleAsync<int>(SqlBuilderFactory.GetCountFromSqlBuilder(sqlTextSearchProductByName), new { Name = name }, cancellationToken: cancellationToken);
        return new PaginationResponse<ProductDto>(list.ToList(), count, pageNumber, pageSize);
    }

}
