using System;
using VirtoCommerce.Foundation.Assets.Repositories;

namespace VirtoCommerce.Framework.Web.Asset
{
    public interface IAssetsProviderManager
    {
        void RegisterProvider(string name, Func<string, IBlobStorageProvider> factory);
    }
}
