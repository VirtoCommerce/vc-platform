using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall.Configuration 
{
    public class TraceContextConfigurationSection : ConfigurationSection, ITraceContextConfigurator
    {
        #region ConfigurationSection support
        private static readonly ConfigurationPropertyCollection InternalProperties;

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return InternalProperties;
            }
        }

        static TraceContextConfigurationSection()
        {
            InternalProperties = new ConfigurationPropertyCollection
                                     {
                                         contextElementCollectionProperty
                                     };
        }

        private static readonly ConfigurationProperty contextElementCollectionProperty =
            new ConfigurationProperty("", typeof(ContextElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        #endregion

        #region Element's collection: BasicMap of context
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ContextElementCollection Contexts
        {
            get
            {
                return (ContextElementCollection)base[contextElementCollectionProperty];
            }
        }
        #endregion

        #region ITraceContextConfigurator support
        public TraceContextConfiguration GetDefault()
        {
            var dictionary = new Dictionary<string, string>();
            var trace = ContextElement.TraceDefaultValue;
            var activity = ContextElement.ActivityDefaultValue;
            var bufferizeCatchExceptionAndFlash = ContextElement.BufferizeCatchExceptionAndFlashDefaultValue;
            foreach (ContextElement i in Contexts)
            {
                if (string.IsNullOrEmpty(i.Key))
                {
                    trace = i.Trace;
                    activity = i.Activity;
                    bufferizeCatchExceptionAndFlash = i.BufferizeCatchExceptionAndFlash;
                    foreach (RegisterConfigElement r in i.RegisterConfigs)
                    {
                        dictionary[r.Type] = r.PropertiesProperty;
                    }

                    break;
                }
            }

            return new TraceContextConfiguration 
            {
                Configs = dictionary, 
                Activity = activity,
                Trace = trace,
                BufferizeCatchExceptionAndFlash = bufferizeCatchExceptionAndFlash
            };
        }

        public TraceContextConfiguration GetDefault(string service, string method)
        {
            var dictionary = GetDefault();
            ContextElement methodContext = null;
            foreach (ContextElement i in Contexts)
            {
                if (i.Service == service && string.IsNullOrEmpty(i.Method))
                {
                    dictionary.Configs.Clear();
                    dictionary.Activity = i.Activity;
                    dictionary.Trace = i.Trace;
                    dictionary.BufferizeCatchExceptionAndFlash = i.BufferizeCatchExceptionAndFlash;
                    foreach (RegisterConfigElement r in i.RegisterConfigs)
                    {
                        dictionary.Configs[r.Type] = r.PropertiesProperty;
                    }
                }
                else if (i.Service == service && i.Method == method)
                {
                    methodContext = i;
                }
            }
            if (methodContext != null)
            {
                dictionary.Activity = methodContext.Activity;
                dictionary.Trace = methodContext.Trace;
                dictionary.BufferizeCatchExceptionAndFlash = methodContext.BufferizeCatchExceptionAndFlash;
                dictionary.Configs.Clear();
                foreach (RegisterConfigElement r in methodContext.RegisterConfigs)
                {
                    dictionary.Configs[r.Type] = r.PropertiesProperty;
                }
            }
            return dictionary;
        }
        #endregion

        #region Tracking & reporting configuration changes support

        public readonly static DateTime CreatedAtDateTime = DateTime.Now;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("TraceContextConfigurationSection (")
              .Append(CreatedAtDateTime.ToString(CultureInfo.InvariantCulture))
              .AppendLine(")");
            if (Contexts.Count != 0)
                sb.AppendLine(Contexts.ToString());
            return sb.ToString();
        }
        #endregion
    }
}
