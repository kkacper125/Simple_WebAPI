using System;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Simple_WebAPI.Controllers;
using Simple_WebAPI.Interfaces.Services;
using Simple_WebAPI.Models;
using Simple_WebAPI.Models.DTOs;

namespace Simple_WebAPI.unitTests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _controller = new ProductsController(_productServiceMock.Object);
    }

    public static IEnumerable<object[]> CreateProduct_ReturnsCreatedAtActionResult_TestData()
    {
        yield return new object[] 
        {
            new ProductUpsertDTO { Name="SomeName", Description = "SomeDescription", Cost=5000 },
            new ProductResponseDTO { Id = 1, Name = "SomeName", Description = "SomeDescription", Cost=5000}
        };

        yield return new object[]
        {
            new ProductUpsertDTO { Name = "Product Name 2", Description = "Product Description 2", Cost=1000},
            new ProductResponseDTO { Id = 1, Name = "Product Name 2", Description = "Product Description 2", Cost=1000}
        };
    }

    [Theory]
    [MemberData(nameof(CreateProduct_ReturnsCreatedAtActionResult_TestData))]
    public async Task ProductsController_CreateProduct_ReturnsCreatedAtActionResult(ProductUpsertDTO productToCreate, ProductResponseDTO newProduct)
    {
        //Arrange
        _productServiceMock
            .Setup(service => service.CreateProductAsync(It.IsAny<ProductUpsertDTO>()))
            .ReturnsAsync(newProduct);

        // Act
        var result = await _controller.CreateProduct(productToCreate);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetProductById", createdAtActionResult.ActionName);
        Assert.Equal(newProduct.Id, createdAtActionResult.RouteValues!["id"]);
        Assert.Equal(newProduct, createdAtActionResult.Value);
    }

    [Fact]
    public async Task CreateProduct_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var invalidProduct = new ProductUpsertDTO
        {
            Name = default!,
            Description = "",
            Cost = 1000
        }; 

        _controller.ModelState.AddModelError("Name", "Name is required"); 

        // Act
        var result = await _controller.CreateProduct(invalidProduct);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode); 
    }
}
