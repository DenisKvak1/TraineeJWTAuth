using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Identity;
using WebApp.Identity.Context;

namespace WebApp.Data;

public class AppDbContext : TokenIdentityContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}