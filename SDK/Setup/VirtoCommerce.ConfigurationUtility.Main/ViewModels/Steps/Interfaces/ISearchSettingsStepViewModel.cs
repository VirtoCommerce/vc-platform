namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces
{
	public interface ISearchSettingsStepViewModel : IConfigureStep
	{
		string IndexesLocation { get; set; }
		string IndexScope { get; set; }
		string LuceneFolderLocation { get; set; }
	}
}