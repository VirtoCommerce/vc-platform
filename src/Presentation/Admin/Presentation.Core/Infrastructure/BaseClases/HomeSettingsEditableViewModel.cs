using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{

	/// <summary>
	/// Expands base class with Add/Edit/Remove commands.
	/// Implement virtual ItemAdd and ItemDelete methods 
	/// </summary>
	/// <typeparam name="T">Class of HomeView list item</typeparam>
	public abstract class HomeSettingsEditableViewModel<T> : HomeSettingsViewModel<T>, IHomeSettingsEditableViewModel<T> where T : class
	{
		protected readonly IViewModelsFactory<IViewModel> WizardVmFactory;
		protected readonly IViewModelsFactory<IViewModel> EditVmFactory;

		protected HomeSettingsEditableViewModel(IFactory entityFactory, IViewModelsFactory<IViewModel> wizardVMFactory, IViewModelsFactory<IViewModel> editVMFactory)
			: base(entityFactory)
		{
			WizardVmFactory = wizardVMFactory;
			EditVmFactory = editVMFactory;

			ItemAddCommand = new DelegateCommand(RaiseItemAddInteractionRequest, CanRaiseItemAddExecute);
			ItemEditCommand = new DelegateCommand<T>(RaiseItemEditInteractionRequest, CanRaiseItemEditExecute);
			ItemDeleteCommand = new DelegateCommand<T>(RaiseItemDeleteInteractionRequest, CanRaiseItemDeleteExecute);

			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			CommonWizardDialogRequest = new InteractionRequest<Confirmation>();
		}

		#region IHomeSettingsEditableViewModel

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		public InteractionRequest<Confirmation> CommonWizardDialogRequest { get; private set; }

		public DelegateCommand ItemAddCommand { get; private set; }
		public DelegateCommand<T> ItemEditCommand { get; private set; }
		public DelegateCommand<T> ItemDeleteCommand { get; private set; }

		#endregion

		#region Abstract Methods
		/// <summary>
		/// Implementation of append new Item (ItemAdd helps you)
		/// </summary>
		protected abstract void RaiseItemAddInteractionRequest();
		/// <summary>
		/// Implementation of modify Item
		/// </summary>
		/// <param name="item"></param>
		protected abstract void RaiseItemEditInteractionRequest(T item);
		/// <summary>
		/// Implementation of remove Item (ItemDelete helps you)
		/// </summary>
		protected abstract void RaiseItemDeleteInteractionRequest(T item);

		#endregion

		#region HomeSettingsViewModel

		/// <summary>
		/// Refresh Command state
		/// </summary>
		public override void RaiseCanExecuteChanged()
		{
			ItemAddCommand.RaiseCanExecuteChanged();
			ItemEditCommand.RaiseCanExecuteChanged();
			ItemDeleteCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region Protected Virtual Methods

		/// <summary>
		/// CanExecute for ItemAddCommand
		/// </summary>
		/// <returns></returns>
		protected virtual bool CanRaiseItemAddExecute()
		{
			return true;
		}

		/// <summary>
		/// CanExecute for ItemEditCommand
		/// </summary>
		protected virtual bool CanRaiseItemEditExecute(T item)
		{
			return item != null;
		}

		/// <summary>
		/// CanExecute for ItemDeleteCommand
		/// </summary>
		protected virtual bool CanRaiseItemDeleteExecute(T item)
		{
			return item != null;
		}

		// This method will delete soon
		/// <summary>
		/// Base method to append Item. 
		/// Rises given confirmation dialog and store given Item to DB after edit.
		/// Appends edited Item to HomeView list and set cursor to it.
		/// Rises PrepareAndSave() if confirmation dialog inherit ISupportWizardSave.
		/// (It needs if Wizard views has a property that is not binding to InnerItem)
		/// Works with a repository in not UI thread. 
		/// Shows Load Animation 
		/// </summary>
		/// <param name="item">Item for edit in confirmation dialog and store it</param>
		/// <param name="confirmation">confirmation dialog for editing given Item</param>
		/// <param name="repository">Repository</param>
		protected virtual void ItemAdd(T item, Confirmation confirmation, IRepository repository)
		{
			CommonWizardDialogRequest.Raise(confirmation, async (x) =>
			{
				if (x.Confirmed)
				{
					ShowLoadingAnimation = true;

					try
					{
						bool success = false;
						await Task.Run(() =>
							{
								var vm = (x.Content as ISupportWizardSave);
								if (vm != null)
								{
									success = vm.PrepareAndSave();
								}
								else
								{
									repository.Add(item);
									repository.UnitOfWork.Commit();
									success = true;
								}
							});

						if (success)
						{
							AfterItemAddSaved(item);
						}
					}
					finally
					{
						ShowLoadingAnimation = false;
					}
				}
			});
		}

		protected virtual void AfterItemAddSaved(T item)
		{
			Items.Add(item);
			var view = CollectionViewSource.GetDefaultView(Items);
			view.MoveCurrentTo(item);
		}

		protected virtual void ItemAdd(Confirmation confirmation, IRepository repository, object itemFromRep)
		{
			CommonWizardDialogRequest.Raise(confirmation, async (x) =>
				{
					if (x.Confirmed)
					{
						OnUIThread(() => { ShowLoadingAnimation = true; });

						try
						{
							await Task.Run(() =>
								{
									var vm = (x.Content as ISupportWizardSave);
									if (vm != null)
									{
										vm.PrepareAndSave();
									}
									else
									{
										repository.Add(itemFromRep);
										repository.UnitOfWork.Commit();
									}
								});
							OnUIThread(() =>
								{
									var item = CreateItem(itemFromRep);
									AfterItemAddSaved(item);
								});
						}
						finally
						{
							OnUIThread(() => { ShowLoadingAnimation = false; });
						}

					}
				});
		}

		// This method will delete soon
		protected virtual void ItemDelete(T item, ConditionalConfirmation confirmation, IRepository repository)
		{
			ItemDelete(item, confirmation, repository, item);
		}

		/// <summary>
		/// Base method to delete Item. 
		/// Rises given confirmation dialog and remove given Item from DB.
		/// Removes given Item from HomeView list.
		/// Works with a repository in not UI thread. 
		/// Shows Load Animation 
		/// </summary>
		/// <param name="item">Item for remove</param>
		/// <param name="confirmation">confirmation dialog for removing given Item</param>
		/// <param name="repository">Repository</param>
		/// <param name="itemFromRep">Item for remove from repository</param>
		protected virtual void ItemDelete(T item, ConditionalConfirmation confirmation, IRepository repository, object itemFromRep)
		{
			if (item == null || itemFromRep == null || confirmation == null || repository == null)
				return;

			if (string.IsNullOrEmpty(confirmation.Title))
			{
				confirmation.Title = "Delete confirmation".Localize();
			}
			CommonConfirmRequest.Raise(confirmation, async (x) =>
			{
				if (x.Confirmed)
				{
					ShowLoadingAnimation = true;
					try
					{
						await Task.Run(() =>
						{
							if (!repository.IsAttachedTo(itemFromRep))
								repository.Attach(itemFromRep);
							repository.Remove(itemFromRep);
							repository.UnitOfWork.Commit();
						});

						Items.Remove(item);
					}
					finally
					{
						ShowLoadingAnimation = false;
					}
				}
			});
		}

		protected virtual void ItemDelete(T item, string confirmationContent, IRepository repository, object itemFromRep)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = confirmationContent
			};

			ItemDelete(item, confirmation, repository, itemFromRep);
		}

		//todo mark as abstract
		protected virtual object GetItemById(string itemId)
		{
			return null;
		}

		#endregion

		//todo mark as abstract
		protected virtual T CreateItem(object itemFromRep)
		{
			return null;
		}
	}
}
