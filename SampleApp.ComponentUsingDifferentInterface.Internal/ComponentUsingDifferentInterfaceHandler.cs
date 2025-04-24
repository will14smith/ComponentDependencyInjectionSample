using SampleApp.ComponentUsingDifferentInterface.Contracts;
using SampleApp.S3;

namespace SampleApp.ComponentUsingDifferentInterface.Internal;

internal class ComponentUsingDifferentInterfaceHandler(IS3Service s3) : IComponentUsingDifferentInterfaceHandler
{
    public void Handle() => Console.WriteLine($"ComponentUsingDifferentInterface called S3: {s3.GetData()}");
}