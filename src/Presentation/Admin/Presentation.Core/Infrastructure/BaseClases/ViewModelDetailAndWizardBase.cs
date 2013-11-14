using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public abstract class ViewModelDetailAndWizardBase<T> : ViewModelDetailBase<T>, IWizardStep where T : class
    {
        protected ViewModelDetailAndWizardBase(IFactory entityFactory, T item, bool isWizardMode)
            : base(entityFactory, item)
        {
            IsWizardMode = isWizardMode;
		}

        public static bool IsInDesignMode
        {
            get
            {
#if DEBUG
                return (bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty
                    , typeof (DependencyObject))
                    .Metadata.DefaultValue;
#else
                return false;
#endif
            }
        }

		/// <summary>
        /// True if created for Wizard, False - Detail
        /// </summary>
        protected bool IsWizardMode = false;

        //todo remove it after remove from XAML
        public bool IsSingleDialogEditing
        {
            get { return !IsWizardMode; }
        }

        protected void OnIsValidChanged()
        {
            var tmp = StepIsValidChangedEvent;
            if (tmp != null)
            {
                tmp(this, null);
            }
        }

        #region IWizardStep Members

        public virtual string Comment { get { return string.Empty; } }
		
		
		public virtual bool IsValid
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public virtual bool IsLast
		{
			get { return false; }
		}

		public virtual string Description
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		


        // derived classes should implement IWizardStep and use this member
        public event EventHandler StepIsValidChangedEvent;

        #endregion

        #region Override ViewModelDetailBase Methods

        /// <summary>
        /// Do changing and commit data in Repository
        /// Executing in not UI thread
        /// </summary>
        protected override void DoSaveChanges()
        {
            if (IsWizardMode)
            {
                Repository.Add(InnerItem);
            }
            base.DoSaveChanges();
        }

        /// <summary>
        /// Execute when ViewModel Property was changed
        /// Execute in UI thread
        /// </summary>
        protected override void OnViewModelPropertyChangedUI(object sender, PropertyChangedEventArgs e)
        {
            if (IsWizardMode)
            {
                OnIsValidChanged();
            }
            else
            {
                base.OnViewModelPropertyChangedUI(sender, e);
            }
        }

        /// <summary>
        /// Execute when ViewModel Collection was changed 
        /// Execute in UI thread
        /// </summary>
        protected override void OnViewModelCollectionChangedUI(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (IsWizardMode)
            {
                OnIsValidChanged();
            }
            else
            {
                base.OnViewModelCollectionChangedUI(sender, e);
            }
        }

		/// <summary>
		/// Rises then Wizard is opened  (InitializeForOpen doesn't execute when Wizard Mode)
		/// </summary>
        public sealed override void InitializeForOpen()
        {
#if DEBUG
			if (Application.Current != null && Application.Current.Dispatcher.Thread == Thread.CurrentThread)
			{
				throw new ThreadStateException("Has to invoke in not UI thread");
			}
#endif
			if (IsWizardMode)
            {
	            try
	            {
					GetRepository();
					OnLoad();
					InitializePropertiesForViewing(); //override it in each step and init only necessary property for current step
					SetAllSubscription();
				}
	            catch (Exception ex)
	            {
					ShowErrorDialog(ex, string.Format("An error occurred when trying to initialize {0}",
						ExceptionContextIdentity));
	            }
            }
            else
            {
                base.InitializeForOpen();
            }
        }

		#endregion

    }
}
