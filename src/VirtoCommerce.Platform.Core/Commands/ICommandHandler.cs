using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Commands
{
    public interface ICommandHandler<in T> : IHandler<T> where T : ICommand
    {
    }
}
