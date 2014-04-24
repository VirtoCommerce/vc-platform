using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.Properties;

namespace VirtoCommerce.ManagementClient
{
	public class ShellViewModel : ViewModelBase
	{
		#region Dependencies

		private readonly NavigationManager _navigationManager;
		private readonly IRegionManager _regionManager;

		#endregion

		public ShellViewModel()
		{
			_regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
			_navigationManager = ServiceLocator.Current.GetInstance<NavigationManager>();
			CommandInit();
			OnUIThread(() => OnPropertyChanged("IsMainMenuVisible"));
			RiseWindowResizeCommand();
			var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
			container.RegisterType<IGlobalConfigService, GlobalConfigService>(new ContainerControlledLifetimeManager());
			EventSystem.Subscribe<ShellMessageEvent>((mess) => OnUIThread(() =>
				{
					NumAssignedCases = mess.Message == "0" ? "" : mess.Message;
				}));
			EventSystem.Subscribe<GenericEvent<Tuple<List<CultureInfo>, Action<string>>>>(xx => OnUIThread(() =>
			{
				AvailableGuiCultures.Clear();

				xx.Message.Item1.ForEach(x =>
					{
						AvailableGuiCultures.Add(x);
					});
				if (AvailableGuiCultures.All(x => x.Name != "en-US"))
				{
					AvailableGuiCultures.Insert(0, CultureInfo.GetCultureInfo("en-US"));
				}

				if (AvailableGuiCultures.All(x => x.Name != System.Threading.Thread.CurrentThread.CurrentUICulture.Name))
				{
					CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
				}
				GuiCulture = System.Threading.Thread.CurrentThread.CurrentUICulture.ThreeLetterWindowsLanguageName;
				ChangeLanguageAction = xx.Message.Item2;
			}));
		}

		private void CommandInit()
		{
			BackCommand = new DelegateCommand<object>(RaiseBackCommand);

			ShowMainMenuCommand = new DelegateCommand(ShowMainMenu);
			CloseMainMenuCommand = new DelegateCommand(CloseMainMenu, () => IsMainMenuVisible = true);

			GoToDashboardCommand = new DelegateCommand(GoToDashboard);
			WindowResizeCommand = new DelegateCommand(RiseWindowResizeCommand);

			NavigateToCustomersCommand = new DelegateCommand(NavigateToCustomers);

			AvailableGuiCultures = new ObservableCollection<CultureInfo>();
			ChangeLanguageCommand = new DelegateCommand<string>(DoChangeLanguage);
		}

		private void DoChangeLanguage(string cultureName)
		{
			if (ChangeLanguageAction != null)
				ChangeLanguageAction(cultureName);

			GuiCulture = System.Threading.Thread.CurrentThread.CurrentUICulture.ThreeLetterWindowsLanguageName;
		}

		#region Public Members

		public string AssemblyVersion
		{
			get
			{
				var assembly = Assembly.GetExecutingAssembly();
				return String.Format(Resources._0___Build__1__, assembly.GetInformationalVersion(), assembly.GetFileVersion());
			}
		}

		public bool IsMessageShow
		{
			get { return !String.IsNullOrEmpty(NumAssignedCases); }
		}

		#endregion

		#region Commands

		public DelegateCommand<object> BackCommand { get; private set; }

		public DelegateCommand ShowMainMenuCommand { get; private set; }
		public DelegateCommand CloseMainMenuCommand { get; private set; }

		public DelegateCommand GoToDashboardCommand { get; private set; }
		public DelegateCommand WindowResizeCommand { get; private set; }

		public DelegateCommand NavigateToCustomersCommand { get; private set; }

		#endregion


		#region Commands Implementation

		private void RaiseBackCommand(object changedProperty)
		{
			if (_navigationManager != null)
			{
				_navigationManager.Back();
			}
		}

		private void ShowMainMenu()
		{
			OnUIThread(() => { IsMainMenuVisible = true; });
		}

		private void CloseMainMenu()
		{
			OnUIThread(() => { IsMainMenuVisible = false; });
		}

		private void GoToDashboard()
		{
			NavigateTo(Main.NavigationNames.HomeName);
		}

		private void NavigateToCustomers()
		{
			NavigateTo(Customers.NavigationNames.HomeName);
		}

		private void RiseWindowResizeCommand()
		{
			NumHorizontalDocuments = (int)((Application.Current.MainWindow.ActualWidth - 400) / 150);
		}

		#endregion

		#region VirtoCommerce.ManagementClient Properties

		private IRegion _mainRegion;
		private IRegion MainRegion
		{
			get
			{
				if (_mainRegion == null && _regionManager.Regions.ContainsRegionWithName(RegionNames.MainRegion))
				{
					_mainRegion = _regionManager.Regions[RegionNames.MainRegion];
					_mainRegion.Views.CollectionChanged += (s, e) => RefreshHorizontalAndVerticalDocs();
				}
				return _mainRegion;
			}
		}

		private bool _isMainMenuVisible;
		public bool IsMainMenuVisible
		{
			get { return _isMainMenuVisible; }
			set
			{
				_isMainMenuVisible = value;
				OnPropertyChanged();
			}
		}

		private string _numAssignedCases = string.Empty;
		public string NumAssignedCases
		{
			get { return _numAssignedCases; }
			set
			{
				_numAssignedCases = value;
				OnPropertyChanged("IsMessageShow");
				OnPropertyChanged();
			}
		}

		private string _userName;
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged();
			}
		}

		public string GuiCulture
		{
			get { return _guiCulture; }
			set { _guiCulture = value; OnPropertyChanged(); }
		}

		public ObservableCollection<CultureInfo> AvailableGuiCultures
		{
			get { return _availableGuiCultures; }
			set
			{
				_availableGuiCultures = value;
				OnPropertyChanged();
			}
		}

		public DelegateCommand<string> ChangeLanguageCommand { get; set; }
		private Action<string> ChangeLanguageAction;

		private string _baseUrl;
		public string BaseUrl
		{
			get { return _baseUrl; }
			set
			{
				_baseUrl = value;
				OnPropertyChanged();
			}
		}

		private IViewModel _selecteditem;
		public IViewModel SelectedItem
		{
			get { return _selecteditem; }
			set
			{
				_selecteditem = value;
				OnPropertyChanged();
				RefreshDocs();
			}
		}

		private int _numHorizontalDocuments;
		private string _guiCulture;
		private ObservableCollection<CultureInfo> _availableGuiCultures;

		public int NumHorizontalDocuments
		{
			get { return _numHorizontalDocuments; }
			set
			{
				_numHorizontalDocuments = value;
				RefreshDocs();

			}
		}
		#endregion

		public bool IsVerticalMenuShow
		{
			get { return VerticalDocs != null && VerticalDocs.Count > 0; }
		}

		public List<IViewModel> HorizontalDocs
		{
			get
			{
				return
					 MainRegion == null || !MainRegion.Views.Any() ? new List<IViewModel>() :
						MainRegion.Views.OfType<IViewModel>().Where(
							x => x is IClosable && x is IOpenTracking && x.MenuOrder <= NumHorizontalDocuments)
													  .OrderBy(x => x.MenuOrder)
													  .ToList();
			}
		}

		public List<IViewModel> VerticalDocs
		{
			get
			{
				return
					 MainRegion == null || !MainRegion.Views.Any() ? new List<IViewModel>() :
							 MainRegion.Views.OfType<IViewModel>().Where(x => x is IClosable && x.MenuOrder > NumHorizontalDocuments)
													  .OrderBy(x => x.MenuOrder)
													  .ToList();
			}
		}

		private void RefreshDocs()
		{
			if (MainRegion != null && MainRegion.Views.Any())
			{
				if (SelectedItem == null)
				{
					MainRegion.Views.OfType<IOpenTracking>().Where(x => x.IsActive).ForEach(x => x.IsActive = false);
					var i = 1;
					MainRegion.Views.OfType<IViewModel>().Where(x => x.MenuOrder > 0).OrderBy(x => x.MenuOrder).ForEach(x => x.MenuOrder = i++);
				}
				else
				{
					if (SelectedItem is IOpenTracking)
					{
						(SelectedItem as IOpenTracking).IsActive = true;
					}

					// Reorder opened documents
					if (SelectedItem is IClosable && (SelectedItem.MenuOrder == 0 || SelectedItem.MenuOrder > NumHorizontalDocuments))
					{
						MainRegion.Views.OfType<IViewModel>().Where(
							x => x.MenuOrder > 0 && (SelectedItem.MenuOrder == 0 || x.MenuOrder < SelectedItem.MenuOrder))
												  .ForEach(x => x.MenuOrder++);
						SelectedItem.MenuOrder = 1;
					}

					RefreshHorizontalAndVerticalDocs();
				}
			}
		}

		private void RefreshHorizontalAndVerticalDocs()
		{
			OnUIThread(() =>
			{
				OnPropertyChanged("VerticalDocs");
				OnPropertyChanged("HorizontalDocs");
				OnPropertyChanged("IsVerticalMenuShow");
			});
		}

		private void NavigateTo(string navigationName)
		{
			var navigationData = _navigationManager.GetNavigationItemByName(navigationName);
			if (navigationData == null)
			{
				var navigationItemId = Guid.NewGuid().ToString();

				navigationData = new NavigationItem(navigationItemId, null, null, this);
			}

			_navigationManager.Navigate(navigationData);
		}
	}
}
