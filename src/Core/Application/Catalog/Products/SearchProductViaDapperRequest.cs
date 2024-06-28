using csumathboy.Application.Catalog.Products;
using csumathboy.Application.Common.Interfaces;
using Mapster;
using System.Collections.Generic;

namespace csumathboy.Application.Posts.Products;

public class SearchProductViaDapperRequest : IRequest<List<ProductDto>>
{
    public string Name { get; set; }

    public SearchProductViaDapperRequest(string name) => Name = name;
}

public class SearchProductViaDapperRequestHandler : IRequestHandler<SearchProductViaDapperRequest, List<ProductDto>>
{
    private readonly IDapperRepository _repository;
    private readonly IStringLocalizer _t;

    public SearchProductViaDapperRequestHandler(IDapperRepository repository, IStringLocalizer<GetProductViaDapperRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<List<ProductDto>> Handle(SearchProductViaDapperRequest request, CancellationToken cancellationToken)
    {
        var productList = await _repository.QueryAsync<Product>(
            $"SELECT * FROM Catalog.\"Products\" WHERE \"Name\"  like '%@Id%' AND \"TenantId\" = '@tenant'", new { Name= request.Name }, cancellationToken: cancellationToken);

        _ = productList ?? throw new NotFoundException(_t["Products {0} Not Found.", request.Name]);

        // Using mapster here throws a nullreference exception because of the "BrandName" property
        return productList.Adapt<IReadOnlyList<ProductDto>>().ToList();
    }
}