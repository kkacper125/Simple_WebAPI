using System;

namespace Simple_WebAPI.Contexts.Seeders;

public static class DbSeeder
{
    public static void SeedDatabase(AppDbContext dbContext)
    {
        ProductSeeder.Seed(dbContext);
    }
}
