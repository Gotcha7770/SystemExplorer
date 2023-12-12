using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.ViewModels;

namespace SystemExplorer.Views
{
    public class Viewlocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null)
                throw new InvalidOperationException("Data was not found");

            var fullName = data.GetType().FullName 
                ?? throw new InvalidOperationException("Full name for type was not found");


            var name = fullName
                .Replace("ViewModel", "View")
                .Replace("SystemExplorer", "SystemExplorer.ViewModels");

            var type = Type.GetType(name);

            return type is null 
                ? throw new InvalidOperationException($"Type {name} was not found") 
                : (Control)Activator.CreateInstance(type)!;
        }

        public bool Match(object? data) => data is ViewModelBase;
    }
}
