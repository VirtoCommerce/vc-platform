using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Reviews.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Reviews.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Reviews
{
    public class ReviewsModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly NavigationManager _navigationManager;

		public ReviewsModule(IUnityContainer container, IRegionManager regionManager, NavigationManager navigationManager)
        {
            _container = container;
            _regionManager = regionManager; 
            _navigationManager = navigationManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
        }

        #endregion

        protected void RegisterViewsAndServices()
        {
			_container.RegisterType<IReviewEntityFactory, ReviewEntityFactory>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IReviewRepository, DSReviewClient>();

			_container.RegisterType<IReviewsHomeViewModel, ReviewsHomeViewModel>();
			_container.RegisterType<IReviewEditViewModel, ReviewEditViewModel>();
			
			var resources = new ResourceDictionary
				{
					Source = new Uri("/VirtoCommerce.ManagementClient.Reviews;component/ReviewsModuleDictionary.xaml", UriKind.Relative)
				};
	        Application.Current.Resources.MergedDictionaries.Add(resources);
        }

    }
}
