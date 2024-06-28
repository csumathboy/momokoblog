using csumathboy.Application.Catalog.Interfaces;
using csumathboy.Application.Catalog.Products;
using csumathboy.Application.Common.Persistence;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Infrastructure.Catalog;
public class ProductSearchService : IProductSearchService
{
    private readonly IDapperRepository _repository;
    public ProductSearchService(IDapperRepository repository) => _repository = repository;

    public async Task<ProductDto?> GetProductById(Guid Id, CancellationToken cancellationToken)
    {
        string sqlTextGetProductById = $"SELECT a.*,b.Name FROM Catalog.\"Product\" as a inner join Catalog.\"Brand\" as b on a.BrandId=b.Id  WHERE \"a.Id\"  = '@Id' AND \"TenantId\" = '@tenant'";
        return await _repository.QueryFirstOrDefaultAsync<ProductDto>(sqlTextGetProductById, new { Id = Id }, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<ProductDto>?> SearchProductByName(string name, CancellationToken cancellationToken)
    {
        string sqlTextSearchProductByName = $"SELECT a.*,b.Name FROM Catalog.\"Product\" as a inner join Catalog.\"Brand\" as b on a.BrandId=b.Id   WHERE \"Name\"  like '%@Name%' AND \"TenantId\" = '@tenant'";
        return await _repository.QueryAsync<ProductDto>(sqlTextSearchProductByName, new { Name = name }, cancellationToken: cancellationToken);
    }

}
