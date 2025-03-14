using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple_WebAPI.Interfaces.Services;
using Simple_WebAPI.Models;

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

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] Product product)
    {
        if(!ModelState.IsValid) 
            return BadRequest(ModelState);

        var newProduct = await _productService.CreateProductAsync(product);

        return CreatedAtAction(nameof(GetProductById), new {id = newProduct.Id}, newProduct);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateProduct([FromBody] Product product, int id)
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

