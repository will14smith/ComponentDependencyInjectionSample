using Microsoft.Extensions.DependencyInjection;
using SampleApp.ComponentUsingCommon.Contracts;
using SampleApp.Results.Contracts;
using SampleApp.S3;

namespace SampleApp.ComponentUsingCommon.Internal;

public static class DependencyInjectionExtensions
{
    public static T AddComponentUsingCommon<T>(this T services) where T : IServiceCollection
    {
        // not needed since the root component should register this common implementation
        // but nice as a reminder of what's used and if it's ever needed elsewhere
        services.AddS3Service();
        
        services.AddTransient<IComponentUsingCommonHandler, ComponentUsingCommonHandler>();
        services.AddKeyedTransient<IResultPersistence, ResultPersistence>("composite");
        
        return services;
    } 
}