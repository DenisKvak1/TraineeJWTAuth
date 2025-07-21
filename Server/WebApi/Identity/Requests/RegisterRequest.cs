using System.ComponentModel.DataAnnotations;

namespace WebApp.Identity.ViewModels;

public class RegisterRequest
{
    [Required] 
    public string? Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string?  Email { get; set; }
    
    [Required] 
    public string?  Password { get; set; }
   
    [StringLength(260)]
    public string? Address { get; set; }    
}