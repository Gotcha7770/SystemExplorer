using Splat;
using System;

namespace SystemExplorer.DependencyInjection;

public static class ReadonlyDependencyResolverExtensions
{
    public static TService GetRequiredService<TService>(
        this IReadonlyDependencyResolver resolver)
    {
        return resolver.GetService<TService>() 
            ?? throw new InvalidOperationException(
                $"Failed to resolve object of type {typeof(TService)}");
    }

    public static object GetRequiredService(
        this IReadonlyDependencyResolver resolver, Type type)
    {
        var service = resolver.GetService(type);
        return service is null 
            ? throw new InvalidOperationException($"Failed to resolve object of type {type}") 
            : service;
    }
}
