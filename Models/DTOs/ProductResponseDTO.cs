using System;

namespace Simple_WebAPI.Models.DTOs;

public class ProductResponseDTO
{
    public int Id { get; set;}
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public uint Cost { get; set; }

}
