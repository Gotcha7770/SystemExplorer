using SystemExplorer.Core.Shared.Entities.Abstract;

namespace SystemExplorer.Core.Shared.Entities;

public sealed class DirectoryViewModel : FileEntityViewModel
{
    public string Name { get; set; }

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
