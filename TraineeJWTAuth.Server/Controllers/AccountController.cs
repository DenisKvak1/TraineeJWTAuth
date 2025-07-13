using Microsoft.AspNetCore.Mvc;
using TraineeJWTAuth.Server.Responces;
using WebApp.Identity;
using WebApp.Identity.Interfaces;
using WebApp.Identity.Responses;
using WebApp.Identity.Services;
using WebApp.Identity.ViewModels;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountController(
    AppUserManager _userManager,
    TokenService _tokenService,
    IConfiguration _configuration
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return BadRequest("Email or password is incorrect");
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        var isEmailConfirm = user.EmailConfirmed;
        var isUserLocked = user.LockoutEnabled;
        if (!isPasswordCorrect) return BadRequest("Email or password is incorrect");
        if (!isEmailConfirm) return BadRequest("Email not confirm");
        if (!isUserLocked) return BadRequest("Your account is locked");

        var accessToken = _tokenService.GenerateAccessToken(user);

        if (request.RememberMe)
        {
            var refreshToken = await _tokenService.GenerateRefreshToken(user.Id);
            AddRefreshCookie(refreshToken);
        }

        return Ok(new LoginResponse(accessToken));
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        AppUser user = new AppUser
        {
            EmailConfirmed = true,
            UserName = request.Email,
            Name = request.Name!,
            Email = request.Email,
            Address = request.Address,
        };
        var result = await _userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded) return BadRequest(result.Errors.Select(x => x.Description));

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = await _tokenService.GenerateRefreshToken(user.Id);

        AddRefreshCookie(refreshToken);

        return Ok(new RegisterResponse(accessToken));
    }

    [HttpGet]
    public async Task<IActionResult> Refresh()
    {
        string? refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Refresh token not found");
        (bool isValid, Guid userId) = await _tokenService.VerifyRefreshToken(refreshToken);
        if (!isValid) return BadRequest("Refresh token is invalid");

        AppUser user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return BadRequest("User not found");

        var accessToken = _tokenService.GenerateAccessToken(user);
        return Ok(new RefreshResponse(accessToken));
    }
    
    private void AddRefreshCookie(string refreshToken)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var ttl = jwtSettings.GetValue<int>("refreshTokenLifetime");

        Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None, // FOR TEST
            Expires = DateTime.UtcNow.AddSeconds(ttl),
            Path = Url.Action(nameof(Refresh))
        });
    }
}