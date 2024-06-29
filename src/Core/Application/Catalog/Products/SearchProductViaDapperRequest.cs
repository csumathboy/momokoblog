using csumathboy.Application.Catalog.Interfaces;
using csumathboy.Application.Catalog.Products;
namespace csumathboy.Application.Posts.Products;

public class SearchProductViaDapperRequest : IRequest<PaginationResponse<ProductDto>>
{
    public string? Name { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public SearchProductViaDapperRequest(string name) => Name = name;
}

public class SearchProductViaDapperRequestHandler : IRequestHandler<SearchProductViaDapperRequest, PaginationResponse<ProductDto>>
{
    private readonly IProductDapperSearchService _prodctService;
    private readonly IStringLocalizer _t;

    public SearchProductViaDapperRequestHandler(IProductDapperSearchService prodctService, IStringLocalizer<GetProductViaDapperRequestHandler> localizer) =>
        (_prodctService, _t) = (prodctService, localizer);

    public async Task<PaginationResponse<ProductDto>> Handle(SearchProductViaDapperRequest request, CancellationToken cancellationToken)
    {
        return await _prodctService.SearchProductByName(request.Name, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}