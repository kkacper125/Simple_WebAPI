namespace Simple_WebAPI.Models.DTOs;

public class ProductPricesDTO
{
    public uint MinPrice { get; set; } = 0;
    public required uint MaxPrice { get; set; }
}
