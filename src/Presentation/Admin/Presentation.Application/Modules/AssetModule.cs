using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.ManagementClient.Asset.Dialogs.ViewModel;
using VirtoCommerce.ManagementClient.Asset.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Asset.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Asset.Factories;
using VirtoCommerce.Foundation.Assets;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Asset
{
    public class AssetModule : IModule
    {
        private readonly IUnityContainer _container;

        public AssetModule(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
        }

        #endregion

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IAssetEntityFactory, AssetEntityFactoryExt>(new ContainerControlledLifetimeManager());

#if USE_MOCK
            _container.RegisterType<IAssetRepository, VirtoCommerce.ManagementClient.Asset.Services.FileSystemRepository>();
            _container.RegisterType<IBlobStorageProvider, MockBlobStorageProvider>();
#else
			_container.RegisterService<IAssetService>(
				_container.Resolve<IServiceConnectionFactory>().GetConnectionString(AssetConfiguration.Instance.Connection.ServiceUri),
				AssetConfiguration.Instance.Connection.WSEndPointName
				);
#endif

            // pick single asset
            _container.RegisterType<IPickAssetViewModel, PickAssetViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IInputNameDialogViewModel, InputNameDialogViewModel>();

            var resources = new ResourceDictionary
	            {
		            Source =
			            new Uri("/VirtoCommerce.ManagementClient.Asset;component/AssetModuleDictionary.xaml", UriKind.Relative)
	            };
	        Application.Current.Resources.MergedDictionaries.Add(resources);
        }
    }
}
