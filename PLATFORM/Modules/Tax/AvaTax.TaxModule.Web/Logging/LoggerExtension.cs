using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace AvaTax.TaxModule.Web.Logging
{
    public static class LoggerExtension
    {
        public static void Write(this AvalaraLogger log, int eventCode, object context)
        {
            try
            {
                var type = context.GetType();
                var method =
                    log.GetType().GetMethods().FirstOrDefault(x => x.GetCustomAttributes(typeof(EventAttribute), true).Any() && ((EventAttribute)x.GetCustomAttributes(typeof(EventAttribute), true).First()).EventId == eventCode);
                if (method != null)
                {
                    var parameters = new List<object>();
                    method.GetParameters().ToList().ForEach(value => parameters.Add(type.GetProperty(value.Name).GetValue(context)));
                    method.Invoke(log, parameters.ToArray());
                }
            }
            catch (Exception)
            {
                //try log error
            }

        }
    }
}