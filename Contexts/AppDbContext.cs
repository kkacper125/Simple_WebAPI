using System;
using Microsoft.EntityFrameworkCore;
using Simple_WebAPI.Models;

namespace Simple_WebAPI.Contexts;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Product> Products{ get; set; }
}
