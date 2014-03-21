using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Controls.ViewModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class CatalogEntityViewModelBase : VirtualFolderTreeItemViewModel
	{
		#region Dependences

		internal readonly IAuthenticationContext AuthContext;

		#endregion

		protected readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;

		protected CatalogEntityViewModelBase(IRepositoryFactory<ICatalogRepository> repositoryFactory, IAuthenticationContext authContext)
			: base(null, null)
		{
			_repositoryFactory = repositoryFactory;
			AuthContext = authContext;
		}

		public NavigationItem NavigationData { get; protected set; }
		public DelegateCommand OpenItemCommand { get; protected set; }
		public DelegateCommand ExportItemCommand { get; protected set; }

		public virtual void Delete(ICatalogRepository repository, InteractionRequest<Confirmation> CommonConfirmRequest, InteractionRequest<Notification> errorNotifyRequest, Action onSuccess)
		{
			throw new NotImplementedException();
		}

		#region VirtualFolderTreeItemViewModel methods
		//Prevent creating VirtualFolderTreeItemViewModel models
		protected override IViewModel CreateChildrenModel(object children)
		{
			return children as IViewModel;
		}
		#endregion

		#region private members

		#endregion
	}
}
