using System;
using System.Xml.Serialization;

namespace Shipstation.FulfillmentModule.Web.Models.Notice
{
    [Serializable]
    public class ShipNotice
    {

        private string orderNumberField;

        private string customerCodeField;

        private string labelCreateDateField;

        private string shipDateField;

        private string carrierField;

        private string serviceField;

        private string trackingNumberField;

        private string shippingCostField;

        private ShipNoticeRecipient recipientField;

        private ShipNoticeItemsItem[] itemsField;
        
        [XmlElement]
        public string OrderNumber
        {
            get
            {
                return this.orderNumberField;
            }
            set
            {
                this.orderNumberField = value;
            }
        }

        [XmlElement]
        public string CustomerCode
        {
            get
            {
                return this.customerCodeField;
            }
            set
            {
                this.customerCodeField = value;
            }
        }

        [XmlElement]
        public string LabelCreateDate
        {
            get
            {
                return this.labelCreateDateField;
            }
            set
            {
                this.labelCreateDateField = value;
            }
        }

        [XmlElement]
        public string ShipDate
        {
            get
            {
                return this.shipDateField;
            }
            set
            {
                this.shipDateField = value;
            }
        }

        [XmlElement]
        public string Carrier
        {
            get
            {
                return this.carrierField;
            }
            set
            {
                this.carrierField = value;
            }
        }

        [XmlElement]
        public string Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
            }
        }

        [XmlElement]
        public string TrackingNumber
        {
            get
            {
                return this.trackingNumberField;
            }
            set
            {
                this.trackingNumberField = value;
            }
        }

        [XmlElement]
        public string ShippingCost
        {
            get
            {
                return this.shippingCostField;
            }
            set
            {
                this.shippingCostField = value;
            }
        }

        [XmlElement("Recipient")]
        public ShipNoticeRecipient Recipient
        {
            get
            {
                return this.recipientField;
            }
            set
            {
                this.recipientField = value;
            }
        }

        [XmlArrayItem("Item", IsNullable = false)]
        public ShipNoticeItemsItem[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }
    
    [Serializable]
    public class ShipNoticeRecipient
    {
        private string nameField;

        private string companyField;

        private string address1Field;

        private string address2Field;

        private string cityField;

        private string stateField;

        private string postalCodeField;

        private string countryField;

        [XmlElement]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [XmlElement]
        public string Company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }

        [XmlElement]
        public string Address1
        {
            get
            {
                return this.address1Field;
            }
            set
            {
                this.address1Field = value;
            }
        }

        [XmlElement]
        public string Address2
        {
            get
            {
                return this.address2Field;
            }
            set
            {
                this.address2Field = value;
            }
        }

        [XmlElement]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        [XmlElement]
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        [XmlElement]
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        [XmlElement]
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }

    [Serializable]
    public class ShipNoticeItemsItem
    {

        private string sKUField;

        private string nameField;

        private string quantityField;

        [XmlElement]
        public string SKU
        {
            get
            {
                return this.sKUField;
            }
            set
            {
                this.sKUField = value;
            }
        }

        [XmlElement]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [XmlElement]
        public string Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }
    }

    [Serializable]
    public class NewDataSet
    {

        private ShipNotice[] itemsField;

        [XmlElement("ShipNotice")]
        public ShipNotice[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

}