using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Jobs
{
    public class Job : Entity
    {
        public string State { get; set; }
        public bool Completed { get; set; }
    }
}
