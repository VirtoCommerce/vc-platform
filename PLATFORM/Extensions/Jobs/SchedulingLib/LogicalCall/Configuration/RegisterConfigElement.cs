using System;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall.Configuration
{
    public class RegisterConfigElement : ConfigurationElement
    {
        #region ConfigurationElement support
        private static readonly ConfigurationPropertyCollection internalProperties;

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return internalProperties;
            }
        }

        static RegisterConfigElement()
        {
            internalProperties = new ConfigurationPropertyCollection
                {
                    typeProperty,
                    propertiesProperty
                };
        }

        private static readonly ConfigurationProperty typeProperty =
            new ConfigurationProperty("type",
                                      typeof(string), "",
                                      ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty propertiesProperty =
            new ConfigurationProperty("properties",
                                      typeof(string), "",
                                      ConfigurationPropertyOptions.IsRequired);
        #endregion

        #region Element properties: type, properties

        [ConfigurationProperty("type")]
        public string Type
        {
            get
            {
                return this["type"] as string;
            }
            set
            {
                this["type"] = value;
            }
        }

        [ConfigurationProperty("properties")]
        public string PropertiesProperty
        {
            get
            {
                return this["properties"] as string;
            }
            set
            {
                this["properties"] = value;
            }
        }
        #endregion

        #region RegisterConfigElementCollection support
        private string key = Guid.NewGuid().ToString();
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        #endregion

        #region Tracking & reporting configuration changes support
        public readonly static DateTime CreatedAtDateTime = DateTime.Now;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("RegisterConfig (").Append(CreatedAtDateTime.ToString(CultureInfo.InvariantCulture)).Append(")");
            sb.Append(" type=").Append(this.Type).Append(" properties=\"").Append(this.PropertiesProperty).Append("\"");
            return sb.ToString();
        }
        #endregion

    }
}