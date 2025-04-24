using SampleApp.S3;

namespace SampleApp.ComponentUsingKeyed.Internal.S3;

public class S3ServiceMagic : IS3Service
{
    public string GetData() => "This is some real magic data from S3!";
}