using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Models;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels
{
	public class ProjectsViewModel : HomeSettingsEditableViewModel<Project>, IProjectsViewModel
	{
		#region Dependencies
		private readonly IViewModelsFactory<IConfigurationWizardViewModel> _vmFactory;
		private readonly IRepositoryFactory<IProjectRepository> _projectRepositoryFactory;
		#endregion

		#region Constructor
		public ProjectsViewModel(IViewModelsFactory<IConfigurationWizardViewModel> vmFactory, IRepositoryFactory<IProjectRepository> projectRepositoryFactory)
			: base(null, vmFactory, null)
		{
			_vmFactory = vmFactory;
			_projectRepositoryFactory = projectRepositoryFactory;

			ViewTitle = new ViewTitleBase { Title = "Projects", SubTitle = "VIRTO COMMERCE PROJECT MANAGER" };
			CommandInit();
		}

		void CommandInit()
		{
			ItemHelpCommand = new DelegateCommand(RaiseItemHelpInteractionRequest);

			MenuItemExploreCommand = new DelegateCommand(MenuItemExplorer, () => IsProjectActionAvailable);
			MenuItemAdminCommand = new DelegateCommand(MenuItemAdmin, () => IsProjectActionAvailable);
			MenuItemEditVSCommand = new DelegateCommand(MenuItemEditVS, () => IsProjectActionAvailable);
			MenuItemBrowseCommand = new DelegateCommand(MenuItemBrowse, () => IsProjectActionAvailable);
			MenuItemRemoveCommand = new DelegateCommand(MenuItemRemove);
		}

		#endregion

		#region Commands

		public DelegateCommand ItemHelpCommand { get; private set; }

		public DelegateCommand MenuItemExploreCommand { get; private set; }
		public DelegateCommand MenuItemAdminCommand { get; private set; }
		public DelegateCommand MenuItemEditVSCommand { get; private set; }
		public DelegateCommand MenuItemBrowseCommand { get; private set; }
		public DelegateCommand MenuItemRemoveCommand { get; private set; }

		#endregion

		#region Commands Implementation

		private bool IsProjectActionAvailable
		{
			get
			{
				return SelectedProject != null && SelectedProject.Status == Status.Online;
			}
		}

		private async void RaiseItemBrowseInteractionRequest(Project item)
		{
			ShowLoadingAnimation = true;
			try
			{
				await Task.Run(() =>
				{
					new SiteHelper(_projectRepositoryFactory).StartSite(item);
					RefreshItem(item);
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to browse the site, " + ex.StackTrace);
			}
			finally
			{
				ShowLoadingAnimation = false;
			}
		}

		private void MenuItemRemove()
		{
			RaiseItemDeleteInteractionRequest(SelectedProject);
		}

		private void MenuItemBrowse()
		{
			RaiseItemBrowseInteractionRequest(SelectedProject);
		}

		private void MenuItemExplorer()
		{
			Process.Start(SelectedProject.Location.LocalPath);
		}

		private void MenuItemEditVS()
		{
			Process.Start(string.Format("{0}\\StoreWebApp.csproj", SelectedProject.Location.LocalPath));
		}

		private void RaiseItemHelpInteractionRequest()
		{
			Process.Start("http://virtocommerce.com/sdk-help");
		}

		private void MenuItemAdmin()
		{
			if (Uri.IsWellFormedUriString(SelectedProject.BrowseUrl, UriKind.Absolute))
			{
				var directory = Path.GetDirectoryName(typeof(ProjectsViewModel).Assembly.Location);
				var resourcesDir = Path.GetFullPath(Path.Combine(directory, @"..\Resources"));

				// arguments can be passed through shortcut (.appref-ms) files only
				var shortcutPath = Path.Combine(resourcesDir, "VirtoCommerceManager.appref-ms");
				if (!File.Exists(shortcutPath))
				{
					// Create a shortcut file.
					var uri = new Uri(resourcesDir + "/Admin/VirtoCommerce.application"); 
					var createText = uri.AbsoluteUri + "#VirtoCommerce.application, Culture=neutral, PublicKeyToken=daec1fcaccc0d1ab, processorArchitecture=msil";
					File.WriteAllText(shortcutPath, createText, Encoding.Unicode);
				}

				var url = string.Format("storeurl={0}", SelectedProject.BrowseUrl);
				Process.Start(shortcutPath, url);
			}
		}

		#endregion

		#region Properties

		private Project _selectedProject;
		public Project SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				_selectedProject = value;
				OnPropertyChanged();

				MenuItemExploreCommand.RaiseCanExecuteChanged();
				MenuItemAdminCommand.RaiseCanExecuteChanged();
				MenuItemEditVSCommand.RaiseCanExecuteChanged();
				MenuItemBrowseCommand.RaiseCanExecuteChanged();
				MenuItemRemoveCommand.RaiseCanExecuteChanged();
			}
		}

		#endregion

		protected override void RaiseItemAddInteractionRequest()
		{
			var project = new Project()
			{
				Id = Guid.NewGuid(),
				Created = DateTime.Now,
				Version = "1.0.0.0",
				Location = new ProjectLocation()
			};

			var vm = _vmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", project));
			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create New Project",
				Content = vm
			};
			ItemAdd(project, confirmation, null);
		}

		protected override void RaiseItemDeleteInteractionRequest(Project item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure to delete project '{0}'?", item.Name),
				Title = "Delete confirmation"
			};

			var repository = _projectRepositoryFactory.GetRepositoryInstance();
			ItemDelete(item, confirmation, repository);
		}

		protected override void RaiseItemEditInteractionRequest(Project item)
		{
		}

		protected override object LoadData()
		{
			var repository = _projectRepositoryFactory.GetRepositoryInstance();
			var list = repository.Projects.OrderByDescending(x => x.Created).ToList();
			
			// now go through each project and check the version and if it is online (directory is available)
			foreach (var project in list)
			{
				UpdateProjectStatus(project);
			}

			return list;
		}

		private void UpdateProjectStatus(Project project)
		{
			project.Status = Status.Offline;
			if (Directory.Exists(project.Location.LocalPath))
			{
				project.Status = Status.Online;

				try
				{
					// now get version
					var fvi = FileVersionInfo.GetVersionInfo(String.Format("{0}/bin/VirtoCommerce.Foundation.dll", project.Location.LocalPath));
					project.Version = fvi.FileVersion;
				}
				catch
				{
					project.Version = "unknown";
					project.Status = Status.Offline;
				}
			}
		}

		public override void RefreshItem(object item)
		{
			UpdateProjectStatus((Project)item);
		}
	}
}
