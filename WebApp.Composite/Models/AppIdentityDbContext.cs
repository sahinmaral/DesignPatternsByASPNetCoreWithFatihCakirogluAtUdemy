using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Composite.Models;

public class AppIdentityDbContext : IdentityDbContext<User>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
}