using csumathboy.Application.Catalog.Interfaces;
using csumathboy.Application.Catalog.Products;
using csumathboy.Application.Common.Models;
using csumathboy.Application.Common.Persistence;
using csumathboy.Infrastructure.Persistence.SqlBuilder;

namespace csumathboy.Infrastructure.Catalog;
public class ProductDapperSearchService : IProductDapperSearchService
{
    private readonly IDapperRepository _repository;

    private readonly ISqlBuilderService _sqlbuildService;

    /// <summary>
    ///  Important!!! （主要注意！！！）
    /// 1.Select sql must contain all of the collumn in ProductDto.
    /// 1、查询语句必须包含所有ProductDto中的字段，原来的代码使用Product，其实这样还要增加一次DTO转换
    /// 这样一来，可能就失去了使用Dapper的意义了
    /// 2.Diffrent database needed diffrent sql.
    /// 2、不同的数据库需要编写不同的sql语句，为了高效地使用Dapper查询，这个是不可避免的
    /// 3.Using Dapper are not quite interested in the abstractions provided by Specifications.
    /// On the contrary, we use Dapper to avoid such abstractions.
    /// 3、查了ardalis.Specifications类库在github上面的讨论，大家认为使用Dapper的人为了高效查询，
    /// 所以避免使用太多类似Specifications的封装，毕竟这样会耗掉很大性能.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="sqlbuildService"></param>
    public ProductDapperSearchService(IDapperRepository repository, ISqlBuilderService sqlbuildService) => (_repository, _sqlbuildService) = (repository, sqlbuildService);

    /// <summary>
    /// Get By Id.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto?> GetProductById(Guid Id, CancellationToken cancellationToken)
    {
        string sqlTextGetProductById = $"SELECT a.*,b.\"Name\" FROM \"Catalog\".\"Product\" as a inner join \"Catalog\".\"Brand\" as b on a.\"BrandId\"=b.\"Id\"  WHERE \"a.Id\"  = '@Id' AND \"TenantId\" = '@tenant'";
        return await _repository.QueryFirstOrDefaultAsync<ProductDto>(sqlTextGetProductById, new { Id = Id }, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// SearchByName.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PaginationResponse<ProductDto>> SearchProductByName(string? name, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        string searchConditions = "   \"Catalog\".\"Products\" as a inner join \"Catalog\".\"Brands\" as b on a.\"BrandId\"=b.\"Id\"   WHERE  a.\"TenantId\" = '@tenant'";
        if (!string.IsNullOrEmpty(name))
        {
            searchConditions += "AND a.\"Name\"  like '%@Name%'  ";
        }

        string sqlCountText = searchConditions;
        string sqlTextSearch = "select a.*,b.\"Name\" FROM" + searchConditions;
        var list = await _repository.QueryAsync<ProductDto>(_sqlbuildService.GetDataFromSqlBuilder(sqlTextSearch, pageNumber, pageSize), new { Name = name }, cancellationToken: cancellationToken);
        int count = await _repository.QuerySingleAsync<int>(_sqlbuildService.GetCountFromSqlBuilder(sqlCountText), new { Name = name }, cancellationToken: cancellationToken);
        return new PaginationResponse<ProductDto>(list.ToList(), count, pageNumber, pageSize);
    }

}
