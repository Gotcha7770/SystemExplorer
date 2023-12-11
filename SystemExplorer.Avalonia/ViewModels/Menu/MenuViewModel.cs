using ReactiveUI;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemExplorer.Avalonia.ViewModels.Interfaces;

namespace SystemExplorer.Avalonia.ViewModels.Menu;

public class MenuViewModel : ViewModelBase, IMenuViewModel
{
    private readonly IDialogService _dialogService;

    public ICommand ExitCommand { get; }

    public ICommand AboutCommand { get; }

    public ICommand OpenSettingsCommand { get; }

    public MenuViewModel(
        IApplicationCloser applicationCloser,
        IDialogService dialogService)
    {
        _dialogService = dialogService;

        ExitCommand = ReactiveCommand.Create(applicationCloser.CloseApp);
        AboutCommand = ReactiveCommand.CreateFromTask(ShowAboutDialogAsync);
        OpenSettingsCommand = ReactiveCommand.CreateFromTask(ShowSettingsDialogAsync);
    }

    private Task ShowAboutDialogAsync() => _dialogService.ShowDialogAsync(nameof(AboutDialogViewModel));

    private Task ShowSettingsDialogAsync() => _dialogService.ShowDialogAsync(nameof(SettingsDialogViewModel));
}
