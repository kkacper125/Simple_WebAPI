using System;
using System.ComponentModel.DataAnnotations;

namespace Simple_WebAPI.Models.DTOs;

public class ProductUpsertDTO
{
    [Required, StringLength(255)]
    public required string Name { get; set; }
    [StringLength(1024), MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
    public uint Cost { get; set; }
}
