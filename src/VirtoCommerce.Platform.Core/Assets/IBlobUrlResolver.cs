using System;

namespace VirtoCommerce.Platform.Core.Assets
{
    [Obsolete("Deprecated. Use assets from Assets module.")]
    public interface IBlobUrlResolver
    {
        string GetAbsoluteUrl(string blobKey);
    }
}
