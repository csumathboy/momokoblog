using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csumathboy.Application.Catalog.Products;

namespace csumathboy.Application.Catalog.Interfaces;
public interface IProductDapperSearchService
{
    public Task<ProductDto?> GetProductById(Guid Id, CancellationToken cancellationToken);

    public Task<PaginationResponse<ProductDto>> SearchProductByName(string? name, int pageNumber, int pageSize, CancellationToken cancellationToken);

}
