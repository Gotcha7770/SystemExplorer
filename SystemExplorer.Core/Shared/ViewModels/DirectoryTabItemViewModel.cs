using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using SystemExplorer.Core.Shared.BaseModels.Abstract;
using SystemExplorer.Core.Shared.Commands;
using SystemExplorer.Core.Shared.Entities.Abstract;
using SystemExplorer.Core.Shared.Entities;

namespace SystemExplorer.Core.Shared.ViewModels;
/// <summary>
/// TODO: <br/>
/// 1) Use certain ENUM for a corresponding file system? <br/>
/// 2) Distribute methods to their ViewModels and make them static <br/>
/// 3) Improve methods create, delete, update (they`re too heavy and partially dumb as ****) <br/>
/// </summary>
public class DirectoryTabItemViewModel : BaseViewModel
{
    #region Private Variables
    private string filePath = string.Empty;
    private string name = string.Empty;
    private string newFileNameInput = "Введите название файла:";
    private List<string> fileExtensions = new List<string>() 
    { ".txt", ".doc", ".xml", ".zip"};
    private string? selectedExtension;
    private ObservableCollection<FileEntityViewModel> directories = new();
    private FileEntityViewModel? selectedFile;
    #endregion Private Variables

    public string FilePath
    {
        get => filePath;
        set
        {
            Set(ref filePath, value);
            OnPropertyChanged(nameof(FilePath));
        }
    }

    public string Name
    {
        get => name;
        set
        {
            Set(ref name, value);
            OnPropertyChanged();
        }
    }

    public string NewFileNameInput
    {
        get => newFileNameInput;
        set
        {
            Set(ref newFileNameInput, value);
            OnPropertyChanged();
        }
    }

    public List<string> FileExtensions 
        => fileExtensions;

    public string? SelectedExtension
    {
        get => selectedExtension;
        set
        {
            Set(ref selectedExtension, value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<FileEntityViewModel> Directories
    {
        get => directories;
        set
        {
            Set(ref directories, value);
            OnPropertyChanged();
        }
    }

    public FileEntityViewModel? SelectedFile
    {
        get => selectedFile;
        set
        {
            Set(ref selectedFile, value);
            OnPropertyChanged();
        }
    }

    public DirectoryTabItemViewModel()
    {
        Name = "This computer";

        foreach (var dir in Directory.GetLogicalDrives())
            Directories.Add(new DirectoryViewModel(new DirectoryInfo(dir)));
    }

    #region Commands
    public ICommand OpenCommand =>
        new DelegateCommand(Open);

    public ICommand OpenTargetDirectoryCommand =>
        new DelegateCommand(OpenTargetDirectory);

    public ICommand CreateDirectoryCommand =>
        new DelegateCommand(CreateDirectory);

    public ICommand CreateFileCommand =>
        new DelegateCommand(CreateFile);

    public ICommand UpdateCommand =>
        new DelegateCommand(Update);

    public ICommand DeleteCommand =>
        new DelegateCommand(Delete);

    public ICommand CloseCommand =>
        new DelegateCommand(Close);
    #endregion

    public event EventHandler? Closed;

    private void Close(object obj) =>
        Closed?.Invoke(this, EventArgs.Empty);
    private void Open(object? parameter = null)
    {
        if (parameter is DirectoryViewModel directoryViewModel)
        {
            FilePath = directoryViewModel.FullName;
            Name = directoryViewModel.Name;
            Directories.Clear();

            SwitchDirectory();
        }
        else if (parameter is FileViewModel fileViewModel)
        {
            ExecuteFile(fileViewModel);
        }
    }
    private void OpenTargetDirectory(object? parameter = null)
    {
        string? filePath = parameter?.ToString();
        if (!string.IsNullOrEmpty(filePath))
        {
            var DirectoryVM = new DirectoryViewModel(new DirectoryInfo(filePath));
            Open(DirectoryVM);
        }
    }
    private void CreateDirectory(object? parameter)
    {
        if (string.IsNullOrEmpty(parameter?.ToString())) return;

        string path = FilePath + @"\Новая папка";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        else
        {
            var t = Directory.GetDirectories(FilePath)
                .Select(s => s[s.LastIndexOf('\\')..][1..])
                .Where(s => s.Contains("Новая папка") && !s.Equals("Новая папка"));

            if (!t.Any())
            {
                Directory.CreateDirectory(path + " (1)");
            }
            else
            {
                int newDirectoryIndex = t
                    .Where(s => int.TryParse(s[(s.IndexOf('(') + 1)..s.IndexOf(')')], out _))
                    .Select(s => int.Parse(s[(s.IndexOf('(') + 1)..s.IndexOf(')')])).Max();

                Directory.CreateDirectory(path + $" ({newDirectoryIndex + 1})");
            }
        }
    }
    private void CreateFile(object? parameter)
    {
        if (string.IsNullOrEmpty(NewFileNameInput?.ToString())) 
            NewFileNameInput = "New_file";
        if (string.IsNullOrEmpty(SelectedExtension?.ToString())) 
            SelectedExtension = ".txt";

        string path = FilePath + $"\\{NewFileNameInput}{SelectedExtension}";

        if (!File.Exists(path))
        {
            File.Create(path);
        }
        else
        {
            throw new Exception("Файл с данным названием уже существует в указанном расположении.");
        }
        NewFileNameInput = "Введите название файла: ";
    }
    private void Update(object? parameter)
    {
        string? path = SelectedFile?.FullName[..SelectedFile.FullName.LastIndexOf(@"\")];

        if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(NewFileNameInput))
        {
            switch (SelectedFile)
            {
                case DirectoryViewModel directory:
                    {
                        DirectoryInfo dirinfo = new(directory.FullName);
                        dirinfo.MoveTo(Path.Combine(path, NewFileNameInput));
                        break;
                    }

                case FileViewModel file:
                    {
                        string supposedName = $"{NewFileNameInput}{file.Extension}";

                        bool canRename = !Directories
                            .Where(d => d is FileViewModel)
                            .Cast<FileViewModel>()
                            .Any(f => f.Name.Equals(supposedName));

                        if (canRename)
                        {
                            File.Move(SelectedFile.FullName, Path.Combine(path, supposedName));
                        }

                        break;
                    }
            }
        }
    } 
    private void Delete(object? parameter)
    {
        if (parameter is DirectoryViewModel directoryViewModel)
        {
            Directory.Delete(directoryViewModel.FullName, true);

            //FilePath = FilePath.Remove(FilePath.LastIndexOf(@"\"));
            //Name = FilePath[FilePath.LastIndexOf(@"\")..];
            Directories.Remove(directoryViewModel);
        }
        else if (parameter is FileViewModel fileViewModel)
        {
            File.Delete(fileViewModel.FullName);
            Directories.Remove(fileViewModel);
        }
    }  
    private void SwitchDirectory()
    {
        var dirInfo = new DirectoryInfo(FilePath);
        try
        {
            foreach (var dir in dirInfo.GetDirectories())
                Directories.Add(new DirectoryViewModel(dir));

            foreach (var file in dirInfo.GetFiles())
                Directories.Add(new FileViewModel(file));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UnauthorizedAccessException:
                    throw new UnauthorizedAccessException(ex.Message);
                case ArgumentNullException:
                    throw new ArgumentNullException(ex.Message);
            }
        }
    }

    private static void ExecuteFile(FileViewModel fileViewModel)
    {
        Process p = new()
        {
            StartInfo = new()
            {
                UseShellExecute = true,
                FileName = $"{fileViewModel.FullName}"
            }
        };

        try
        {
            p.Start();
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case InvalidOperationException:
                    throw new InvalidOperationException(ex.Message);
                case PlatformNotSupportedException:
                    throw new PlatformNotSupportedException(ex.Message);
            }
        }
    }
}
