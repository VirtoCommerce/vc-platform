using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard
{
	public interface ISupportWizardSave
	{
		void PrepareAndSave();
	}
	public interface ISupportWizardPrepare
	{
		void Prepare();
	}

}
