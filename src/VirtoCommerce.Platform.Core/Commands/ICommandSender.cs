using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Commands
{
    public interface ICommandSender
    {
        Task Send<T>(T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand;
    }
}
