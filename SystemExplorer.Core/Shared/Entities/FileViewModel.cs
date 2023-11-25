using SystemExplorer.Core.Shared.Entities.Abstract;

namespace SystemExplorer.Core.Shared.Entities;

public sealed class FileViewModel : FileEntityViewModel
{
    public string Name { get; set; }


    public FileViewModel(string fileFullName, string fileName) 
        : base(fileFullName)
    {
        Name = fileName;
    }

    public FileViewModel(FileInfo fileName)
        : base(fileName.FullName)
    {
        Name = fileName.Name;
    }
}
