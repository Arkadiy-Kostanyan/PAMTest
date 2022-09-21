using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PAM.Core.AgreementAggregate;

namespace PAM.Infrastructure.Data.Config;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.Property(t => t.ProductNumber)
        .IsRequired().HasColumnType("nvarchar(255)");

    builder.Property(t => t.Price)
        .HasPrecision(18, 4);

    builder.HasIndex(u => u.ProductNumber)
        .IsUnique();
  }
}
