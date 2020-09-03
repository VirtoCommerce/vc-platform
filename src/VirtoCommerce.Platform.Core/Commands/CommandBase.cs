using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Commands
{
    public class CommandBase : Entity, ICommand
    {
        public int ExpectedVersion { get; set; }
    }
}
