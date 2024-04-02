using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Strategy.Models;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Product> Products { get; set; }
}