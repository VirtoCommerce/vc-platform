using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
    public enum PropertyValueType
    {
        ShortString,
        LongString,
        Image, /* URL to the image*/
        Decimal,
        Integer,
        Boolean,
        DateTime
    }
}
