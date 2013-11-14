using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
    [DataContract]
	[EntitySet("CatalogBases")]
    public class VirtualCatalog : CatalogBase
    {
    }
}
