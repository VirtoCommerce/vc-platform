using System;
using VirtoCommerce.Foundation.Assets.Repositories;

namespace VirtoCommerce.Platform.Core.Asset
{
    public interface IAssetsProviderManager
    {
        void RegisterProvider(string name, Func<string, IBlobStorageProvider> factory);
    }
}
