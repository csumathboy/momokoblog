namespace csumathboy.Application.Catalog.Products.Specifications;

public class ProductByIdWithBrandSpec : Specification<Product, ProductDetailsDto>, ISingleResultSpecification
{
    public ProductByIdWithBrandSpec(DefaultIdType id) =>
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Brand);
}