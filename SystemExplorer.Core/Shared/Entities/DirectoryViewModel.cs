using SystemExplorer.Core.Shared.Entities.Abstract;

namespace SystemExplorer.Core.Shared.Entities;

public sealed class DirectoryViewModel : FileEntityViewModel
{
    public string Name { get; set; }

    public DirectoryViewModel(string directoryName, string directoryFullName) 
        : base(directoryFullName)
    {
        Name = directoryName;
    }

    public DirectoryViewModel(DirectoryInfo directoryName)
        : base(directoryName.FullName)
    {
        Name = directoryName.Name;
    }
}
