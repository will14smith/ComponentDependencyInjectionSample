using SampleApp.Results.Contracts;

namespace SampleApp.ComponentUsingCommon.Internal;

internal class ResultPersistence : IResultPersistence
{
    public void PersistResult(string result)
    {
        Console.WriteLine("Persisting result in ComponentUsingComment: " + result);
    }
}