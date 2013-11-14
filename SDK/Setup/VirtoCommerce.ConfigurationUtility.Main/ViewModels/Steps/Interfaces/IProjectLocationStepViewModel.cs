using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces
{
	public interface IProjectLocationStepViewModel : IConfigureStep
	{
		string ProjectLocation { get; }
		string ProjectName { get; set; }
		string ProjectPath { get; set; }
		
		DelegateCommand<object> BrowseCommand { get; }
	}
}