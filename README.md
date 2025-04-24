# Strategies for managing DI across multiple components

This is a set of patterns for managing DI when we have multiple components using common interfaces.
Generally we'll want components to be using the default implementation of the interface to reduce cognitive load, but there will be cases where we want to override that.
We considered using multiple sub-DI containers to manage this, but that would add a lot of complexity to solve a problem that we don't believe is actually that common in practice.

To ensure that we don't end up with multiple implementations of the same interface, we can use an architecture test as described in [Architecture.cs](./SampleApp/Architecture.cs).

## Example 1: Single implementation interfaces (e.g. `IS3Service`)

There are 3 components that all use different implementations of `IS3Service`:

### [`SampleApp.ComponentUsingCommon`](./SampleApp.ComponentUsingCommon.Internal/DependencyInjectionExtensions.cs)
This component is the baseline. It is using the common implementation of `IS3Service` registered by the module responsible for S3.

The S3 module registers the implementation using `TryAddTransient` so that it can be registered idempotently. **TODO: it should probably verify another implementation isn't registered instead.**

```csharp

### [`SampleApp.ComponentUsingDifferentInterface`](./SampleApp.ComponentUsingDifferentInterface.Internal/DependencyInjectionExtensions.cs)
This component is using a sub-classed interface of `IS3Service` and the custom implementation of the S3 interface is registered against that.

```csharp
internal interface IS3ServiceSpecial : IS3Service;
public class S3ServiceSpecial : IS3ServiceSpecial
{
    // ...
}
```

Resolution is either handled by the constructor requesting the sub-classed interface directly, or by setting up the DI injection to resolve the sub-classed interface.

```csharp
internal class ComponentHandler(IS3ServiceSpecial s3Service) { }
// or
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient(provider => new ComponentHandler(provider.GetRequiredService<IS3ServiceSpecial>()));
}
```

### [`SampleApp.ComponentUsingKeyed`](./SampleApp.ComponentUsingKeyed.Internal/DependencyInjectionExtensions.cs)
This component is using a keyed registrations of `IS3Service`.
Resolution is handled by the constructor requesting the keyed interface directly, or by setting up the DI injection to resolve the keyed interface.

```csharp
internal class ComponentHandler([FromKeyedServices(Constants.S3Key)] IS3Service s3Service) { }
// or
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient(provider => new ComponentHandler(provider.GetRequiredKeyedService<IS3Service>(Constants.S3Key)));
}
```

## Example 2: Composite interfaces (e.g. `IResultPersistence`)

In this example we have a component that uses a composite interface that is implemented by multiple components.
This is different to the previous strategies because the aim *is* to have multiple implementations. 
There are various limitations of Microsoft.Extensions.DependencyInjection that make this a bit tricky (compared to Autofac for example).

To work around those limitation we can use keyed services again.
Each component registers its implementation of the composite interface with a well-known key (ideally this would be wrapped in some helper methods).
Then the composite owner registers an implementation of the interface that uses the keyed services to resolve and call the implementations.

The `IsAllowedCompositeType` method in the architecture test is used to ensure that we stick to this convention.

Registering the composite interface with a well-known key:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddKeyedTransient<IResultPersistence, ResultPersistence>("composite");
}
```

Registering the composite owner:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IResultPersistence, CompositeResultPersistence>();
}

internal class CompositeResultPersistence([FromKeyedServices("composite")] IEnumerable<IResultPersistence> composites) : IResultPersistence
{
    public void PersistResult(string result)
    {
        foreach (var resultPersistence in composites)
        {
            resultPersistence.PersistResult(result);
        }
    }
}
```