namespace csumathboy.Application.Catalog.Products.Specifications;

public class ProductsByBrandSpec : Specification<Product>
{
    public ProductsByBrandSpec(DefaultIdType brandId) =>
        Query.Where(p => p.BrandId == brandId);
}
