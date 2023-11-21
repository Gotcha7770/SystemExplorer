using System.Collections.ObjectModel;
using System.Windows.Input;
using SystemExplorer.Core.Shared.BaseModels.Abstract;
using SystemExplorer.Core.Shared.Commands;


namespace SystemExplorer.Core.Shared.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region Private Variables
    private ObservableCollection<DirectoryTabItemViewModel> directoryTabItems = new();
    private DirectoryTabItemViewModel currentDirectoryTabItem = new();
    #endregion

    #region Public Properties
    public ObservableCollection<DirectoryTabItemViewModel> DirectoryTabItems 
    { 
        get => directoryTabItems;
        set
        {
            Set(ref  directoryTabItems, value);
            OnPropertyChanged();
        }
    }
    public DirectoryTabItemViewModel CurrentDirectoryTabItem 
    { 
        get => currentDirectoryTabItem;
        set 
        {
            Set(ref currentDirectoryTabItem, value);
            OnPropertyChanged(nameof(CurrentDirectoryTabItem));
        }
    }
    #endregion

    #region Ctor
    public MainViewModel()
    {
        AddTabItem();
    }
    #endregion

    #region Commands
    public ICommand AddTabItemCommand =>
        new DelegateCommand(AddTabItem);

    private void AddTabItem(object obj) =>
        AddTabItem();
    #endregion

    #region Private Command Methods
    #endregion

    #region Private Methods
    private void AddTabItem()
    {
        var vm = new DirectoryTabItemViewModel();
        vm.Closed += VMClosed;

        directoryTabItems.Add(vm);

        CurrentDirectoryTabItem = vm;
    }



    private void VMClosed(object sender, EventArgs e)
    {
        if (sender is DirectoryTabItemViewModel directoryVM)
        {
            CloseTab(directoryVM);
        }
    }

    private void CloseTab(DirectoryTabItemViewModel directoryVM)
    {
        directoryVM.Closed -= VMClosed;

        DirectoryTabItems.Remove(directoryVM);
    }
    #endregion
}
