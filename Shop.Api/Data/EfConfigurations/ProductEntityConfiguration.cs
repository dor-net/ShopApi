using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Api.Entities;

namespace Shop.Api.Data.EfConfigurations;

public class ProductEntityConfiguration
    : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(x => x.Name)
          .IsRequired()
          .HasMaxLength(500);


        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);

        builder.HasMany(x => x.Images)
          .WithOne(x => x.Product)
          .HasForeignKey(x => x.ProductId);


    }
}
