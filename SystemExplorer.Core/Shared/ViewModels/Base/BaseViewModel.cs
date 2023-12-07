using ReactiveUI;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SystemExplorer.Core.Shared.BaseModels.Abstract;

public class BaseViewModel
    : ReactiveObject, INotifyPropertyChanged
{
    #region Public Properties
    #endregion

    #region Ctor
    public BaseViewModel() { }
    #endregion

    #region Events
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region Protected Methods
    protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
    }

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? PropertyName = null)
    {
        if (Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(PropertyName);
        return true;
    }
    #endregion
}
