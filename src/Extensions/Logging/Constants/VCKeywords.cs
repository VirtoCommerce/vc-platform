using System.Diagnostics.Tracing;

namespace VirtoCommerce.Slab.Constants
{
    public class VCKeywords
    {
        // Keywords for indicating subject of event
        public const EventKeywords Web      = (EventKeywords)1;
        public const EventKeywords WebApi   = (EventKeywords)(1 << 2);
        public const EventKeywords DataBase = (EventKeywords)(1 << 3);
        public const EventKeywords Workflow = (EventKeywords)(1 << 4);
        public const EventKeywords Jobs     = (EventKeywords)(1 << 5);

        // Keywords for indicating type of event
        public const EventKeywords Diagnostic   = (EventKeywords)(1 << 10);
        public const EventKeywords Performance  = (EventKeywords)(1 << 11);
        public const EventKeywords Auditing     = (EventKeywords)(1 << 12);

        public const EventKeywords Custom       = (EventKeywords)(1 << 30);
    }
}