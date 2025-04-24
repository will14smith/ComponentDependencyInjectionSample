using Microsoft.Extensions.DependencyInjection;
using SampleApp.ComponentUsingDifferentInterface.Contracts;
using SampleApp.ComponentUsingDifferentInterface.Internal.S3;
using SampleApp.Results.Contracts;

namespace SampleApp.ComponentUsingDifferentInterface.Internal;

public static class DependencyInjectionExtensions
{
    public static T AddComponentUsingDifferentInterface<T>(this T services) where T : IServiceCollection
    {
        // register our special S3 implementation using a *different interface*
        services.AddTransient<IS3ServiceSpecial, S3ServiceSpecial>();
        
        // this is assuming the handler class is using the base IS3Service interface
        // if the different interface is used in the constructor directly, then we wouldn't need an explicit func constructor
        services.AddTransient<IComponentUsingDifferentInterfaceHandler>(provider => new ComponentUsingDifferentInterfaceHandler(provider.GetRequiredService<IS3ServiceSpecial>()));
        
        services.AddKeyedTransient<IResultPersistence, ResultPersistence>("composite");
        
        return services;
    } 
}