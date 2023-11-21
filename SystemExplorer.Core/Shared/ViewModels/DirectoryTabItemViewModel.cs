using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using SystemExplorer.Core.Shared.BaseModels.Abstract;
using SystemExplorer.Core.Shared.Commands;
using SystemExplorer.Core.Shared.Entities.Abstract;
using SystemExplorer.Core.Shared.Entities;

namespace SystemExplorer.Core.Shared.ViewModels;

public class DirectoryTabItemViewModel : BaseViewModel
{
    #region Private Variables
    private string filePath = string.Empty;
    private string name = string.Empty;
    private ObservableCollection<FileEntityViewModel> directories = new();
    private FileEntityViewModel selectedFile;
    #endregion

    #region Public Properties
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

    public ObservableCollection<FileEntityViewModel> Directories
    {
        get => directories;
        set
        {
            Set(ref directories, value);
            OnPropertyChanged();
        }
    }

    public FileEntityViewModel SelectedFile
    {
        get => selectedFile;
        set
        {
            Set(ref selectedFile, value);
            OnPropertyChanged();
        }
    }
    #endregion

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

    public ICommand UpdateCommand =>
        new DelegateCommand(Update);

    public ICommand DeleteCommand =>
        new DelegateCommand(Delete);

    public ICommand CloseCommand =>
        new DelegateCommand(Close);

    private void Close(object obj) =>
        Closed?.Invoke(this, EventArgs.Empty);
    #endregion

    public event EventHandler? Closed;

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

    private void Update(object obj)
    {
        throw new NotImplementedException();
    }
    private void Delete(object obj)
    {
        throw new NotImplementedException();
    }

    private void ExecuteFile(FileViewModel fileViewModel)
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
}
