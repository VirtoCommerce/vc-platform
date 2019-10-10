using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Domain
{
    public interface IUnitOfWork
	{
		int Commit();
        Task<int> CommitAsync();
	}
}
