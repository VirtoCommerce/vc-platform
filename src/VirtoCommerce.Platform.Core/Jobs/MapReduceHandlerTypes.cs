using System;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Resolves the closed <see cref="IMapJobHandler{TItem, TResult}"/> / <see cref="IReduceJobHandler{TState, TResult}"/>
/// interfaces a map/reduce handler pair implements, validating each implements exactly one and that the map result
/// type matches the reduce input result type. Shared by handler-explicit enqueue (<c>Enqueue&lt;TMap, TReduce&gt;</c>)
/// and registration (<c>AddMapReduceJob&lt;TMap, TReduce&gt;</c>).
/// </summary>
public static class MapReduceHandlerTypes
{
    /// <summary>The closed map and reduce handler interfaces; map TResult is validated to equal reduce TResult.</summary>
    public static (Type MapInterface, Type ReduceInterface) ResolveInterfaces(Type mapHandlerType, Type reduceHandlerType)
    {
        var mapInterface = SingleClosedInterface(mapHandlerType, typeof(IMapJobHandler<,>), "IMapJobHandler");
        var reduceInterface = SingleClosedInterface(reduceHandlerType, typeof(IReduceJobHandler<,>), "IReduceJobHandler");

        // The map produces what the reduce consumes — the TResult of both must match.
        if (mapInterface.GetGenericArguments()[1] != reduceInterface.GetGenericArguments()[1])
        {
            throw new ArgumentException(
                $"{mapHandlerType.Name} and {reduceHandlerType.Name} disagree on the map result type.");
        }

        return (mapInterface, reduceInterface);
    }

    /// <summary>The (itemType, resultType, stateType) a map/reduce handler pair operates on.</summary>
    public static (Type ItemType, Type ResultType, Type StateType) ResolveTypes(Type mapHandlerType, Type reduceHandlerType)
    {
        var (mapInterface, reduceInterface) = ResolveInterfaces(mapHandlerType, reduceHandlerType);
        var mapArgs = mapInterface.GetGenericArguments();
        return (mapArgs[0], mapArgs[1], reduceInterface.GetGenericArguments()[0]);
    }

    // GetInterfaces returns the CLOSED generics (IMapJobHandler<SomeItem, SomeResult>) — pick the single match.
    public static Type SingleClosedInterface(Type handlerType, Type openGeneric, string friendlyName)
    {
        var matches = handlerType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGeneric)
            .ToArray();

        return matches.Length switch
        {
            1 => matches[0],
            0 => throw new ArgumentException($"{handlerType.Name} must implement {friendlyName}<...>."),
            _ => throw new ArgumentException(
                $"{handlerType.Name} implements {friendlyName}<...> for multiple type arguments; use the explicit AddMapReduceJob overload."),
        };
    }
}
