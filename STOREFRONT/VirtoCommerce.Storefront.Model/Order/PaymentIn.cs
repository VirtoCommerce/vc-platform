using System;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Order
{
    /// <summary>
    /// Represent incoming payment operation
    /// </summary>
    public class PaymentIn
    {
        public PaymentIn()
        {
            ChildrenOperations = new List<Operation>();
            DynamicProperties = new List<DynamicProperty>();
        }

        /// <summary>
        /// Customer organization
        /// </summary>
        /// <value>Customer organization</value>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Gets or Sets OrganizationId
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or Sets CustomerName
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or Sets CustomerId
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Payment purpose text
        /// </summary>
        /// <value>Payment purpose text</value>
        public string Purpose { get; set; }

        /// <summary>
        /// Payment gateway code used for link with gateway provider realization
        /// </summary>
        /// <value>Payment gateway code used for link with gateway provider realization</value>
        public string GatewayCode { get; set; }

        /// <summary>
        /// Expected date of receipt of payment
        /// </summary>
        /// <value>Expected date of receipt of payment</value>
        public DateTime? IncomingDate { get; set; }

        /// <summary>
        /// Outer id used for link with payment in external systems
        /// </summary>
        /// <value>Outer id used for link with payment in external systems</value>
        public string OuterId { get; set; }

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
        /// Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status
        /// (Order completion, Shipment send etc)
        /// </summary>
        /// <value>Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status
        /// (Order completion, Shipment send etc)
        /// </value>
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