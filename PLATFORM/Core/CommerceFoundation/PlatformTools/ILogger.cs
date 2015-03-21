using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.PlatformTools
{
    public interface ILogger
    {
        void TraceEvent(TraceEventType eventType, string message);
        void Error(string message);
        void Info(string message);
        void Warning(string message);
    }

}
