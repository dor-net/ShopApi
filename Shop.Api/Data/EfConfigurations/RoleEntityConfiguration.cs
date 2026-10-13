
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Api.Entities;

namespace Shop.Api.Data.EfConfigurations;

public class RoleEntityConfiguration
    : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.Property(x => x.Name)
           .IsRequired()
           .HasMaxLength(100);

        builder.Property(x => x.DisplayName)
          .IsRequired()
          .HasMaxLength(100);



    }

}
