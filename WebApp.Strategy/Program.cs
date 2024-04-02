using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.Models;
using WebApp.Strategy.Repositories;

namespace WebApp.Strategy;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Add services to the container.
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

        builder.Services.AddScoped<IProductRepository>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var dbContext = sp.GetRequiredService<AppIdentityDbContext>();
            var databaseTypeClaim = httpContextAccessor.HttpContext.User.Claims
                .Where(c => c.Type == Settings.ClaimDatabaseType)
                .FirstOrDefault();
            if (databaseTypeClaim is null)
                return new ProductRepositoryFromSqlServer(dbContext);

            var databaseTypeClaimValue = (EDatabaseType)int.Parse(databaseTypeClaim.Value);
            return databaseTypeClaimValue switch
            {
                EDatabaseType.SqlServer => new ProductRepositoryFromSqlServer(dbContext),
                EDatabaseType.MongoDb => new ProductRepositoryFromMongoDb(builder.Configuration),
                _ => throw new NotImplementedException()
            };

        });

        builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnectionString"))
        );

        builder.Services.AddIdentity<User, IdentityRole>(
            options => { options.User.RequireUniqueEmail = true; }).AddEntityFrameworkStores<AppIdentityDbContext>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            db.Database.Migrate();
            if (!userManager.Users.Any())
            {
                userManager.CreateAsync(new User { UserName = "user1", Email = "user1@outlook.com" }, "Abc1234.")
                    .Wait();
                userManager.CreateAsync(new User { UserName = "user2", Email = "user2@outlook.com" }, "Abc1234.")
                    .Wait();
                userManager.CreateAsync(new User { UserName = "user3", Email = "user3@outlook.com" }, "Abc1234.")
                    .Wait();
                userManager.CreateAsync(new User { UserName = "user4", Email = "user4@outlook.com" }, "Abc1234.")
                    .Wait();
                userManager.CreateAsync(new User { UserName = "user5", Email = "user5@outlook.com" }, "Abc1234.")
                    .Wait();
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");

        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAuthorization();

        app.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}