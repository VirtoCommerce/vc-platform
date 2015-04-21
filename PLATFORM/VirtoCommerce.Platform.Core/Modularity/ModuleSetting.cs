using System.Globalization;
using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ModuleSetting
    {
        public const string TypeBoolean = "boolean";
        public const string TypeInteger = "integer";
        public const string TypeDecimal = "decimal";
        public const string TypeString = "string";
        public const string TypeSecureString = "secureString";

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("valueType")]
        public string ValueType { get; set; }

        [XmlElement("defaultValue")]
        public string DefaultValue { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlArray("allowedValues")]
        [XmlArrayItem("value")]
        public string[] AllowedValues { get; set; }

        [XmlElement("isArray")]
        public bool IsArray { get; set; }

        [XmlArray("arrayValues")]
        [XmlArrayItem("value")]
        public string[] ArrayValues { get; set; }


        public object RawDefaultValue()
        {
            return RawValue(DefaultValue);
        }

        public object RawValue(string value)
        {
            return RawValue(ValueType, value);
        }

        public static object RawValue(string valueType, string value)
        {
            object result = null;

            if (value != null)
            {
                switch (valueType)
                {
                    case TypeBoolean:
                        result = bool.Parse(value);
                        break;
                    case TypeInteger:
                        result = int.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case TypeDecimal:
                        result = decimal.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case TypeString:
                    case TypeSecureString:
                        result = value;
                        break;
                }
            }

            return result;
        }
    }
}
