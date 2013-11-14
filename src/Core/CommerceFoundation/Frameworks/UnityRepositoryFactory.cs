using Microsoft.Practices.Unity;

namespace VirtoCommerce.Foundation.Frameworks
{
    public class UnityRepositoryFactory<T> : IRepositoryFactory<T> where T : IRepository
    {
        private readonly IUnityContainer _container;

        public UnityRepositoryFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T GetRepositoryInstance()
        {
            return _container.Resolve<T>();
        }
    }
}
