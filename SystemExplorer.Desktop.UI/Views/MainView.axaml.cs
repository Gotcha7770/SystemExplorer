using Avalonia;
using Avalonia.Controls;

namespace SystemExplorer.Desktop.UI.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }
}
