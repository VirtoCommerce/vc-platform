using System.Linq;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard
{
	public abstract class WizardContainerStepsViewModel : WizardViewModelBare, ISupportWizardSave
	{

		protected virtual void BeforePrepareSteps()
		{
		}

		protected virtual void AfterPrepareSteps()
		{
		}

		#region ISupportWizardSave

		public bool PrepareAndSave()
		{
			BeforePrepareSteps();
			foreach (var step in AllRegisteredSteps)
			{
				if (step is ISupportWizardPrepare)
				{
					(step as ISupportWizardPrepare).Prepare();
				}
			}
			AfterPrepareSteps();

			var vm = AllRegisteredSteps.First(x => x is IViewModelDetailBase) as IViewModelDetailBase;
			if (vm != null)
			{
				vm.SaveWithoutUIChanges();
			}
			return true;
		}

		#endregion
	}
}
