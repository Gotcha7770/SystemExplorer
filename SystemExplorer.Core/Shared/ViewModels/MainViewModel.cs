using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Xml.XPath;
using SystemExplorer.Core.Shared.BaseModels.Abstract;
using SystemExplorer.Core.Shared.Commands;
using SystemExplorer.Core.Shared.Entities;
using SystemExplorer.Core.Shared.Entities.Abstract;

namespace SystemExplorer.Core.Shared.BaseModels;

public class MainViewModel : BaseViewModel
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

    #region Ctor
    public MainViewModel()
    {
        FilePath = Environment.SystemDirectory;
        Name = "This computer";

        foreach (var dir in Directory.GetLogicalDrives()) 
            Directories.Add(new DirectoryViewModel(new DirectoryInfo(dir)));
    }
    #endregion

    #region Commands
    public ICommand OpenCommand => 
        new DelegateCommand(Open);

    public ICommand OpenTargetDirectoryCommand => 
        new DelegateCommand(OpenTargetDirectory);
    #endregion

    #region Private Command Methods
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
    #endregion

    #region Private Methods
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
    #endregion
}
