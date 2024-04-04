using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Composite.Models;

namespace WebApp.Composite;

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
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            db.Database.Migrate();
            if (!userManager.Users.Any())
            {
                var user1 = new User { UserName = "user1", Email = "user1@outlook.com" };
                userManager.CreateAsync(user1,"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user2", Email = "user2@outlook.com" },"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user3", Email = "user3@outlook.com" },"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user4", Email = "user4@outlook.com" },"Abc1234.").Wait();
                userManager.CreateAsync(new User { UserName = "user5", Email = "user5@outlook.com" },"Abc1234.").Wait();

                var newCategory1 = new Category() { Name = "Suç romanları", ReferenceId = 0, UserId = user1.Id };
                var newCategory2 = new Category() { Name = "Cinayet romanları", ReferenceId = 0, UserId = user1.Id };
                var newCategory3 = new Category() { Name = "Polisiye romanları", ReferenceId = 0, UserId = user1.Id };
                
                db.Categories.AddRange(newCategory1,newCategory2,newCategory3);
                db.SaveChanges();
                
                var newCategory3_1 = new Category()
                    { Name = "Sherlock Holmes polisiye romanları", ReferenceId = newCategory3.Id, UserId = user1.Id };
                var newCategory1_1 = new Category()
                    { Name = "Agatha Christie suç romanları", ReferenceId = newCategory1.Id, UserId = user1.Id };
                var newCategory2_1 = new Category()
                    { Name = "Tess Gerritsen cinayet romanları", ReferenceId = newCategory2.Id, UserId = user1.Id };
                
                db.Categories.AddRange(newCategory1_1, newCategory2_1, newCategory3_1);
                db.SaveChanges();
                
                var newCategory3_1_1 = new Category()
                    { Name = "Sherlock Holmes sevilen polisiye romanları", ReferenceId = newCategory3_1.Id, UserId = user1.Id };

                db.Categories.Add(newCategory3_1_1);
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