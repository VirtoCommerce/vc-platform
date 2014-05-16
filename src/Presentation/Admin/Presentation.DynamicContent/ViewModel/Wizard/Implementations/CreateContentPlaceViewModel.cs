using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Implementations
{
	public class CreateContentPlaceViewModel : WizardViewModelBase, ICreateContentPlaceViewModel
	{
		public CreateContentPlaceViewModel(IViewModelsFactory<IContentPlaceOverviewStepViewModel> overviewVmFactory, DynamicContentPlace item)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
		}

		protected CreateContentPlaceViewModel(DynamicContentPlace item)
		{
			InnerItem = item;
			InnerItem.PropertyChanged += InnerItem_PropertyChanged;
		}

		public DynamicContentPlace InnerItem
		{
			get;
			private set;
		}
		
		#region IWizardStep Members

		private bool _isValid;
		public override bool IsValid
		{
			get { return _isValid; }
		}

		public override bool IsLast
		{
			get
			{
				return this is IContentPlaceOverviewStepViewModel;
			}
		}

		public override string Comment
		{
			get
			{
				return string.Empty;
			}
		}

		public override string Description
		{
			get
			{
                return string.Format("Enter content place details".Localize());
			}
		}

		#endregion

		#region WizardViewModelBase overrides
		protected override void OnIsValidChanged()
		{
			bool doNotifyChanges = false;

			InnerItem.Validate(doNotifyChanges);

			_isValid = InnerItem.Errors.Count == 0 && (!string.IsNullOrEmpty(InnerItem.Name) && !string.IsNullOrEmpty(InnerItem.Description));

			base.OnIsValidChanged();
		}
		#endregion

		#region private members

		void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			OnIsValidChanged();
		}

		#endregion
	}

	public class ContentPlaceOverviewStepViewModel : CreateContentPlaceViewModel, IContentPlaceOverviewStepViewModel
	{
		public ContentPlaceOverviewStepViewModel(DynamicContentPlace item)
			: base(item)
		{

		}
	}
}
