using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApp.Decorator.Decorator;
using WebApp.Decorator.Models;
using WebApp.Decorator.Repositories;

namespace WebApp.Decorator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddScoped<IProductRepository>(sp =>
        {
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>();
            var db = sp.GetRequiredService<AppIdentityDbContext>();
            var memoryCache = sp.GetRequiredService<IMemoryCache>();
            var logger = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();
            ProductRepository productRepository = new ProductRepository(db);

            if (httpContext.HttpContext.User.Identity.Name == "user1")
            {
                var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository,memoryCache);
                return cacheDecorator;
            }
        
            
            var loggingDecorator = new ProductRepositoryLoggingDecorator(productRepository,logger);
            return loggingDecorator;
        });

        // builder.Services.AddScoped<IProductRepository, ProductRepository>()
        //     .Decorate<IProductRepository, ProductRepositoryCacheDecorator>()
        //     .Decorate<IProductRepository, ProductRepositoryLoggingDecorator>();
        
        builder.Services.AddMemoryCache();

        builder.Services.AddDbContext<AppIdentityDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnectionString"))
            );

        builder.Services.AddIdentity<User, IdentityRole>(
            options =>
            {
                options.User.RequireUniqueEmail = true;
                
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            db.Database.Migrate();
            if (!userManager.Users.Any())
            {
                userManager.CreateAsync(new User { UserName = "user1", Email = "user1@outlook.com" },"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user2", Email = "user2@outlook.com" },"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user3", Email = "user3@outlook.com" },"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user4", Email = "user4@outlook.com" },"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user5", Email = "user5@outlook.com" },"Abc1234.").Wait();
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}