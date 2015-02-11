namespace VirtoCommerce.PagesModule.Web
{
    #region

    using Microsoft.Practices.Unity;

    using VirtoCommerce.Framework.Web.Modularity;

    #endregion

    public class Module : IModule //, IDatabaseModule
    {
        #region Fields

        private readonly IUnityContainer _container;

        #endregion

        #region Constructors and Destructors

        public Module(IUnityContainer container)
        {
            this._container = container;
        }

        #endregion

        #region Public Methods and Operators

        public void Initialize()
        {
        }

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        #endregion
    }
}