using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Results.Contracts;

namespace SampleApp;

public static class Architecture
{
    // this would probably be better as an attribute on the interface or something 
    private static readonly HashSet<Type> AllowedCompositeTypes = [
        typeof(IResultPersistence)
    ];

    public static void VerifyNoDuplicateComponents(ServiceCollection services)
    {
        var duplicates = services
            .GroupBy(x => (x.ServiceType, x.ServiceKey))
            .Where(g => g.Count() > 1 && !IsAllowedCompositeType(g.Key.ServiceType, g.Key.ServiceKey))
            .Select(g => new { g.Key.ServiceType, g.Key.ServiceKey, Implementations = g.ToList() })
            .ToList();

        if (duplicates.Count == 0)
        {
            return;
        }

        var message = new StringBuilder();
        message.AppendLine("Duplicate components found:");
        
        foreach (var duplicate in duplicates)
        {
            message.Append($"{duplicate.ServiceType}{(duplicate.ServiceKey is not null ? $" (keyed: {duplicate.ServiceKey})" : "")}: ");
            message.AppendLine(string.Join(", ", duplicate.Implementations.Select(i => i.ImplementationType)));
        }
        
        throw new Exception(message.ToString());
    }

    private static bool IsAllowedCompositeType(Type keyServiceType, object? serviceKey)
    { 
        // because a limitation of Microsoft DI, we can't use the exact same composite pattern as AutoFac
        // this is because last registered wins, so if we don't register things in the correct order, we can end up with the wrong implementation
        // to get around this, we have the composite implementation registered without a key and the sub-implementations registered with a well-known key
        // ideally there would be some nice helper methods around to do this

        // check it's even a potential composite type
        if(!AllowedCompositeTypes.Contains(keyServiceType))
        {
            return false;
        }

        return serviceKey as string == "composite";
    }
}