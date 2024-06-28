using csumathboy.Application.Catalog.Interfaces;
using csumathboy.Domain.PostsAggregate;
using Mapster;
using System;

namespace csumathboy.Application.Catalog.Products;

public class GetProductViaDapperRequest : IRequest<ProductDto>
{
    public Guid Id { get; set; }

    public GetProductViaDapperRequest(Guid id) => Id = id;
}

public class GetProductViaDapperRequestHandler : IRequestHandler<GetProductViaDapperRequest, ProductDto>
{
    private readonly IProductSearchService _prodctService;
    private readonly IStringLocalizer _t;
    public GetProductViaDapperRequestHandler(IStringLocalizer<GetProductViaDapperRequestHandler> localizer, IProductSearchService prodctService) =>
         (_t, _prodctService) = (localizer, prodctService);

    public async Task<ProductDto> Handle(GetProductViaDapperRequest request, CancellationToken cancellationToken)
    {
        var product = await _prodctService.GetProductById(request.Id, cancellationToken);
        _ = product ?? throw new NotFoundException(_t["product {0} Not Found.", request.Id]);

        return product;

    }
}