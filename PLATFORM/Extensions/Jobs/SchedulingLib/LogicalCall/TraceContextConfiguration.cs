using System.Collections.Generic;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    public class TraceContextConfiguration
    {
        public bool Trace { get; set; }
        public bool Activity { get; set; }
        public bool BufferizeCatchExceptionAndFlash { get; set; }
        public Dictionary<string, string> Configs { get; set; }
    }
}