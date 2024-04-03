using WebApp.Observer.Models;

namespace WebApp.Observer.Observer;

public class UserObserverCreateDiscount(IServiceProvider serviceProvider) : IUserObserver
{
    public async Task UserCreated(User user)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<UserObserverCreateDiscount>>();

        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();    
            await context.Discounts.AddAsync(new Discount()
            {
                Rate = 10, UserId = user.Id
            });
            await context.SaveChangesAsync();
        }
        
        logger.LogInformation("Discount created");
    }
}