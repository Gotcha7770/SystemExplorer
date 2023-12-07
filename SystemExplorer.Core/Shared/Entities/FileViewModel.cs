using SystemExplorer.Core.Shared.Entities.Abstract;

namespace SystemExplorer.Core.Shared.Entities;

public sealed class FileViewModel : FileEntityViewModel
{
    public string Name { get; set; }
    public string? Extension => Name[Name.LastIndexOf(".")..];

    public override bool CanNavigateNext 
    { 
        get => throw new NotImplementedException(); 
        protected set => throw new NotImplementedException(); 
    }
    public override bool CanNavigatePrevious 
    { 
        get => throw new NotImplementedException(); 
        protected set => throw new NotImplementedException(); 
    }

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
