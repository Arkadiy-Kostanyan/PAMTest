using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PAM.Core.AgreementAggregate;

namespace PAM.Infrastructure.Data.Config;
public class ProductGroupConfiguration : IEntityTypeConfiguration<ProductGroup>
{
  public void Configure(EntityTypeBuilder<ProductGroup> builder)
  {
    builder.Property(t => t.GroupCode)
        .IsRequired().HasColumnType("nvarchar(255)");

    builder.HasIndex(u => u.GroupCode)
        .IsUnique();
  }
}
