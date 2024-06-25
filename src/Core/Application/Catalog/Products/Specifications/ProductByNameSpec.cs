namespace csumathboy.Application.Catalog.Products.Specifications;

public class ProductByNameSpec : Specification<Product>, ISingleResultSpecification
{
    public ProductByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}