#region

using System.Globalization;
using System.Runtime.Serialization;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

    /// <summary>
    ///     The details of an error
    /// </summary>
    [DataContract(Name = "Detail", Namespace = "http://schemas.virtocommerce.com/2.0")]
    public class ErrorDetail
    {
        #region Public Properties

        /// <summary>
        ///     Gets the error code.
        /// </summary>
        [DataMember(Order = 0)]
        public string Code { get; set; }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        [DataMember(Order = 2)]
        public string Message { get; set; }

        /// <summary>
        ///     Gets the error source.
        /// </summary>
        [DataMember(Order = 1)]
        public string Source { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Required override to properly display the error detail
        /// </summary>
        /// <returns>string representing the error detail</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Code: {0}, Source: {1}, Message: {2}",
                Code,
                Source,
                Message);
        }

        #endregion
    }
}
