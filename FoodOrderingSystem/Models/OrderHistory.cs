using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FoodOrderingSystem;
public class OrderHistory{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    [Required]
    public string? OrderId { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? ItemName { get; set; }
    [Required]
    public string? Quantity { get; set; }
    [Required]
    public string? ItemPrice { get; set; }
    [Required]
    public string? Total { get; set; }
    [Required]
    public string? Location { get; set; }
}