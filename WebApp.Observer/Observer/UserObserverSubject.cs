using WebApp.Observer.Models;

namespace WebApp.Observer.Observer;

public class UserObserverSubject
{
    private readonly List<IUserObserver> _userObservers;

    public UserObserverSubject()
    {
        _userObservers = new List<IUserObserver>();
    }

    public void RegisterObserver(IUserObserver userObserver)
    {
        _userObservers.Add(userObserver);
    }
    
    public void RemoveObserver(IUserObserver userObserver)
    {
        _userObservers.Remove(userObserver);
    }
    
    public async Task NotifyObservers(User user)
    {
        foreach (var observer in _userObservers)
        {
            await observer.UserCreated(user);
        }
    }
}

