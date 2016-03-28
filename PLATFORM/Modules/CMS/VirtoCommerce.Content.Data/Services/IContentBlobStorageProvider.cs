using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.Content.Data.Services
{
    /// <summary>
    /// Represent functionality to  cms blob content access 
    /// </summary>
    public interface IContentBlobStorageProvider : IBlobStorageProvider
    {
        void MoveContentItem(string oldUrl, string newUrl);

    }
}
