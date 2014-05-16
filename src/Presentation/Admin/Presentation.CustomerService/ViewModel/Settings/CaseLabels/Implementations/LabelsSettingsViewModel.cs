using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Interfaces;
using VirtoCommerce.ManagementClient.Localization;


namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Implementations
{
	public class LabelsSettingsViewModel : ViewModelBase, ILabelsSettingsViewModel, ISupportDelayInitialization
	{

		#region Dependencies

		private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;
		private readonly IViewModelsFactory<ILabelViewModel> _labelVmFactory;

		#endregion

		#region Constructor

		public LabelsSettingsViewModel(IViewModelsFactory<ILabelViewModel> labelVmFactory, IRepositoryFactory<ICustomerRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
			_labelVmFactory = labelVmFactory;

			CommandsInit();
		}

		#endregion

		#region Commands

		private void CommandsInit()
		{
			AddLabelCommand = new DelegateCommand(RaiseAddLabelRequest);
			EditLabelCommand = new DelegateCommand<Label>(RaiseEditLabelRequest, l => l != null);
			DeleteLabelCommand = new DelegateCommand<Label>(RaiseDeleteLabelRequest, l => l != null);

			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		private void RaiseAddLabelRequest()
		{
			var itemVM = _labelVmFactory.GetViewModelInstance(
				  new KeyValuePair<string, object>("item", new Label()));
			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(itemVM.InnerItem.Validate)
				{
					Title = "Add Label".Localize(),
					Content = itemVM
				},
				(x) =>
				{
					if (x.Confirmed)
					{
						OnUIThread(() => { ShowLoadingAnimation = true; });
						using (var repository = _repositoryFactory.GetRepositoryInstance())
						{
							repository.Add(itemVM.InnerItem);
							repository.UnitOfWork.Commit();
						}
						OnUIThread(() =>
						{
							Items.Add(itemVM.InnerItem);
							var view = CollectionViewSource.GetDefaultView(Items);
							view.MoveCurrentTo(itemVM.InnerItem);
							ShowLoadingAnimation = false;
						});
					}
				}
			);
		}

		private void RaiseEditLabelRequest(Label selectedLabel)
		{
			var itemVM = _labelVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", selectedLabel));
			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(itemVM.InnerItem.Validate)
				{
					Title = "Edit Label".Localize(),
					Content = itemVM
				},
				(x) =>
				{
					if (x.Confirmed)
					{
						ShowLoadingAnimation = true;
						Task.Factory.StartNew(() =>
						{
							using (var _repository = _repositoryFactory.GetRepositoryInstance())
							{
								var labelForUpdate = _repository.Labels.Where(l => l.LabelId == selectedLabel.LabelId).SingleOrDefault();
								if (labelForUpdate != null)
								{
									OnUIThread(() => labelForUpdate.InjectFrom<CloneInjection>(selectedLabel));
									_repository.UnitOfWork.Commit();
								}
							}
						}).ContinueWith(t =>
						{
							if (t.Exception != null)
							{
								if (t.Exception.InnerExceptions.Count > 0)
								{
									ShowErrorDialog(t.Exception.InnerExceptions[0], string.Format("Error: {0}", t.Exception.InnerExceptions[0].Message));
								}
							}
							else
							{
								var view = CollectionViewSource.GetDefaultView(Items);
								view.Refresh();
							}
							ShowLoadingAnimation = false;
						}, TaskScheduler.FromCurrentSynchronizationContext());
					}
				}
			);
		}

		private void RaiseDeleteLabelRequest(Label selectedLabel)
		{
			CommonConfirmRequest.Raise(
				new ConditionalConfirmation
				{
					Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory),
					Content = string.Format("Are you sure you want to delete Label '{0}'?".Localize(), selectedLabel.Name)
				},
				(x) =>
				{
					if (x.Confirmed)
					{
						ShowLoadingAnimation = true;
						Task.Factory.StartNew(() =>
						{
							var labelForDelete = selectedLabel;
							using (var _repository = _repositoryFactory.GetRepositoryInstance())
							{
								_repository.Remove(labelForDelete);
								_repository.UnitOfWork.Commit();
							}
							return labelForDelete;

						}).ContinueWith(t =>
						{
							if (t.Exception != null)
							{
								if (t.Exception.InnerExceptions.Count > 0)
								{
									ShowErrorDialog(t.Exception.InnerExceptions[0], string.Format("Error: {0}", t.Exception.InnerExceptions[0].Message));
								}
							}
							else
							{
								Items.Remove(t.Result);
							}
							ShowLoadingAnimation = false;
						}, TaskScheduler.FromCurrentSynchronizationContext());
					}
				}
			);
		}

		#endregion

		#region ICustomerServiceLabelsSettingViewModel Members

		public DelegateCommand AddLabelCommand { get; private set; }
		public DelegateCommand<Label> EditLabelCommand { get; private set; }
		public DelegateCommand<Label> DeleteLabelCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		private ObservableCollection<Label> _items;
		public ObservableCollection<Label> Items
		{
			get { return _items; }
			set
			{
				_items = value;
				OnPropertyChanged();
			}
		}

		public void RaiseCanExecuteChanged()
		{
			EditLabelCommand.RaiseCanExecuteChanged();
			DeleteLabelCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region ISupportDelayInitialization Members

		public void InitializeForOpen()
		{
			using (var _repository = _repositoryFactory.GetRepositoryInstance())
			{
				OnUIThread(async () =>
					{
						ShowLoadingAnimation = true;
						var items = await Task.Run(() => _repository.Labels.ToList());
						Items = new ObservableCollection<Label>(items);
						ShowLoadingAnimation = false;
					});
			}
		}

		#endregion


	}
}
