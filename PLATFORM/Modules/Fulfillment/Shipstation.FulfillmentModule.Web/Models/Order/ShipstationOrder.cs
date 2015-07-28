using System;
using System.Xml;
using System.Xml.Serialization;

namespace Shipstation.FulfillmentModule.Web.Models.Order
{
    [Serializable]
    public class Orders
    {

        private OrdersOrder[] orderField;

        private short pagesField;

        private bool pagesFieldSpecified;

        [XmlElement("Order")]
        public OrdersOrder[] Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        [XmlAttribute]
        public short pages
        {
            get
            {
                return this.pagesField;
            }
            set
            {
                this.pagesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public bool pagesSpecified
        {
            get
            {
                return this.pagesFieldSpecified;
            }
            set
            {
                this.pagesFieldSpecified = value;
            }
        }
    }

    [Serializable]
    public class OrdersOrder
    {
        private string orderIDField;

        private string orderNumberField;

        private string orderDateField;

        private string orderStatusField;

        private string lastModifiedField;

        private string shippingMethodField;

        private string paymentMethodField;

        private float orderTotalField;

        private float taxAmountField;

        private bool taxAmountFieldSpecified;

        private float shippingAmountField;

        private bool shippingAmountFieldSpecified;

        private string customerNotesField;

        private string internalNotesField;

        private bool giftField;

        private bool giftFieldSpecified;

        private string giftMessageField;

        private string customField1Field;

        private string customField2Field;

        private string customField3Field;

        private OrdersOrderCustomer customerField;

        private OrdersOrderItem[] itemsField;

        [XmlElement("OrderID")]
        public XmlCDataSection OrderIDCDATA
        {
            get
            {
                return OrderID == null ? null : new XmlDocument().CreateCDataSection(OrderID);
            }
            set
            {
                OrderID = value.Value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string OrderID
        {
            get
            {
                return this.orderIDField;
            }
            set
            {
                this.orderIDField = value;
            }
        }

        [XmlElement("OrderNumber")]
        public XmlCDataSection OrderNumberCDATA
        {
            get
            {
                return OrderNumber == null ? null : new XmlDocument().CreateCDataSection(OrderNumber);
            }
            set
            {
                OrderNumber = value.Value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
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

        public string OrderDate
        {
            get
            {
                return this.orderDateField;
            }
            set
            {
                this.orderDateField = value;
            }
        }

        [XmlElement("OrderStatus")]
        public XmlCDataSection OrderStatusCDATA
        {
            get
            {
                return OrderStatus == null ? null : new XmlDocument().CreateCDataSection(OrderStatus);
            }
            set
            {
                OrderStatus = value.Value;
            }
        }

        [XmlIgnore]
        public string OrderStatus
        {
            get
            {
                return this.orderStatusField;
            }
            set
            {
                this.orderStatusField = value;
            }
        }

        public string LastModified
        {
            get
            {
                return this.lastModifiedField;
            }
            set
            {
                this.lastModifiedField = value;
            }
        }

        [XmlElement("ShippingMethod")]
        public XmlCDataSection ShippingMethodCDATA
        {
            get
            {
                return ShippingMethod == null ? null : new XmlDocument().CreateCDataSection(ShippingMethod);
            }
            set
            {
                ShippingMethod = value.Value;
            }
        }

        [XmlIgnore]
        public string ShippingMethod
        {
            get
            {
                return this.shippingMethodField;
            }
            set
            {
                this.shippingMethodField = value;
            }
        }

        [XmlElement("PaymentMethod")]
        public XmlCDataSection PaymentMethodCDATA
        {
            get
            {
                return PaymentMethod == null ? null : new XmlDocument().CreateCDataSection(PaymentMethod);
            }
            set
            {
                PaymentMethod = value.Value;
            }
        }

        [XmlIgnore]
        public string PaymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
            }
        }

        public float OrderTotal
        {
            get
            {
                return this.orderTotalField;
            }
            set
            {
                this.orderTotalField = value;
            }
        }

        public float TaxAmount
        {
            get
            {
                return this.taxAmountField;
            }
            set
            {
                this.taxAmountField = value;
            }
        }

        [XmlIgnore]
        public bool TaxAmountSpecified
        {
            get
            {
                return this.taxAmountFieldSpecified;
            }
            set
            {
                this.taxAmountFieldSpecified = value;
            }
        }

        public float ShippingAmount
        {
            get
            {
                return this.shippingAmountField;
            }
            set
            {
                this.shippingAmountField = value;
            }
        }

        [XmlIgnore]
        public bool ShippingAmountSpecified
        {
            get
            {
                return this.shippingAmountFieldSpecified;
            }
            set
            {
                this.shippingAmountFieldSpecified = value;
            }
        }

        [XmlElement("CustomerNotes")]
        public XmlCDataSection CustomerNotesCDATA
        {
            get
            {
                return CustomerNotes == null ? null : new XmlDocument().CreateCDataSection(CustomerNotes);
            }
            set
            {
                CustomerNotes = value.Value;
            }
        }

        [XmlIgnore]
        public string CustomerNotes
        {
            get
            {
                return this.customerNotesField;
            }
            set
            {
                this.customerNotesField = value;
            }
        }

        [XmlElement("InternalNotes")]
        public XmlCDataSection InternalNotesCDATA
        {
            get
            {
                return InternalNotes == null ? null : new XmlDocument().CreateCDataSection(InternalNotes);
            }
            set
            {
                InternalNotes = value.Value;
            }
        }

        [XmlIgnore]
        public string InternalNotes
        {
            get
            {
                return this.internalNotesField;
            }
            set
            {
                this.internalNotesField = value;
            }
        }

        
        public bool Gift
        {
            get
            {
                return this.giftField;
            }
            set
            {
                this.giftField = value;
            }
        }
        
        [XmlIgnore]
        public bool GiftSpecified
        {
            get
            {
                return this.giftFieldSpecified;
            }
            set
            {
                this.giftFieldSpecified = value;
            }
        }

        [XmlElement("GiftMessage")]
        public XmlCDataSection GiftMessageCDATA
        {
            get
            {
                return GiftMessage == null ? null : new XmlDocument().CreateCDataSection(GiftMessage);
            }
            set
            {
                GiftMessage = value.Value;
            }
        }

        [XmlIgnore]
        public string GiftMessage
        {
            get
            {
                return this.giftMessageField;
            }
            set
            {
                this.giftMessageField = value;
            }
        }

        [XmlElement("CustomField1")]
        public XmlCDataSection CustomField1CDATA
        {
            get
            {
                return CustomField1 == null ? null : new XmlDocument().CreateCDataSection(CustomField1);
            }
            set
            {
                CustomField1 = value.Value;
            }
        }

        [XmlIgnore]
        public string CustomField1
        {
            get
            {
                return this.customField1Field;
            }
            set
            {
                this.customField1Field = value;
            }
        }

        [XmlElement("CustomField2")]
        public XmlCDataSection CustomField2CDATA
        {
            get
            {
                return CustomField2 == null ? null : new XmlDocument().CreateCDataSection(CustomField2);
            }
            set
            {
                CustomField2 = value.Value;
            }
        }

        [XmlIgnore]
        public string CustomField2
        {
            get
            {
                return this.customField2Field;
            }
            set
            {
                this.customField2Field = value;
            }
        }

        [XmlElement("CustomField3")]
        public XmlCDataSection CustomField3CDATA
        {
            get
            {
                return CustomField3 == null ? null : new XmlDocument().CreateCDataSection(CustomField3);
            }
            set
            {
                CustomField3 = value.Value;
            }
        }

        [XmlIgnore]
        public string CustomField3
        {
            get
            {
                return this.customField3Field;
            }
            set
            {
                this.customField3Field = value;
            }
        }
        
        public OrdersOrderCustomer Customer
        {
            get
            {
                return this.customerField;
            }
            set
            {
                this.customerField = value;
            }
        }

        [XmlArrayItem("Item")]
        public OrdersOrderItem[] Items
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
    public class OrdersOrderCustomer
    {
        private string customerCodeField;

        private OrdersOrderCustomerBillTo billToField;

        private OrdersOrderCustomerShipTo shipToField;

        [XmlElement("CustomerCode")]
        public XmlCDataSection CustomerCodeCDATA
        {
            get
            {
                return CustomerCode == null ? null : new XmlDocument().CreateCDataSection(CustomerCode);
            }
            set
            {
                CustomerCode = value.Value;
            }
        }

        [XmlIgnore]
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

        public OrdersOrderCustomerBillTo BillTo
        {
            get
            {
                return this.billToField;
            }
            set
            {
                this.billToField = value;
            }
        }

        public OrdersOrderCustomerShipTo ShipTo
        {
            get
            {
                return this.shipToField;
            }
            set
            {
                this.shipToField = value;
            }
        }
    }

    [Serializable]
    public class OrdersOrderCustomerBillTo
    {
        private string nameField;

        private string companyField;

        private string phoneField;

        private string emailField;

        [XmlElement("Name")]
        public XmlCDataSection NameCDATA
        {
            get
            {
                return Name == null ? null : new XmlDocument().CreateCDataSection(Name);
            }
            set
            {
                Name = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Company")]
        public XmlCDataSection CompanyCDATA
        {
            get
            {
                return Company == null ? null : new XmlDocument().CreateCDataSection(Company);
            }
            set
            {
                Company = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Phone")]
        public XmlCDataSection PhoneCDATA
        {
            get
            {
                return Phone == null ? null : new XmlDocument().CreateCDataSection(Phone);
            }
            set
            {
                Phone = value.Value;
            }
        }

        [XmlIgnore]
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        [XmlElement("Email")]
        public XmlCDataSection EmailCDATA
        {
            get
            {
                return Email == null ? null : new XmlDocument().CreateCDataSection(Email);
            }
            set
            {
                Email = value.Value;
            }
        }

        [XmlIgnore]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }
    
    [Serializable]
    public class OrdersOrderCustomerShipTo
    {

        private string nameField;

        private string companyField;

        private string address1Field;

        private string address2Field;

        private string cityField;

        private string stateField;

        private string postalCodeField;

        private string countryField;

        private string phoneField;

        [XmlElement("Name")]
        public XmlCDataSection NameCDATA
        {
            get
            {
                return Name == null ? null : new XmlDocument().CreateCDataSection(Name);
            }
            set
            {
                Name = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Company")]
        public XmlCDataSection CompanyCDATA
        {
            get
            {
                return Company == null ? null : new XmlDocument().CreateCDataSection(Company);
            }
            set
            {
                Company = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Phone")]
        public XmlCDataSection PhoneCDATA
        {
            get
            {
                return Phone == null ? null : new XmlDocument().CreateCDataSection(Phone);
            }
            set
            {
                Phone = value.Value;
            }
        }

        [XmlIgnore]
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        [XmlElement("Address1")]
        public XmlCDataSection Address1CDATA
        {
            get
            {
                return Address1 == null ? null : new XmlDocument().CreateCDataSection(Address1);
            }
            set
            {
                Address1 = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Address2")]
        public XmlCDataSection Address2CDATA
        {
            get
            {
                return Address2 == null ? null : new XmlDocument().CreateCDataSection(Address2);
            }
            set
            {
                Address2 = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("City")]
        public XmlCDataSection CityCDATA
        {
            get
            {
                return City == null ? null : new XmlDocument().CreateCDataSection(City);
            }
            set
            {
                City = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("State")]
        public XmlCDataSection StateCDATA
        {
            get
            {
                return State == null ? null : new XmlDocument().CreateCDataSection(State);
            }
            set
            {
                State = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("PostalCode")]
        public XmlCDataSection PostalCodeCDATA
        {
            get
            {
                return PostalCode == null ? null : new XmlDocument().CreateCDataSection(PostalCode);
            }
            set
            {
                PostalCode = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Country")]
        public XmlCDataSection CountryCDATA
        {
            get
            {
                return Country == null ? null : new XmlDocument().CreateCDataSection(Country);
            }
            set
            {
                Country = value.Value;
            }
        }

        [XmlIgnore]
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
    public class OrdersOrderItem
    {
        private string lineItemIDField;

        private string sKUField;

        private string nameField;

        private bool adjustmentField;

        private bool adjustmentFieldSpecified;

        private string imageUrlField;

        private float weightField;

        private bool weightFieldSpecified;

        private string weightUnitsField;

        private sbyte quantityField;

        private float unitPriceField;

        private string locationField;

        private OrdersOrderItemOption[] optionsField;

        [XmlElement("LineItemID")]
        public XmlCDataSection LineItemIDCDATA
        {
            get
            {
                return LineItemID == null ? null : new XmlDocument().CreateCDataSection(LineItemID);
            }
            set
            {
                LineItemID = value.Value;
            }
        }

        [XmlIgnore]
        public string LineItemID
        {
            get
            {
                return this.lineItemIDField;
            }
            set
            {
                this.lineItemIDField = value;
            }
        }

        [XmlElement("SKU")]
        public XmlCDataSection SKUCDATA
        {
            get
            {
                return SKU == null ? null : new XmlDocument().CreateCDataSection(SKU);
            }
            set
            {
                SKU = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Name")]
        public XmlCDataSection NameCDATA
        {
            get
            {
                return Name == null ? null : new XmlDocument().CreateCDataSection(Name);
            }
            set
            {
                Name = value.Value;
            }
        }

        [XmlIgnore]
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

        public bool Adjustment
        {
            get
            {
                return this.adjustmentField;
            }
            set
            {
                this.adjustmentField = value;
            }
        }

        [XmlIgnore]
        public bool AdjustmentSpecified
        {
            get
            {
                return this.adjustmentFieldSpecified;
            }
            set
            {
                this.adjustmentFieldSpecified = value;
            }
        }

        [XmlElement("ImageUrl")]
        public XmlCDataSection ImageUrlCDATA
        {
            get
            {
                return ImageUrl == null ? null : new XmlDocument().CreateCDataSection(ImageUrl);
            }
            set
            {
                ImageUrl = value.Value;
            }
        }

        [XmlIgnore]
        public string ImageUrl
        {
            get
            {
                return this.imageUrlField;
            }
            set
            {
                this.imageUrlField = value;
            }
        }

        public float Weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        [XmlIgnore]
        public bool WeightSpecified
        {
            get
            {
                return this.weightFieldSpecified;
            }
            set
            {
                this.weightFieldSpecified = value;
            }
        }

        public string WeightUnits
        {
            get
            {
                return this.weightUnitsField;
            }
            set
            {
                this.weightUnitsField = value;
            }
        }

        public sbyte Quantity
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

        public float UnitPrice
        {
            get
            {
                return this.unitPriceField;
            }
            set
            {
                this.unitPriceField = value;
            }
        }

        [XmlElement("Location")]
        public XmlCDataSection LocationCDATA
        {
            get
            {
                return Location == null ? null : new XmlDocument().CreateCDataSection(Location);
            }
            set
            {
                Location = value.Value;
            }
        }

        [XmlIgnore]
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        [XmlArrayItem("Option")]
        public OrdersOrderItemOption[] Options
        {
            get
            {
                return this.optionsField;
            }
            set
            {
                this.optionsField = value;
            }
        }
    }

    [Serializable]
    public class OrdersOrderItemOption
    {

        private string nameField;

        private string valueField;

        private float weightField;

        private bool weightFieldSpecified;

        [XmlElement("Name")]
        public XmlCDataSection NameCDATA
        {
            get
            {
                return Name == null ? null : new XmlDocument().CreateCDataSection(Name);
            }
            set
            {
                Name = value.Value;
            }
        }

        [XmlIgnore]
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

        [XmlElement("Value")]
        public XmlCDataSection ValueCDATA
        {
            get
            {
                return Value == null ? null : new XmlDocument().CreateCDataSection(Value);
            }
            set
            {
                Value = value.Value;
            }
        }

        [XmlIgnore]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        public float Weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        [XmlIgnore]
        public bool WeightSpecified
        {
            get
            {
                return this.weightFieldSpecified;
            }
            set
            {
                this.weightFieldSpecified = value;
            }
        }
    }
}