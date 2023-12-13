using System.ComponentModel.DataAnnotations;
namespace FoodOrderingSystem.Models;
public class FeedBack
{
    public string? UserName { get; set; }
    public string? EMail{ get; set; }
    public string? Rating{ get; set; }
    public string? Message{ get; set; }

}