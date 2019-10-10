using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Commands
{
    public interface ICommand : IMessage
    {
        int ExpectedVersion { get; }
    }
}
