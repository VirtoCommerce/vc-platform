using System;

namespace VirtoCommerce.Platform.Core.Utils.ChangeDetector
{
    public sealed class DetectChangesAttribute: Attribute
    {
        public string ChangeKey { get; private set; }
        public DetectChangesAttribute(string changeKey)
        {
            ChangeKey = changeKey;
        }
    }
}
