using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using SystemExplorer.Core.Shared.BaseModels.Abstract;
using SystemExplorer.Core.Shared.Commands;
using SystemExplorer.Core.Shared.Entities;
using SystemExplorer.Core.Shared.Entities.Abstract;
using SystemExplorer.Core.Shared.ViewModels;

namespace SystemExplorer.Core.Shared.BaseModels;

public class MainViewModel : BaseViewModel
{
    #region Private Variables
    private ObservableCollection<DirectoryTabItemViewModel> directoryTabItems = new();
    private DirectoryTabItemViewModel? currentDirectoryTabItem;
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
    public DirectoryTabItemViewModel? CurrentDirectoryTabItem 
    { 
        get => currentDirectoryTabItem;
        set 
        {
            Set(ref currentDirectoryTabItem, value);
            OnPropertyChanged();
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

        DirectoryTabItems.Add(vm);
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
