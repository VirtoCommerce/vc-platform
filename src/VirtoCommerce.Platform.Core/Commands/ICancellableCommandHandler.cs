using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Commands
{
    public interface ICancellableCommandHandler<in T> : ICancellableHandler<T> where T : ICommand
    {
    }
}
