using System;
using Simple_WebAPI.Models;

namespace Simple_WebAPI.Contexts.Seeders;

public static class ProductSeeder
{
    public static void Seed(AppDbContext dbContext)
    {
        if(!dbContext.Products.Any())
        {
            dbContext.Products.AddRange(new List<Product>
            {
                new Product {Id = 1, Name="Donut", Description="Round suggar thing", Cost=20_00, Created=DateTime.Today},
                new Product {Id = 2, Name="Pizza", Description="Italian round thing", Cost=360_00, Created=DateTime.Today},
                new Product {Id = 3, Name="Rice", Description="Asian potatos", Cost=23_00, Created=DateTime.Today},
                new Product {Id = 4, Name="Chicken", Description="Animal that says kukuryko", Cost=45_00, Created=DateTime.MinValue}
            });

            dbContext.SaveChanges();
        }
    }
}
