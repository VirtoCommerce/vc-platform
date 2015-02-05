using System.Diagnostics.Tracing;

namespace VirtoCommerce.Slab.Constants
{
    public class VCOpCodes
    {
        public const EventOpcode Read   = (EventOpcode)11;
        public const EventOpcode Add    = (EventOpcode)12;
        public const EventOpcode Modify = (EventOpcode)13;
        public const EventOpcode Remove = (EventOpcode)14;
        public const EventOpcode Search = (EventOpcode)15;
    }
}