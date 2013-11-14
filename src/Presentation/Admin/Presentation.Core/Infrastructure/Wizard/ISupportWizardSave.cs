
namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard
{
	public interface ISupportWizardSave
	{
		bool PrepareAndSave();
	}

	public interface ISupportWizardPrepare
	{
		void Prepare();
	}

}
