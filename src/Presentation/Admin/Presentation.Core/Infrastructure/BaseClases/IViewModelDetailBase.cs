using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IViewModelDetailBase : IViewModel
    {
        void Delete();
	    void Duplicate();
		void SaveWithoutUIChanges();
        void SaveUI(bool isCloseAfterSave);
    }
}
