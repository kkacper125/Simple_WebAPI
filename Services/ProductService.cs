using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Simple_WebAPI.Interfaces.Repositories;
using Simple_WebAPI.Interfaces.Services;
using Simple_WebAPI.Models;

namespace Simple_WebAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository){
        _productRepository = productRepository; 
    }

    public async Task<IEnumerable<Product>> GetProductsAsync() =>
        await _productRepository.GetProductsAsync();

    public async Task<Product?> GetProductByIdAsync(int id) => 
        await _productRepository.GetProductByIdAsync(id);

    public async Task<Product> CreateProductAsync(Product product) =>
        await _productRepository.CreateProductAsync(product); 

    public async Task<Product?> UpdateProductAsync(Product product, int id) 
    {
        var existingProduct = await _productRepository.GetProductByIdAsync(id);  

        if (existingProduct == null)
            return null;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Cost = product.Cost;

        return await _productRepository.UpdateProductAsync(existingProduct);
    }

    public async Task<bool> DeleteProductAsync(int id) =>
        await _productRepository.DeleteProductAsync(id);
}
