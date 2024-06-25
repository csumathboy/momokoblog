using Finbuckle.MultiTenant.EntityFrameworkCore;
using csumathboy.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csumathboy.Infrastructure.Persistence.Configuration;

public class BrandConfig : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name).IsRequired()
                .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
    }
}

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name).IsRequired()
                .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

        builder
            .Property(p => p.ImagePath)
                .HasMaxLength(DataSchemaConstants.DEFAULT_IMAGEURL_LENGTH);
    }
}