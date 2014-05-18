using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	/// <summary>
	/// Presents base functionality for detail ViewModel. (Load, Save, Cancel)
	/// Implements IOpenTracking, IClosable, ISupportAcceptChanges, ISupportDelayInitialization interfaces
	/// Contains OriginalItem and InnerItem
	/// </summary>
	/// <typeparam name="T">Class of edited Item</typeparam>
	public abstract class ViewModelDetailBase<T> : ViewModelBase, IViewModelDetailBase, IOpenTracking, IClosable, ISupportAcceptChanges, ISupportDelayInitialization where T : class
	{
		#region Dependencies

		protected readonly IFactory EntityFactory;
		protected IRepository Repository;


		#endregion

		#region Constructor

		protected ViewModelDetailBase(IFactory entityFactory, T item)
		{
			EntityFactory = entityFactory;
			OriginalItem = item;
			InitCommands();
		}

		#endregion

		#region Properties

		/// <summary>
		/// OriginalItem matches items from Repository
		/// </summary>
		private T _originalItem;
		public T OriginalItem
		{
			get
			{
				return _originalItem;
			}
			set
			{
				_originalItem = value;
				OnPropertyChanged();

			}
		}

		/// <summary>
		/// Item for edit. View has to binding to the property
		/// </summary>
		private T _innerItem;
		public T InnerItem
		{
			get { return _innerItem ?? (_innerItem = _originalItem); }
			set
			{
				_innerItem = value;
				OnPropertyChanged();

			}
		}

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#endregion

		#region ISupportAcceptChanges Members

		public InteractionRequest<Confirmation> CancelConfirmRequest { get; private set; }
		public DelegateCommand<object> CancelCommand { get; private set; }
		public DelegateCommand<object> SaveChangesCommand { get; private set; }

		private bool _isModified;
		public bool IsModified
		{
			get
			{
				return _isModified;
			}

			protected set
			{
				if (_isModified != value)
				{
					_isModified = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region IOpenTracking Members

		private bool _isActive;
		public bool IsActive
		{
			get { return _isActive; }
			set { _isActive = value; OnPropertyChanged(); }
		}

		public DelegateCommand OpenItemCommand { get; set; }

		#endregion

		#region IClosable Members

		public event EventHandler CloseViewRequestedEvent;
		public abstract NavigationItem NavigationData { get; }

		protected void OnCloseViewRequestedEvent(EventArgs args)
		{
			_mustReinitialize = true;
			CloseAllSubscription();
			if (Repository != null)
				Repository.Dispose();
			OnClose();
			InnerItem = null;

			EventHandler handler = CloseViewRequestedEvent;
			if (handler != null)
				handler(this, args);
		}

		#endregion

		#region ISupportDelayInitialization Members

		/// <summary>
		/// Loads full info to OriginalItem (LoadInnerItem)
		/// Clones OriginalItem to InnerItem (CloneInnerItem)
		/// Init some ViewModel properties (InitializePropertiesForViewing)
		/// Works in not UI thread
		/// Show Load animation
		/// </summary>
		public virtual void InitializeForOpen()
		{
#if DEBUG
			if (Application.Current != null && Application.Current.Dispatcher.Thread == Thread.CurrentThread)
			{
				throw new ThreadStateException("Has to invoke in not UI thread");
			}
#endif
			if (_mustReinitialize)
			{
				// has to do before open view (first time or each time after close the view)
				SetPropertyAtStartProcessUI("Loading ...");
				try
				{
					GetRepository();
					OnLoad();
					CloseAllSubscription();
					LoadInnerItem();
					InitializePropertiesForViewing();
					SetAllSubscription();
					_mustReinitialize = false;
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex, string.Format("An error occurred when trying to open {0}",
						ExceptionContextIdentity));
				}
				finally
				{
					SetPropertyAtStopProcessUI();
				}
			}
		}

		#endregion

		#region IViewModelDetailBase

		/// <summary>
		/// Save Item with out any UI changes if user has permission and Item is valid.
		/// Executing in not UI thread
		/// Used in wizard and so on
		/// </summary>
		public void SaveWithoutUIChanges()
		{
			if (HasPermission() && IsValidForSave())
			{
				try
				{
					CloseAllSubscription();
					BeforeSaveChanges();
					DoSaveChanges();
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex, string.Format("An error occurred when trying to save {0}".Localize(null, LocalizationScope.DefaultCategory),
													  ExceptionContextIdentity));
				}
			}
		}

		/// <summary>
		/// Save Item if user has permission and Item is valid.
		/// Rises DoSaveChanges in UI thread
		/// </summary>
		/// <param name="isCloseAfterSave">If true then close dialog after save</param>
		public async void SaveUI(bool isCloseAfterSave)
		{
			if (HasPermission() && IsValidForSave())
			{
				SetPropertyAtStartProcessUI("Saving ...");
				CloseAllSubscription();
				try
				{
					await Task.Run(() =>
						{
							BeforeSaveChanges();
							// Sasha: causes the error: Repository.Update(InnerItem);
							DoSaveChanges();
						});
					IsModified = false;
					RaiseSaveChangesCanExecute();
					AfterSaveChangesUI();
					if (isCloseAfterSave)
					{
						OnCloseViewRequestedEvent(EventArgs.Empty);
					}
					else
					{
						OnPropertyChanged("ViewTitle");
						OnPropertyChanged("DisplayName");
						OnPropertyChanged("OriginalItem");
					}
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex, string.Format("An error occurred when trying to save {0}".Localize(null, LocalizationScope.DefaultCategory),
													  ExceptionContextIdentity));
				}
				finally
				{
					if (!isCloseAfterSave)
					{
						SetAllSubscription();
					}
					SetPropertyAtStopProcessUI();
				}
			}
		}

		public void Delete()
		{
			if (HasPermission())
			{
				try
				{
					GetRepository();
					LoadInnerItem(ItemLoadingMode.Delete); //reload fresh InnerInner before delete (it can been modify or removed another user)

					if (BeforeDelete())
					{
						Repository.Remove(InnerItem);
						Repository.UnitOfWork.Commit();
						CloseWithoutSave();
					}
					else
					{
						throw new NotImplementedException();
					}
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex, string.Format("An error occurred when trying to remove {0}",
						ExceptionContextIdentity));
				}
			}
		}

		/// <summary>
		/// Duplicate Original item.
		/// Load InnerItem from repository using LoadInnerItem() method.
		/// Execute DoDuplicate for customization duplicate algorithm
		/// </summary>
		public void Duplicate()
		{
			try
			{
				GetRepository();
				LoadInnerItem(ItemLoadingMode.Duplicate); //reload fresh InnerInner before delete (it can been modify or removed another user)

				DoDuplicate();
				Repository.UnitOfWork.Commit();
				CloseWithoutSave();
			}
			catch (Exception ex)
			{
				ShowErrorDialog(ex, string.Format("An error occurred when trying to duplicate {0}",
					ExceptionContextIdentity));
			}
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Flag to reload data in InitializeForOpen()
		/// </summary>
		private bool _mustReinitialize = true;

		/// <summary>
		/// True if InnerItem tracking changes
		/// </summary>
		private bool _isSubscribed;

		/// <summary>
		/// Set properties before load data from DB
		/// </summary>
		/// <param name="text">Text of loading animation</param>
		private void SetPropertyAtStartProcessUI(string text)
		{
			IsInitializing = true;
            AnimationText = text.Localize();
			ShowLoadingAnimation = true;
		}

		/// <summary>
		/// Set properties after load data from DB
		/// </summary>
		private void SetPropertyAtStopProcessUI()
		{
			IsInitializing = false;
			ShowLoadingAnimation = false;
			AnimationText = "";
		}

		private void InitCommands()
		{
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			CancelConfirmRequest = new InteractionRequest<Confirmation>();

			SaveChangesCommand = new DelegateCommand<object>((x) => RaiseSaveInteractionRequest(), (x) => HasPermission() && IsModified);
			CancelCommand = new DelegateCommand<object>(RaiseCancelInteractionRequest);
		}

		private void RaiseSaveInteractionRequest()
		{
			SaveUI(false);
		}

		/// <summary>
		/// Implement cancelation request. Save Item if need
		/// </summary>
		/// <param name="arg"></param>
		private void RaiseCancelInteractionRequest(object arg)
		{
			if (!IsModified)
			{
				OnCloseViewRequestedEvent(EventArgs.Empty);
			}
			else if (!HasPermission())
			{
				CloseWithoutSave();
			}
			else
			{
				CancelConfirmRequest.Raise(CancelConfirm(), (x) =>
										   {
											   if (x.Confirmed)
											   {
												   SaveUI(true); //check thread
											   }
											   else if (((RefusedConfirmation)x).Refused)
											   {
												   CloseWithoutSave();
											   }
											   else
											   {
												   // cancel closing
												   var cancelArgs = arg as System.ComponentModel.CancelEventArgs;
												   if (cancelArgs != null)
												   {
													   cancelArgs.Cancel = true;
													   OpenItemCommand.Execute();
												   }
											   }
										   });
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		///  ViewModel's property changed Callback
		/// </summary>
		protected void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnUIThread(() => OnViewModelPropertyChangedUI(sender, e));
		}

		/// <summary>
		///  ViewModel's collection changed Callback
		/// </summary>
		protected void ViewModel_PropertyChanged(object sender,
			System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			OnUIThread(() => OnViewModelCollectionChangedUI(sender, e));
		}

		/// <summary>
		/// Set subscription to tracking changes of ViewModel's properties, InnerItem and its collections
		/// </summary>
		protected void SetAllSubscription()
		{
			if (InnerItem is INotifyPropertyChanged && !_isSubscribed)
			{
				OnUIThread(() =>
				{
					(InnerItem as INotifyPropertyChanged).PropertyChanged += ViewModel_PropertyChanged;
					SetSubscriptionUI();
					_isSubscribed = true;
				});
			}
		}

		/// <summary>
		/// Close subscription to tracking changes of ViewModel's properties, InnerItem and its collections
		/// </summary>
		protected void CloseAllSubscription()
		{
			if (InnerItem is INotifyPropertyChanged)
			{
				OnUIThread(() =>
				{
					(InnerItem as INotifyPropertyChanged).PropertyChanged -= ViewModel_PropertyChanged;
					CloseSubscriptionUI();
					_isSubscribed = false;
				});
			}
		}


		protected void CloseWithoutSave()
		{
			_mustReinitialize = true;
			IsModified = false;
			OnCloseViewRequestedEvent(EventArgs.Empty);
		}

		protected string GetNavigationKey(string id)
		{
			return string.Format("{0}: {1}", GetType(), id);
		}

		#endregion

		#region Abstract Members

		/// <summary>
		/// Text of identity ViewModel in ErrorDialog
		/// </summary>
		public abstract string ExceptionContextIdentity { get; }

		/// <summary>
		/// Resolve repository here
		/// </summary>
		protected abstract void GetRepository();

		/// <summary>
		/// Load full data for editing to OriginalItem instead short data that using at HomeView    
		/// Executing from InitializeForOpen if MustReinitialize is true
		/// Executing in not UI thread
		/// </summary>
		protected abstract void LoadInnerItem();

		/// <summary>
		/// Return true if InnerItem is correct for save in Detail mode
		/// </summary>
		/// <returns></returns>
		protected abstract bool IsValidForSave();

		/// <summary>
		/// Return RefusedConfirmation for Cancel Confirm dialog
		/// </summary>
		/// <returns></returns>
		protected abstract RefusedConfirmation CancelConfirm();

		#endregion

		#region Virtual Methods

		/// <summary>
		/// Return true if allow edit  
		/// </summary>
		/// <returns></returns>
		protected virtual bool HasPermission()
		{
			return true;
		}
		/// <summary>
		/// Execute before remove InnerItem from repository
		/// Repository has resolved and InnerItem attached to it. 
		/// </summary>
		protected virtual bool BeforeDelete()
		{
			return true;
		}

		protected virtual void DoDuplicate()
		{
		}

		/// <summary>
		/// Execute before DoSaveChanges()
		/// Executing in not UI thread.
		/// </summary>
		protected virtual void BeforeSaveChanges()
		{

		}

		/// <summary>
		/// Do changing and commit data in Repository
		/// Executing in not UI thread
		/// </summary>
		protected virtual void DoSaveChanges()
		{
			Repository.UnitOfWork.Commit();
		}

		/// <summary>
		/// Execute after DoSaveChanges()
		/// Executing in UI thread.
		/// </summary>
		protected virtual void AfterSaveChangesUI()
		{
		}

		/// <summary>
		/// Init some view model properties after Clone. (or load wizard step)
		/// Executing from InitializeForOpen if MustReinitialize is true.
		/// Executing in not UI thread.
		/// </summary>
		protected virtual void InitializePropertiesForViewing()
		{
		}

		/// <summary>
		/// Loads the inner item using loading mode provided.
		/// Calls default implementation by default. Should be overridden in derived classes to provide custom loading.
		/// </summary>
		/// <param name="mode">The loading mode.</param>
		protected virtual void LoadInnerItem(ItemLoadingMode mode)
		{
			LoadInnerItem();
		}

		/// <summary>
		/// Set subscription to tracking changes of ViewModel's properties or InnerItem's collections 
		/// Executes in UI thread 
		/// </summary>
		protected virtual void SetSubscriptionUI()
		{
		}

		/// <summary>
		/// Close subscription to tracking changes of ViewModel's properties or InnerItem's collections  
		/// Executes in UI thread 
		/// </summary>
		protected virtual void CloseSubscriptionUI()
		{
		}

		/// <summary>
		/// Execute when ViewModel Property was changed
		/// Execute in UI thread
		/// For change state don't use IsModified = true directly
		/// </summary>
		protected virtual void OnViewModelPropertyChangedUI(object sender, PropertyChangedEventArgs e)
		{
			IsModified = true;
			OnInnerItemChangedUI();
		}

		/// <summary>
		/// Execute when ViewModel Collection was changed
		/// Execute in UI thread
		/// </summary>
		protected virtual void OnViewModelCollectionChangedUI(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			IsModified = true;
			OnInnerItemChangedUI();
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnInnerItemChangedUI()
		{
			RaiseSaveChangesCanExecute();
		}

		private void RaiseSaveChangesCanExecute()
		{
			if (SaveChangesCommand != null)
			{
				SaveChangesCommand.RaiseCanExecuteChanged();
			}
		}


		protected virtual void OnLoad()
		{
		}

		protected virtual void OnClose()
		{
		}


		#endregion

	}

	/// <summary>
	/// Defines item loading modes
	/// </summary>
	public enum ItemLoadingMode
	{
		Default = 0,
		Duplicate = 1,
		Delete = 2
	}
}
