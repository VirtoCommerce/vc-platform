namespace VirtoCommerce.Foundation.Frameworks
{
    public interface IRepositoryFactory<T> where T : IRepository
    {
        T GetRepositoryInstance();
    }
}
