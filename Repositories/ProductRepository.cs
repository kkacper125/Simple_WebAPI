using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Simple_WebAPI.Contexts;
using Simple_WebAPI.Interfaces.Repositories;
using Simple_WebAPI.Models;
using Simple_WebAPI.Models.DTOs;

namespace Simple_WebAPI.Repositories;

public class ProductRepository : IProductRepository
{   
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync() =>
        await _dbContext.Products.ToListAsync();

    public async Task<Product?> GetProductByIdAsync(int id) => 
        await _dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetProductsBySearchAsync(string? name, uint? minPrice, uint? maxPrice, int offset=0, int limit=5)
    {
        var query = _dbContext.Products.OrderBy(p => p.Name).AsQueryable();

        if(!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if(minPrice.HasValue)
            query = query.Where(p => p.Cost >= minPrice.Value);

        if(maxPrice.HasValue)
            query = query.Where(p => p.Cost <= maxPrice.Value);
        
        return await query
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> GetNumberOfProductsAsync(string? name, uint? minPrice, uint? maxPrice){
        var query = _dbContext.Products.AsQueryable();

        if(!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if(minPrice.HasValue)
            query = query.Where(p => p.Cost >= minPrice.Value);

        if(maxPrice.HasValue)
            query = query.Where(p => p.Cost <= maxPrice.Value);
        
        return await query.CountAsync();
    }

    public async Task<uint> GetProductMinPriceAsync() =>
        await _dbContext.Products.MinAsync(p => p.Cost);

    public async Task<uint> GetProductMaxPriceAsync()=>
        await _dbContext.Products.MaxAsync(p => p.Cost);

    public async Task<Product> CreateProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id) 
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id); 
        if (product == null)
            return false;

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;    
    }
}
