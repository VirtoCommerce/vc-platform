using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IClosable
    {
        event EventHandler CloseViewRequestedEvent;
        NavigationItem NavigationData { get; }
    }
}
