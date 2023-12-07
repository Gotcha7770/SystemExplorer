using SystemExplorer.Core.Shared.BaseModels.Abstract;
using SystemExplorer.Core.Shared.ViewModels.Base;

namespace SystemExplorer.Core.Shared.Entities.Abstract;

public abstract class FileEntityViewModel : PageViewModelBase
{
    public string FullName { get; } = null!;

    protected FileEntityViewModel(string fullName)
    {
        FullName = fullName;
    }
}
