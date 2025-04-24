using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SampleApp.S3;

public static class DependencyInjectionExtensions
{
    public static T AddS3Service<T>(this T services) where T : IServiceCollection
    {
        // using TryAdd to allow this to be called idempotently from multiple components
        services.TryAddTransient<IS3Service, S3Service>();
        
        return services;
    } 
}