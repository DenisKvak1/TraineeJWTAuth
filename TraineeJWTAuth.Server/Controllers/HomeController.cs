using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HomeController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Privacy()
    {
        return Ok("Privacy page is work   (:>");
    }
}