using Simple_WebAPI.Models;
using Simple_WebAPI.Models.DTOs;

namespace Simple_WebAPI.Interfaces.Services;

public interface IProductService
{
    public Task<IEnumerable<ProductResponseDTO>> GetProductsAsync();
    public Task<IEnumerable<ProductResponseDTO>> GetProductsBySearchAsync(string? name, uint? minPrice, uint? maxPrice, int offset=0, int limit=5);
    public Task<ProductResponseDTO?> GetProductByIdAsync(int id);
    public Task<ProductResponseDTO> CreateProductAsync(ProductUpsertDTO productDTO);
    public Task<ProductResponseDTO?> UpdateProductAsync(ProductUpsertDTO product, int id);
    public Task<bool> DeleteProductAsync(int id);
}
