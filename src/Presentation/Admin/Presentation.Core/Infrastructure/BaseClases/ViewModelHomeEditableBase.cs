using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public abstract class ViewModelHomeEditableBase<T> : ViewModelHomeBase<T>, IViewModelHomeEditableBase where T : class
	{
		protected ViewModelHomeEditableBase()
		{
			CommonWizardDialogRequest = new InteractionRequest<Confirmation>();
			CommonConfirmRequest = new InteractionRequest<Confirmation>();

			ItemAddCommand = new DelegateCommand(RaiseItemAddInteractionRequest, CanItemAddExecute);
			ItemDeleteCommand = new DelegateCommand<IList>(RaiseItemDeleteInteractionRequest, CanItemDeleteExecute);
		}

		#region IViewModelHomeBase


		public DelegateCommand ItemAddCommand { get; private set; }
		public DelegateCommand<IList> ItemDeleteCommand { get; private set; }

		public InteractionRequest<Confirmation> CommonWizardDialogRequest { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#endregion

		#region Abstract members

		protected abstract bool CanItemAddExecute();
		protected abstract bool CanItemDeleteExecute(IList x);

		protected abstract void RaiseItemAddInteractionRequest();
		protected abstract void RaiseItemDeleteInteractionRequest(IList selectedItemsList);


		#endregion

		protected void ItemAdd(T item, Confirmation confirmation, IRepository repository)
		{
			CommonWizardDialogRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					OnUIThread(() => ShowLoadingAnimation = true);
					try
					{
						repository.Add(item);
						repository.UnitOfWork.Commit();
						AfterItemAddSaved(item);
					}
					catch (Exception ex)
					{
						ShowErrorDialog(ex, string.Format("An error occurred when trying to save".Localize(null, LocalizationScope.DefaultCategory)));
					}
					finally
					{
						OnUIThread(() => ShowLoadingAnimation = false);
					}
				}
			});
		}

		/// <summary>
		/// Added if Create...ViewModel has ISupportWizardSave
		/// </summary>
		/// <param name="confirmation"></param>
		/// <param name="parameter">additional parameter to pass to AfterItemAddSaved</param>
		protected void ItemAdd(Confirmation confirmation, object parameter = null)
		{
			CommonWizardDialogRequest.Raise(confirmation, async (x) =>
			{
				if (x.Confirmed)
				{
					OnUIThread(() => ShowLoadingAnimation = true);
					try
					{
						var vm = (x.Content as ISupportWizardSave);
						if (vm != null)
						{
							await Task.Run(() => vm.PrepareAndSave()); //todo check show animation for all task process
							AfterItemAddSaved(parameter);
						}
					}
					catch (Exception ex)
					{
						ShowErrorDialog(ex, string.Format("An error occurred when trying to save".Localize(null, LocalizationScope.DefaultCategory)));
					}
					finally
					{
						OnUIThread(() => ShowLoadingAnimation = false);
					}
				}
			});
		}

		protected virtual void AfterItemAddSaved(object item)
		{
			ListItemsSource.Refresh();
		}

		protected void ItemDelete(ConditionalConfirmation confirmation, IList selectedItemsList, IRepository repository)
		{
			var selectionCount = selectedItemsList.Count;
			if (selectionCount > 0)
			{
				CommonConfirmRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							OnUIThread(() => ShowLoadingAnimation = true);
							var anyOperationSucceded = false;
							try
							{
								foreach (var item in selectedItemsList)
								{
									if (item is IViewModelDetailBase)
									{
										(item as IViewModelDetailBase).Delete();
									}
									else if (item is T)
									{
										repository.Attach(item);
										repository.Remove(item);
										repository.UnitOfWork.Commit();
									}
									anyOperationSucceded = true;
								}
							}
							catch (Exception ex)
							{
								ShowErrorDialog(ex, string.Format("An error occurred when trying to remove"));
							}
							finally
							{
								OnUIThread(() => ShowLoadingAnimation = false);
								if (anyOperationSucceded)
									ListItemsSource.Refresh();
							}
						}
					});
			}
		}

		protected bool ItemDelete(IList<IViewModelDetailBase> selectedItemsList)
		{
			var anyOperationSucceded = true;
			var selectionCount = selectedItemsList.Count;
			if (selectionCount > 0)
			{
				string joinedNames = null;
				var names = selectedItemsList.Take(4).Select(x => ((ViewModelBase)x).DisplayName);

				joinedNames = string.Join(",\n", names);
				if (selectionCount > 4)
					joinedNames += "...";

				var confirmation = new ConditionalConfirmation()
				{
					Content = joinedNames,
                    Title = "Are you sure you want to delete?".Localize()
				};

				CommonConfirmRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							OnUIThread(() => ShowLoadingAnimation = true);
							try
							{
								foreach (var item in selectedItemsList.Where(y => y != null))
								{
									item.Delete();									
								}
							}
							catch (Exception ex)
							{
								ShowErrorDialog(ex, string.Format("An error occurred when trying to remove\n{0}", joinedNames));
							}
							finally
							{
								OnUIThread(() => ShowLoadingAnimation = false);
								if (anyOperationSucceded)
									ListItemsSource.Refresh();
							}
						}
					});
			}
			return anyOperationSucceded;
		}
		
		protected void ItemDuplicate(IList<IViewModelDetailBase> selectedItemsList)
		{
			var selectionCount = selectedItemsList.Count;
			if (selectionCount > 0)
			{
				string joinedNames = null;
				var names = selectedItemsList.Take(4).Select(x => ((ViewModelBase)x).DisplayName);

				joinedNames = string.Join(",\n", names);
				if (selectionCount > 4)
					joinedNames += "...";

				var confirmation = new ConditionalConfirmation()
				{
					Content = joinedNames,
                    Title = "Are you sure you want to create duplicate?".Localize()
				};

				CommonConfirmRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							OnUIThread(() => ShowLoadingAnimation = true);
							var anyOperationSucceded = false;
							try
							{
								foreach (var item in selectedItemsList.Where(y => y != null))
								{
									item.Duplicate();
									anyOperationSucceded = true;
								}
							}
							catch (Exception ex)
							{
								ShowErrorDialog(ex, string.Format("An error occurred when trying to create duplicate\n{0}", joinedNames));
							}
							finally
							{
								OnUIThread(() => ShowLoadingAnimation = false);
								if (anyOperationSucceded)
									ListItemsSource.Refresh();
							}
						}
					});
			}
		}

		protected virtual IEnumerable<GestureActionName> GetGestureNames()
		{
			yield return GestureActionName.add;
			yield return GestureActionName.delete;
		}

		protected override IEnumerable<ActionBinding> GetActionBindings()
		{
			yield return new ActionBinding { Command = ItemAddCommand, Name = GestureActionName.add };
			// yield return new ActionBinding { Command = ItemDeleteCommand, Name = GestureActionName.delete };
			yield return new ActionBinding { Command = ItemDeleteCommand, Name = GestureActionName.delete };
		}
	}
}
