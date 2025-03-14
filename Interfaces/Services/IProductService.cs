using Simple_WebAPI.Models;

namespace Simple_WebAPI.Interfaces.Services;

public interface IProductService
{
    public Task<IEnumerable<Product>> GetProductsAsync();
    public Task<Product?> GetProductByIdAsync(int id);
    public Task<Product> CreateProductAsync(Product product);
    public Task<Product?> UpdateProductAsync(Product product, int id);
    public Task<bool> DeleteProductAsync(int id);
}
