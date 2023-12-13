using System.ComponentModel.DataAnnotations;

namespace FoodOrderingSystem.Models;

public class Cart{
    [Key]
    public string? ItemName { get; set; }
    public string? ItemPrice { get; set; }
    public string? UserName { get; set; }

    public string? Quantity { get; set; }

    public string? TotalPrice { get; set; }
    
    public string? Bay { get; set; }

    
}