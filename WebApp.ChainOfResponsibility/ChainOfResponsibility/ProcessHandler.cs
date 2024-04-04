namespace WebApp.ChainOfResponsibility.ChainOfResponsibility;

public abstract class ProcessHandler : IProcessHandler
{
    private IProcessHandler _nextProcessHandler;
    
    public IProcessHandler SetNext(IProcessHandler processHandler)
    {
        _nextProcessHandler = processHandler;
        return _nextProcessHandler;
    }

    public virtual object Handle(object o)
    {
        if (_nextProcessHandler is not null)
            return _nextProcessHandler.Handle(o);
        return null;
    }
}