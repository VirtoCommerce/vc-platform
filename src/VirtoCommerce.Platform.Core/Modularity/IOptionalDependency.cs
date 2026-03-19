namespace VirtoCommerce.Platform.Core.Modularity;

public interface IOptionalDependency<T>
{
    bool HasValue { get; }

    public T Value { get; }
}
