using Microsoft.AspNetCore.Mvc;

namespace WebApp.Command.Commands;

public class FileCreateInvoker
{
    private ITableActionCommand _tableActionCommand;
    private List<ITableActionCommand> _tableActionCommands = new List<ITableActionCommand>();

    public void SetCommand(ITableActionCommand tableActionCommand)
    {
        _tableActionCommand = tableActionCommand;
    }

    public void AddCommand(ITableActionCommand tableActionCommand)
    {
        _tableActionCommands.Add(tableActionCommand);
    }

    public IActionResult CreateFile()
    {
        return _tableActionCommand.Execute();
    }

    public List<IActionResult> CreateFiles()
    {
        return _tableActionCommands.Select(command => command.Execute()).ToList();
    }
}