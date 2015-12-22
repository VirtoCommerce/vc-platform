using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Data.Asset;

namespace VirtoCommerce.Content.Data.Services
{
    public class ContentStorageProviderImpl : FileSystemBlobProvider, IContentStorageProvider
    {
        public ContentStorageProviderImpl(string storagePath)
            : base(storagePath)
        {
        }
    }
}
