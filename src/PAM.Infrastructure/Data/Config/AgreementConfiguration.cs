using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PAM.Core.AgreementAggregate;

namespace PAM.Infrastructure.Data.Config;
public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
{
  public void Configure(EntityTypeBuilder<Agreement> builder)
  {

    builder.Property(t => t.UserId)
        .IsRequired().HasColumnType("nvarchar(450)");

    builder.Property(t => t.ProductPrice)
        .HasPrecision(18, 4);

    builder.Property(t => t.NewPrice)
        .HasPrecision(18, 4);
  }
}
