using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Api.Entities;

namespace Shop.Api.Data.EfConfigurations;

public class ProductImageEntityConfiguration
    : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(1000);

    }

}
