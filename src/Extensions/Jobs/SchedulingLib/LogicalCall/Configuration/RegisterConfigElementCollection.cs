using System;
using System.Configuration;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall.Configuration
{
    [ConfigurationCollection(typeof(RegisterConfigElement), AddItemName = "registerConfig", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class RegisterConfigElementCollection : ConfigurationElementCollection
    {
        #region ConfigurationElementCollection Support
        static RegisterConfigElementCollection()
        {
            configurationPropertyCollection = new ConfigurationPropertyCollection();
        }

        private static readonly ConfigurationPropertyCollection configurationPropertyCollection;

        protected override ConfigurationPropertyCollection Properties
        {
            get { return configurationPropertyCollection; }
        }

        
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public RegisterConfigElement this[int index]
        {
            get
            {
                return (RegisterConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public new RegisterConfigElement this[string name]
        {
            get
            {
                return (RegisterConfigElement)BaseGet(name);
            }
        }

        protected override string ElementName
        {
            get
            {
                return "registerConfig";
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RegisterConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RegisterConfigElement)element).Key;
        }
        #endregion

        #region Tracking & reporting configuration changes support
        public readonly static DateTime CreatedAtDateTime = DateTime.Now;
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (RegisterConfigElement x in this)
                sb.Append(x);
            return sb.ToString();
        }
        #endregion
    }
}