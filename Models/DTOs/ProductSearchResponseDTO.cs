using System;

namespace Simple_WebAPI.Models.DTOs;

public class ProductSearchResponseDTO
{
    public IEnumerable<ProductResponseDTO> Products { get; set; } = [];
    public int TotalCount { get; set; } = 0;
}
