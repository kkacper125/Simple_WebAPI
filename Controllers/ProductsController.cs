using Microsoft.AspNetCore.Mvc;
using Simple_WebAPI.Interfaces.Services;
using Simple_WebAPI.Models.DTOs;


namespace Simple_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();

        if (!products.Any())
            return NoContent();    

        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<ActionResult> GetProductsBySearch(string? name, uint? minPrice, uint? maxPrice, int offset = 0, int limit = 5)
    {
        var products = await _productService.GetProductsBySearchAsync(name, minPrice, maxPrice, offset, limit);

        return Ok(products);
    }

    [HttpGet("prices")]
    public async Task<ActionResult> GetProductsPrices()
    {
        var prices = await _productService.GetProductsPricesAsync();
        return Ok(prices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] ProductUpsertDTO product)
    {
        if(!ModelState.IsValid) 
            return BadRequest(ModelState);

        var newProduct = await _productService.CreateProductAsync(product);

        return CreatedAtAction(nameof(GetProductById), new {id = newProduct.Id}, newProduct);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateProduct([FromBody] ProductUpsertDTO product, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedProduct = await _productService.UpdateProductAsync(product, id);

        if(updatedProduct == null)
            return NotFound();

        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductById(int id)
    {
        if(await _productService.DeleteProductAsync(id))
            return NoContent();
        
        return NotFound(new { Message = "Product not found." });
    }
}

