using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    public interface IHandlerRegistrar
    {
        [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        void RegisterHandler<T>(Func<T, CancellationToken, Task> handler) where T : IMessage;
    }
}
