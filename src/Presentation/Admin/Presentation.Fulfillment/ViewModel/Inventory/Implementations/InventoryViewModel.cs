using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Implementations
{
	public class InventoryViewModel : ViewModelDetailBase<Foundation.Inventories.Model.Inventory>, IInventoryViewModel
	{
		public class InventoryStatusDisplay
		{
			public int Id
			{
				get;
				set;
			}

			public string Name
			{
				get;
				set;
			}
		}

		public List<InventoryStatusDisplay> AvailableStatuses { get; set; }

		#region Dependencies
		private readonly IEditQuantityViewModel _editQuantityVm;
		private readonly IRepositoryFactory<IInventoryRepository> _repositoryFactory;
		private readonly INavigationManager _navManager;
		#endregion

		public InventoryViewModel(IRepositoryFactory<IInventoryRepository> repositoryFactory,
			INavigationManager navManager,
			IEditQuantityViewModel editQuantityVm,
			IInventoryEntityFactory entityFactory,
			Foundation.Inventories.Model.Inventory item)
			: base(entityFactory, item)
		{
			_repositoryFactory = repositoryFactory;
			_editQuantityVm = editQuantityVm;
			_navManager = navManager;
			ViewTitle = new ViewTitleBase
				{
                    Title = "Inventory",
					SubTitle = (item != null && !string.IsNullOrEmpty(item.Sku)) ? item.Sku.ToUpper(CultureInfo.InvariantCulture) : ""
				};

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			InStockQuantityEditCommand = new DelegateCommand(RaiseEditQuantityInteractionRequest);

			var statuses = Enum.GetValues(typeof(InventoryStatus)).Cast<InventoryStatus>();
			AvailableStatuses = new List<InventoryStatusDisplay>();
			foreach (var s in statuses)
			{
				AvailableStatuses.Add(new InventoryStatusDisplay { Id = (int)s, Name = s.ToString() });
			}
		}

		#region Commands
		public DelegateCommand InStockQuantityEditCommand
		{
			get;
			protected set;
		}
		#endregion

		#region ViewModelBase overrides

		public bool IsBackorderError
		{
			get
			{
				return InnerItem.AllowBackorder && InnerItem.BackorderAvailabilityDate == null;
			}
		}

		public bool IsPreorderError
		{
			get
			{
				var value = InnerItem.AllowPreorder && InnerItem.PreorderAvailabilityDate == null;
				return value;
			}
		}

		protected override bool IsValidForSave()
		{
			var value = true;
			if (IsBackorderError)
			{
				value = false;
			}
			else if (IsPreorderError)
			{
				value = false;
			}
			OnSpecifiedPropertyChanged("IsBackorderError");
			OnSpecifiedPropertyChanged("IsPreorderError");
			return value;
		}

		public override sealed string DisplayName
		{
			get
			{
				return string.IsNullOrEmpty(OriginalItem.Sku) ? "" : OriginalItem.Sku;
			}
		}

		public override sealed string IconSource
		{
			get
			{
				return "Icon_ReceiveInventory";
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
						(SolidColorBrush)Application.Current.TryFindResource("InventoryDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.InventoryId),
														NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region privates

		private void RaiseEditQuantityInteractionRequest()
		{
			var confirmation = new ConditionalConfirmation { Title = "Edit in stock quantity".Localize(), Content = _editQuantityVm };

			CommonConfirmRequest.Raise(confirmation, x =>
			{
				if (x.Confirmed)
				{
					if (_editQuantityVm.SelectedAction == "Add")
						InnerItem.InStockQuantity += _editQuantityVm.NewQuantity;
					else
						InnerItem.InStockQuantity -= _editQuantityVm.NewQuantity;
				}
			});
		}

		#endregion

		#region ViewModelDetailBase

		public override string ExceptionContextIdentity { get { return string.Format("Inventory ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}



		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Inventory item '{0}'?".Localize(), InnerItem.Sku),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = ((IInventoryRepository)Repository).Inventories.Where(comment => comment.InventoryId == InnerItem.InventoryId).SingleOrDefault();
			OnUIThread(() => InnerItem = item);

		}

		protected override void AfterSaveChangesUI()
		{
			OriginalItem.InjectFrom<CloneInjection>(InnerItem);
		}

		#endregion
	}
}
