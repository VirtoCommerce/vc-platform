using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace VirtoCommerce.Platform.Core
{
    public class VirtualFolderOptions
    {
        public VirtualFolderOptions()
        {
            Items = new Dictionary<PathString, string>();
        }
        public IDictionary<PathString, string> Items { get; private set; }

    }
}
