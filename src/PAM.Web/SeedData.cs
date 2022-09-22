using Microsoft.EntityFrameworkCore;
using PAM.Core.AgreementAggregate;
using PAM.Infrastructure.Data;

namespace PAM.Web
{
  public static class SeedData
  {
    public static void Initialize(IServiceProvider serviceProvider)
    {
      using (var dbContext = new AppDbContext(
          serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
      {
        if (!dbContext.Agreements.Any())
        {
          PopulateTestData(dbContext);   
        }
      }
    }
    public static void PopulateTestData(AppDbContext dbContext)
    {
      ProductGroup g1 = new ProductGroup();
      g1.GroupCode = "G1";
      g1.Description = "G1 Description";
      var g2 = new ProductGroup();
      g2.GroupCode = "G2";
      g2.Description = "G2 Description";

      dbContext.ProductGroups.Add(g1);
      dbContext.ProductGroups.Add(g2);

      dbContext.SaveChanges();

      Product p1 = new Product();
      p1.ProductNumber = "P1";
      p1.Description = "P1 Description";
      p1.ProductGroup = g1;
      p1.Price = 10m;

      Product p2 = new Product();
      p2.ProductNumber = "P2";
      p2.Description = "P2 Description";
      p2.ProductGroup = g1;
      p2.Price = 20m;

      Product p3 = new Product();
      p3.ProductNumber = "P3";
      p3.Description = "P3 Description";
      p3.ProductGroup = g2;
      p3.Price = 30m;

      Product p4 = new Product();
      p4.ProductNumber = "P4";
      p4.Description = "P4 Description";
      p4.ProductGroup = g2;
      p4.Price = 40m;

      dbContext.Products.Add(p1);
      dbContext.Products.Add(p2);
      dbContext.Products.Add(p3);
      dbContext.Products.Add(p4);

      dbContext.SaveChanges();
    }
  }
}
