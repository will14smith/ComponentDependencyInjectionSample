using SampleApp.Results.Contracts;

namespace SampleApp.ComponentUsingDifferentInterface.Internal;

internal class ResultPersistence : IResultPersistence
{
    public void PersistResult(string result)
    {
        Console.WriteLine("Persisting result in ComponentUsingDifferentInterface: " + result);
    }
}