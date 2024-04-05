using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Decorator.Models;

public class AppIdentityDbContext : IdentityDbContext<User>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
}