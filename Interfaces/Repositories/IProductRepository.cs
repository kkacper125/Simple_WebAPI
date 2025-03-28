using Simple_WebAPI.Models;

namespace Simple_WebAPI.Interfaces.Repositories;

public interface IProductRepository
{
    public Task<IEnumerable<Product>> GetProductsAsync();
    public Task<IEnumerable<Product>> GetProductsBySearchAsync(string? name, uint? minPrice, uint? maxPrice, int offset=0, int limit=5);
    public Task<Product?> GetProductByIdAsync(int id);
    public Task<Product> CreateProductAsync(Product product);
    public Task<Product> UpdateProductAsync(Product product);
    public Task<bool> DeleteProductAsync(int id);
}
