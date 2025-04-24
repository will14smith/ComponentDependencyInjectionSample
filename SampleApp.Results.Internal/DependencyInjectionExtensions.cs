using Microsoft.Extensions.DependencyInjection;
using SampleApp.Results.Contracts;

namespace SampleApp.Results.Internal;

public static class DependencyInjectionExtensions
{
    public static T AddResults<T>(this T services) where T : IServiceCollection
    {
        services.AddTransient<IResultPersistence, CompositeResultPersistence>();
        return services;
    } 
}