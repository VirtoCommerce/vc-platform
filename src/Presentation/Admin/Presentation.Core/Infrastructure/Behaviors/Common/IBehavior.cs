namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors.Common
{
	public interface IBehavior
	{
		bool IsApplicable();

		void Attach();

		void Detach();

		void Update();
	}
}