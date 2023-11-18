using SystemExplorer.Core.Shared.Entities.Abstract;

namespace SystemExplorer.Core.Shared.Entities;

public sealed class FileViewModel : FileEntityViewModel
{
    public string FullName { get; set; }

    public FileViewModel(string fileName) 
        : base(fileName)
    {
        FullName = fileName;
    }

    public FileViewModel(FileInfo fileName)
        : base(fileName.FullName)
    {
        FullName = fileName.FullName;
    }
}
