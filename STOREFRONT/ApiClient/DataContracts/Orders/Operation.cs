using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class Operation
    {
        #region Public Properties

        public string CancelReason { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Currency { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCancelled { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string Number { get; set; }

        public string Status { get; set; }

        public decimal Sum { get; set; }

        public decimal Tax { get; set; }
        public bool TaxIncluded { get; set; }

        public ICollection<OperationProperty> Properties { get; set; }

        #endregion
    }
}
