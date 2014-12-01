using System.Diagnostics.Tracing;

namespace VirtoCommerce.Slab.Constants
{
    public class VCTasks
    {
        // Event tasks
        public const EventTask DbQuery  = (EventTask)1;
        public const EventTask Remote   = (EventTask)2;
        public const EventTask IOAction = (EventTask)3;
        public const EventTask UIAction = (EventTask)4;
   }
}