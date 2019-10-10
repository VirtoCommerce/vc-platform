namespace VirtoCommerce.Platform.Core.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T obj);   
    }
}
