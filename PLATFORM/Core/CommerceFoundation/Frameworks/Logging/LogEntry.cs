using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Frameworks.Logging
{
    [DataContract, Serializable]
    public partial class LogEntry
    {
        public virtual DateTime Timestamp { get; set; }
        public virtual string Key { get; set; }
        public LogOperation Op { get; set; }
        public int Index { get; set; }

        public LogEntry()
        {
        }

        public LogEntry(string key, LogOperation op)
        {
            this.Key = key;
            this.Op = op;
        }
    }
}
