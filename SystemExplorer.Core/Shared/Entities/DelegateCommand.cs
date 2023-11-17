using System.Windows.Input;

namespace SystemExplorer.Core.Shared.Entities;

public class DelegateCommand : ICommand
{
    private readonly Action<object> _open;
    public event EventHandler? CanExecuteChanged;

    public DelegateCommand(Action<Object> open)
    {
        _open = open;
    }

    public bool CanExecute(object? parameter) 
        => true;

    public void Execute(object? parameter)
    {
        _open?.Invoke(parameter);
    }
}
