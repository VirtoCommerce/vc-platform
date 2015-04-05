using System;
using System.Configuration;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall.Configuration
{
    [ConfigurationCollection(typeof(ContextElement), AddItemName = "context", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ContextElementCollection : ConfigurationElementCollection
    {
        #region ConfigurationElementCollection support
        static ContextElementCollection()
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


        public ContextElement this[int index]
        {
            get
            {
                return (ContextElement)BaseGet(index);
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

        public new ContextElement this[string name]
        {
            get
            {
                return (ContextElement)BaseGet(name);
            }
        }

        protected override string ElementName
        {
            get
            {
                return "context";
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ContextElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ContextElement)element).Key;
        }
        #endregion

        #region Tracking & reporting configuration changes support
        public readonly static DateTime CreatedAtDateTime = DateTime.Now;

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (ContextElement x in this)
               sb.Append(x);
            return sb.ToString();
        }
        #endregion

    }
}