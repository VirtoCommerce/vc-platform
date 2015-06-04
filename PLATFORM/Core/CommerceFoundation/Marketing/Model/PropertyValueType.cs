using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model
{
    public enum PropertyValueType
    {
		LongString,
        ShortString,
        Image, /* URL to the image*/
        Decimal,
        Integer,
        Boolean,
        DateTime,
		Category /*Category id*/
    }
}
