using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Core.DataContracts
{
    /// <summary>
    /// The error object that will be returned back to the caller
    /// </summary>
    [DataContract(Name = "Error", Namespace = "http://schemas.virtocommerce.com/2.0")]
    public class ManagementServiceError
    {
        /// <summary>
        /// Gets or sets the error code. 
        /// </summary>
        [DataMember(Order = 0)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the message. 
        /// </summary>
        [DataMember(Order = 1)]
        public string Message { get; set; }

        /// <summary>
        /// Gets the list of error details. 
        /// </summary>
        [DataMember(Order = 2)]
        public List<ErrorDetail> Details { get; set; }
    }
}
