using Microsoft.Extensions.DependencyInjection;
using SampleApp.Results.Contracts;

namespace SampleApp.Results.Internal;

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