using SampleApp.ComponentUsingKeyed.Contracts;
using SampleApp.S3;

namespace SampleApp.ComponentUsingKeyed.Internal;

internal class ComponentUsingKeyedHandler(IS3Service s3) : IComponentUsingKeyedHandler
{
    public void Handle() => Console.WriteLine($"ComponentUsingKeyedHandler called S3: {s3.GetData()}");
}