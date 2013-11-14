using System;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
        bool IsInitializing { get; set; }

        string DisplayName { get; }

        ViewTitleBase ViewTitle { get; set; }

        string IconSource { get; }

        bool IsBackTrackingDisabled { get; }

        int MenuOrder { get; set; }
    }
}
