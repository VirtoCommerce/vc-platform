using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts.Stores
{
    public class SyncAsset
    {
        #region Public Properties

        public string Content { get; set; }

        public byte[] ByteContent { get; set; }

        public string ContentType { get; set; }

        public string Id { get; set; }

        public DateTime Updated { get; set; }

        #endregion
    }
}
