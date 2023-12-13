
using System.ComponentModel.DataAnnotations;
public class User
{
    [Required]
    public string? UserName { get; set; }
     [Required]
    public string? Password { get; set; }
     [Required]
     [Compare("Password")]
    public string? RepeatPassword { get; set; }
     [Required]
     [EmailAddress]
    public string? Email { get; set; }
     [Required]
     [StringLength(10)]
    public string? MobileNumber { get; set; }
}