using WebApp.Observer.Models;

namespace WebApp.Observer.Observer;

public interface IUserObserver
{
    Task UserCreated(User user);
}


