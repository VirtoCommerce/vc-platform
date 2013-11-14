using System;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    /// <summary>
    /// Interface for window minimize command
    /// </summary>
    public interface IMinimizable
    {
        event EventHandler MinimizableViewRequestedEvent;

        DelegateCommand MinimizeCommand { get; }
    }
}
