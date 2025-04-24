namespace SampleApp.ComponentUsingDifferentInterface.Internal.S3;

public class S3ServiceSpecial : IS3ServiceSpecial
{
    public string GetData() => "Hello from Special S3!";
}