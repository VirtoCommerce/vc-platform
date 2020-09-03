using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    public interface IHandlerRegistrar
    {
        void RegisterHandler<T>(Func<T, CancellationToken,Task> handler) where T : class, IMessage;
    }
}
