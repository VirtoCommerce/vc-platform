using System;

namespace VirtoCommerce.Scheduling
{
    public static class Helper
    {
        public static string FormatTrace(string message, string className = "CLASS NOT PROVIDED", string methodName = "METHOD NAME NOT PROVIDED", string additionalContext = "", string cloudContext = "")
        {
            return String.Format("TRACE|{0}|{1}|{2}|{3}|{4}|{5}", DateTime.UtcNow, className, methodName, additionalContext, cloudContext, message);
        }

        public static string FormatException(Exception ex, string className = "CLASS NOT PROVIDED", string methodName = "METHOD NAME NOT PROVIDED", string additionalContext = "", string cloudContext = "")
        {
            return String.Format("EXCEPTION|{0}|{1}|{2}|{3}|{4}|{5}", DateTime.UtcNow, className, methodName, additionalContext, cloudContext, ex);
        }

    }
}
