using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Identity;

public class AppUser :IdentityUser<Guid>
{
    [Required]
    [StringLength(32)]
    [MaxLength(32)]
    public string Name { get; set; }
    
    [StringLength(260)]
    public string? Address { get; set; }   
}