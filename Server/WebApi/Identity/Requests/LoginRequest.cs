using System.ComponentModel.DataAnnotations;

namespace WebApp.Identity.ViewModels;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}