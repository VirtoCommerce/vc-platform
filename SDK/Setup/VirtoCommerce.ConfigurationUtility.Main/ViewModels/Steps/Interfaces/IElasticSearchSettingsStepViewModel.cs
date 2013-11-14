namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces
{
	public interface IElasticSearchSettingsStepViewModel : IConfigureStep
	{
		string IndexesLocation { get; set; }
		string IndexScope { get; set; }
	}
}