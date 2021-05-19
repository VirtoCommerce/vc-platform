using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.Platform.Core.ChangesUtils
{
    public class UseInChangesDetectorAttribute: Attribute
    {
        public string ChangeKey { get; set; }
        public UseInChangesDetectorAttribute( string changeKey )
        {
            ChangeKey = changeKey;
        }
    }
}
