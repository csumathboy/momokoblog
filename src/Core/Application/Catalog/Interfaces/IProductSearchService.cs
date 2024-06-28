using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csumathboy.Application.Catalog.Products;

namespace csumathboy.Application.Catalog.Interfaces;
public interface IProductSearchService
{
    public Task<ProductDto?> GetProductById(Guid Id, CancellationToken cancellationToken);

    public Task<IReadOnlyList<ProductDto>?> SearchProductByName(string name, CancellationToken cancellationToken);

}
