using SampleApp.ComponentUsingCommon.Contracts;
using SampleApp.S3;

namespace SampleApp.ComponentUsingCommon.Internal;

internal class ComponentUsingCommonHandler(IS3Service s3) : IComponentUsingCommonHandler
{
    public void Handle() => Console.WriteLine($"ComponentUsingCommon called S3: {s3.GetData()}");
}