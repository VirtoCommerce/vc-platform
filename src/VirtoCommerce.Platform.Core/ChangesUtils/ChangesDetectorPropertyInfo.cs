using System.Reflection;

namespace VirtoCommerce.Platform.Core.ChangesUtils
{
    public class ChangesDetectorPropertyInfo
    {
        public string PropertyName { get; set; }
        public string ChangeKey { get; set; }
        public MethodInfo Getter { get; set; }
    }
}
