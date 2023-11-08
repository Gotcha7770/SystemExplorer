using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemExplorer;

public class DependencyInjection : Application
{
    // https://github.com/AvaloniaUI/Avalonia/issues/5241
    public static Window? FocusedWindow =>
        Current?.DataTemplates
        .Select(t => t.Build(null))
        .Cast<Window>()
        .FirstOrDefault(w => w.IsFocused);

    public static Window? ActivedWindow =>
        Current?.DataTemplates
        .Select(t => t.Build(null))
        .Cast<Window>()
        .FirstOrDefault(w => w.IsActive);

    public static Window? GetWindowByTitle(string title) =>
        Current?.DataTemplates
        .Select(t => t.Build(null))
        .Cast<Window>()
        .FirstOrDefault(
            w => !string.IsNullOrEmpty(w.Title) &&
            w.Title.Equals(title));


    private Window CreateWindowForModel(object model)
    {
        var models = Current?.DataTemplates
            .Where(template => template.Match(model))
            ?? throw new NullReferenceException(nameof(Current));
        
        foreach (var template in models)
        {
            var control = template.Build(model);
            if (control is Window w)
                return w;
            return new Window { Content = control };
        }

        throw new KeyNotFoundException("Unable to find view for model: " + model);
    }

    public void ShowWindow(object model) => CreateWindowForModel(model).Show();

}
