using System.Reflection;

namespace VirtoCommerce.Platform.Core.Utils.ChangeDetector
{
    public sealed class ChangesDetectorPropertyInfo
    {
        public string PropertyName { get; set; }
        public bool Inherited { get; set; }
        public string ChangeKey { get; set; }
        public MethodInfo Getter { get; set; }
    }
}
