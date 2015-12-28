using System;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Order
{
    /// <summary>
    /// Represent customer order
    /// </summary>
    public class CustomerOrder
    {
        public CustomerOrder()
        {
            Addresses = new List<Address>();
            InPayments = new List<PaymentIn>();
            Items = new List<LineItem>();
            TaxDetails = new List<TaxDetail>();
            ChildrenOperations = new List<Operation>();
            DynamicProperties = new List<DynamicProperty>();
        }

        /// <summary>
        /// Gets or Sets CustomerName
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or Sets CustomerId
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Chanel (Web site, mobile application etc)
        /// </summary>
        /// <value>Chanel (Web site, mobile application etc)</value>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or Sets StoreId
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or Sets StoreName
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Gets or Sets OrganizationName
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Gets or Sets OrganizationId
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Employee who should handle that order
        /// </summary>
        /// <value>Employee who should handle that order</value>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or Sets EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or Sets DiscountAmount
        /// </summary>
        public Money DiscountAmount { get; set; }


        /// <summary>
        /// All shipping and billing order addresses
        /// </summary>
        /// <value>All shipping and billing order addresses</value>
        public ICollection<Address> Addresses { get; set; }

        /// <summary>
        /// Incoming payments operations
        /// </summary>
        /// <value>Incoming payments operations</value>
        public ICollection<PaymentIn> InPayments { get; set; }

        /// <summary>
        /// All customer order line items
        /// </summary>
        /// <value>All customer order line items</value>
        public ICollection<LineItem> Items { get; set; }

        /// <summary>
        /// All customer order shipments
        /// </summary>
        /// <value>All customer order shipments</value>
        public List<Shipment> Shipments { get; set; }

        /// <summary>
        /// All customer order discount
        /// </summary>
        /// <value>All customer order discount</value>
        public Discount Discount { get; set; }

        /// <summary>
        /// Tax details
        /// </summary>
        /// <value>Tax details</value>
        public ICollection<TaxDetail> TaxDetails { get; set; }

        /// <summary>
        /// Security permission scopes used for security check on UI
        /// </summary>
        /// <value>Security permission scopes used for security check on UI</value>
        public ICollection<string> Scopes { get; set; }

        /// <summary>
        /// Operation type string representation (CustomerOrder, Shipment etc)
        /// </summary>
        /// <value>Operation type string representation (CustomerOrder, Shipment etc)</value>
        public string OperationType { get; set; }

        /// <summary>
        /// Unique user friendly document number (generate automatically based on special algorithm realization)
        /// </summary>
        /// <value>Unique user friendly document number (generate automatically based on special algorithm realization)</value>
        public string Number { get; set; }

        /// <summary>
        /// Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status\r\n            (Order completion, Shipment send etc)
        /// </summary>
        /// <value>Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status\r\n            (Order completion, Shipment send etc)</value>
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Current operation status may have any values defined by concrete business process
        /// </summary>
        /// <value>Current operation status may have any values defined by concrete business process</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Currecy code
        /// </summary>
        /// <value>Currecy code</value>
        public Currency Currency { get; set; }

        /// <summary>
        /// Gets or Sets TaxIncluded
        /// </summary>
        public bool? TaxIncluded { get; set; }

        /// <summary>
        /// Money amount without tax
        /// </summary>
        /// <value>Money amount without tax</value>
        public Money Sum { get; set; }

        /// <summary>
        /// Tax total
        /// </summary>
        /// <value>Tax total</value>
        public Money Tax { get; set; }

        /// <summary>
        /// Gets or Sets IsCancelled
        /// </summary>
        public bool? IsCancelled { get; set; }

        /// <summary>
        /// Gets or Sets CancelledDate
        /// </summary>
        public DateTime? CancelledDate { get; set; }

        /// <summary>
        /// Gets or Sets CancelReason
        /// </summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// Used for construct hierarchy of operation and represent parent operation id
        /// </summary>
        /// <value>Used for construct hierarchy of operation and represent parent operation id</value>
        public string ParentOperationId { get; set; }

        /// <summary>
        /// Gets or Sets ChildrenOperations
        /// </summary>
        public ICollection<Operation> ChildrenOperations { get; set; }

        /// <summary>
        /// Used for dynamic properties management, contains object type string
        /// </summary>
        /// <value>Used for dynamic properties management, contains object type string</value>
        public string ObjectType { get; set; }

        /// <summary>
        /// Dynamic properties collections
        /// </summary>
        /// <value>Dynamic properties collections</value>
        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public string Id { get; set; }
    }
}