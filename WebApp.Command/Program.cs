using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Command.Models;

namespace WebApp.Command;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

        builder.Services.AddDbContext<AppIdentityDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnectionString"))
            );

        builder.Services.AddIdentity<User, IdentityRole>(
            options =>
            {
                options.User.RequireUniqueEmail = true;
                
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
        
        builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        
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
                
                foreach (var number in Enumerable.Range(1,30))
                {
                    db.Products.Add(new Product()
                    {
                        Name = $"Kalem {number}",
                        Price = number * 100,
                        Stock = number + 50
                    });
                }

                db.SaveChanges();
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