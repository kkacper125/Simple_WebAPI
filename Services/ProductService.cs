using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Simple_WebAPI.Interfaces.Repositories;
using Simple_WebAPI.Interfaces.Services;
using Simple_WebAPI.Models;
using Simple_WebAPI.Models.DTOs;

namespace Simple_WebAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository){
        _productRepository = productRepository; 
    }

    public async Task<IEnumerable<ProductResponseDTO>> GetProductsAsync() 
    {
        var products = await _productRepository.GetProductsAsync();

        return products.Select(p => new ProductResponseDTO 
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Cost = p.Cost
        }).ToArray();
    }

    public async Task<ProductResponseDTO?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
            return null;

        return new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Cost = product.Cost
        };
    }

    public async Task<ProductResponseDTO> CreateProductAsync(ProductUpsertDTO productDTO) 
    {
        var product = new Product 
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Cost = productDTO.Cost
        };

        var createdProduct = await _productRepository.CreateProductAsync(product);
        
        return new ProductResponseDTO
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Cost = createdProduct.Cost
        };
    }

    public async Task<ProductResponseDTO?> UpdateProductAsync(ProductUpsertDTO product, int id) 
    {
        var existingProduct = await _productRepository.GetProductByIdAsync(id);  

        if (existingProduct == null)
            return null;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Cost = product.Cost;

        var updatedProduct = await _productRepository.UpdateProductAsync(existingProduct);
        return new ProductResponseDTO
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Cost = updatedProduct.Cost
        };
    }

    public async Task<bool> DeleteProductAsync(int id) =>
        await _productRepository.DeleteProductAsync(id);
}
