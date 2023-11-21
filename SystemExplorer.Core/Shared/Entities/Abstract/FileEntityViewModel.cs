using SystemExplorer.Core.Shared.BaseModels.Abstract;

namespace SystemExplorer.Core.Shared.Entities.Abstract;

public abstract class FileEntityViewModel : BaseViewModel
{
    public string FullName { get; } = null!;

    protected FileEntityViewModel(string fullName)
    {
        FullName = fullName;
    }
}
