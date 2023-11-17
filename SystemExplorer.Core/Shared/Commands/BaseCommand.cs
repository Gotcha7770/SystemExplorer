using System.Windows.Input;
using SystemExplorer.Shared.Commands;

namespace SystemExplorer.Core.Shared.Commands;

public class BaseCommand : ICommand
{
    #region Delegates
    private readonly Action execute;
    private readonly Func<bool> canExecute;
    private readonly Action RaiseCanExecuteChangedAction;
    #endregion

    #region Events
    public event EventHandler? CanExecuteChanged;

    #endregion

    #region Properties
    bool ICommand.CanExecute(object? parameter) => CanExecute;

    public bool CanExecute
    {
        
        get { return canExecute == null || canExecute(); }
    }
    #endregion

    #region Ctor
    public BaseCommand(Action execute)
        : this(execute, null)
    { }

    public BaseCommand(Action execute, Func<bool> canExecute)
    {
        this.execute = execute ?? throw new ArgumentNullException("execute is null.");
        this.canExecute = canExecute;
        this.RaiseCanExecuteChangedAction = RaiseCanExecuteChanged;
        CommandManager.AddRaiseCanExecuteChangedAction(ref RaiseCanExecuteChangedAction);
    }

    ~BaseCommand() => RemoveCommand();
    #endregion

    #region Public Methods
    public void RemoveCommand() => CommandManager
        .RemoveRaiseCanExecuteChangedAction(RaiseCanExecuteChangedAction);

    public void Execute(object? parameter)
    {
        execute();
        CommandManager.RefreshCommandStates();
    }

    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, new EventArgs());
    #endregion
}
