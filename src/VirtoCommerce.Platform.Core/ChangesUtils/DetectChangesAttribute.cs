using System;

namespace VirtoCommerce.Platform.Core.Utils.ChangeDetector
{
    /// <summary>
    /// Allows to mark public properties to use with <see cref="ChangeDetector"/>.
    /// </summary>
    public sealed class DetectChangesAttribute: Attribute
    {
        public string ChangeKey { get; private set; }
        public DetectChangesAttribute(string changeKey)
        {
            ChangeKey = changeKey;
        }
    }
}
