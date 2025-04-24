using Microsoft.Extensions.DependencyInjection;
using SampleApp.ComponentUsingKeyed.Contracts;
using SampleApp.ComponentUsingKeyed.Internal.S3;
using SampleApp.Results.Contracts;
using SampleApp.S3;

namespace SampleApp.ComponentUsingKeyed.Internal;

public static class DependencyInjectionExtensions
{
    internal const string S3Key = "SampleApp.ComponentUsingKeyed.Internal.S3"; 
    
    public static T AddComponentUsingKeyed<T>(this T services) where T : IServiceCollection
    {
        // register our magic S3 implementation using a key
        services.AddKeyedTransient<IS3Service, S3ServiceMagic>(S3Key);
        
        // this is assuming the handler class is using the base IS3Service interface undecorated
        // if a [FromKeyedServices(S3Key)] attribute was applied in the constructor, then we wouldn't need an explicit func constructor
        services.AddTransient<IComponentUsingKeyedHandler>(provider => new ComponentUsingKeyedHandler(provider.GetRequiredKeyedService<IS3Service>(S3Key)));

        services.AddKeyedTransient<IResultPersistence, ResultPersistence>("composite");
        
        return services;
    } 
}