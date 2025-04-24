using SampleApp.Results.Contracts;

namespace SampleApp.ComponentUsingKeyed.Internal;

internal class ResultPersistence : IResultPersistence
{
    public void PersistResult(string result)
    {
        Console.WriteLine("Persisting result in ComponentUsingKeyed: " + result);
    }
}