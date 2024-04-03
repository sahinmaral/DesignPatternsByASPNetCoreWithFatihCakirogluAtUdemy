using WebApp.Observer.Models;

namespace WebApp.Observer.Observer;

public class UserObserverWriteToConsole(IServiceProvider serviceProvider) : IUserObserver
{
    public Task UserCreated(User user)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<UserObserverWriteToConsole>>();
        logger.LogInformation($"User created => Id: {user.Id}");
        return Task.CompletedTask;
    }
}