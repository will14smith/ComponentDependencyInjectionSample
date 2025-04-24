using Microsoft.Extensions.DependencyInjection;
using SampleApp;
using SampleApp.ComponentUsingCommon.Contracts;
using SampleApp.ComponentUsingCommon.Internal;
using SampleApp.ComponentUsingDifferentInterface.Contracts;
using SampleApp.ComponentUsingDifferentInterface.Internal;
using SampleApp.ComponentUsingKeyed.Contracts;
using SampleApp.ComponentUsingKeyed.Internal;
using SampleApp.Results.Contracts;
using SampleApp.Results.Internal;
using SampleApp.S3;

var services = new ServiceCollection()
    // global common components
    .AddS3Service()
    .AddResults()

    // service specific components
    .AddComponentUsingCommon()
    .AddComponentUsingDifferentInterface()
    .AddComponentUsingKeyed();

// this would be done in an architecture test
Architecture.VerifyNoDuplicateComponents(services);


var provider = services.BuildServiceProvider();

Console.WriteLine("Testing resolving components using different IS3Service implementations:");
provider.GetRequiredService<IComponentUsingCommonHandler>().Handle();
provider.GetRequiredService<IComponentUsingDifferentInterfaceHandler>().Handle();
provider.GetRequiredService<IComponentUsingKeyedHandler>().Handle();

Console.WriteLine();
Console.WriteLine("Testing resolving components using composite pattern:");
provider.GetRequiredService<IResultPersistence>().PersistResult("result1");