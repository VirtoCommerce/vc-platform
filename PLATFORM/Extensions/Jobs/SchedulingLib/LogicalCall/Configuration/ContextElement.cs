using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall.Configuration
{
    public class ContextElement : ConfigurationElement
    {
        public const bool TraceDefaultValue = true;
        public const bool ActivityDefaultValue = true;
        public const bool BufferizeCatchExceptionAndFlashDefaultValue = false;

        #region ConfigurationElement support
        private static readonly ConfigurationPropertyCollection internalProperties;

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return internalProperties;
            }
        }

        static ContextElement()
        {
            internalProperties = new ConfigurationPropertyCollection
                {
                    serviceProperty,
                    methodProperty,
                    activityProperty,
                    traceProperty,
                    bufferizeCatchExceptionAndFlashProperty,
                    registerElementCollectionProperty
                };
        }


        private static readonly ConfigurationProperty serviceProperty =
            new ConfigurationProperty("service",
                                      typeof(string), "",
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty methodProperty =
            new ConfigurationProperty("method",
                                      typeof(string), "",
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty activityProperty =
            new ConfigurationProperty("activity",
                                      typeof(bool), ActivityDefaultValue,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty traceProperty =
            new ConfigurationProperty("trace",
                                      typeof(bool), TraceDefaultValue,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty bufferizeCatchExceptionAndFlashProperty =
            new ConfigurationProperty("bufferizeCatchExceptionAndFlash",
                                      typeof(bool), BufferizeCatchExceptionAndFlashDefaultValue,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty registerElementCollectionProperty = new ConfigurationProperty(
            "",
            typeof(RegisterConfigElementCollection),
            null,
            ConfigurationPropertyOptions.IsDefaultCollection
            );
        #endregion

        #region Validation
        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            Validate();
        }

        private void Validate()
        {

            if (string.IsNullOrEmpty(this.Service) && !string.IsNullOrEmpty(this.Method))
                throw new ApplicationException(
                    "method without service in the configuration (you are allowen only to point both, only service or neither)");

        }
        #endregion

        #region ContextElementCollection support
        public string Key
        {
            get
            {
                string method = Method;
                return Service + (string.IsNullOrEmpty(method) ? "" : "." + Method);
            }
        }
        #endregion


        #region Element properties : Service, Method, Activity, Trace, BufferizeCatchExceptionAndFlash; and RegisterConfig collection

        [ConfigurationProperty("service")]
        public string Service
        {
            get
            {
                return this["service"] as string;
            }
            set
            {
                this["service"] = value;
            }
        }

        [StringValidator(InvalidCharacters = @"~!.@#$%^&*()[]{};'\|\\")]
        [ConfigurationProperty("method")]
        public string Method
        {
            get
            {
                return this["method"] as string;
            }
            set
            {
                this["method"] = value;
            }
        }

        [ConfigurationProperty("activity")]
        public bool Activity
        {
            get
            {
                return (bool)this["activity"];
            }
            set
            {
                this["activity"] = value;
            }
        }

        [ConfigurationProperty("trace")]
        public bool Trace
        {
            get
            {
                return (bool)this["trace"];
            }
            set
            {
                this["trace"] = value;
            }
        }

        [ConfigurationProperty("bufferizeCatchExceptionAndFlash")]
        public bool BufferizeCatchExceptionAndFlash
        {
            get
            {
                return (bool)this["bufferizeCatchExceptionAndFlash"];
            }
            set
            {
                this["bufferizeCatchExceptionAndFlash"] = value;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public RegisterConfigElementCollection RegisterConfigs
        {
            get { return (RegisterConfigElementCollection)base[""]; }
        }

        #endregion

        #region Tracking & reporting configuration changes support
        public readonly static DateTime CreatedAtDateTime = DateTime.Now;


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Context (").Append(CreatedAtDateTime.ToString(CultureInfo.InvariantCulture)).Append(") ");
            var properties = new Dictionary<string, string>();
            foreach (ConfigurationProperty i in internalProperties)
                if (i != registerElementCollectionProperty)
                    if (this[i.Name]!=i.DefaultValue)
                        properties.Add(i.Name, this[i.Name].ToString());
            sb.AppendLine(PairsParser.Encode(properties));
            if (RegisterConfigs.Count != 0)
                sb.AppendLine(RegisterConfigs.ToString());
            return sb.ToString();
        }
        #endregion
    }
}