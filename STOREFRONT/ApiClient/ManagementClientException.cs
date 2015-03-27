#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.ApiClient
{

    #region

    #endregion

    /// <summary>
    ///     Exception class that could be thrown within the client library
    /// </summary>
    [Serializable]
    public class ManagementClientException : Exception
    {
        // Names used in serialization.

        #region Constants

        private const string ClientErrorCodeName = "ErrorCode";

        private const string ClientStatusCodeName = "StatusCode";

        private const string ErrorDetailsName = "Details";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the ManagementClientException class.
        /// </summary>
        /// <param name="statusCode">http status code of the failed request</param>
        /// <param name="errorCode">The detailed error code.</param>
        /// <param name="message">error message</param>
        public ManagementClientException(HttpStatusCode statusCode, string errorCode, string message)
            : this(statusCode, errorCode, message, null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ManagementClientException class.
        /// </summary>
        /// <param name="statusCode">http status code of the failed request</param>
        /// <param name="errorCode">The detailed error code.</param>
        /// <param name="message">error message</param>
        /// <param name="details">error details</param>
        public ManagementClientException(
            HttpStatusCode statusCode,
            string errorCode,
            string message,
            List<ErrorDetail> details)
            : this(statusCode, errorCode, message, details, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ManagementClientException class.
        /// </summary>
        /// <param name="statusCode">http status code of the failed request</param>
        /// <param name="errorCode">a more detailed error code</param>
        /// <param name="message">error message</param>
        /// <param name="details">Error details</param>
        /// <param name="innerException">Inner exception.</param>
        public ManagementClientException(
            HttpStatusCode statusCode,
            string errorCode,
            string message,
            List<ErrorDetail> details,
            Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Details = details;
        }

        /// <summary>
        ///     Initializes a new instance of the ManagementClientException class.
        /// </summary>
        /// <param name="serializationInfo">serialization information</param>
        /// <param name="context">streaming context</param>
        protected ManagementClientException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            if (serializationInfo != null)
            {
                StatusCode = (HttpStatusCode)serializationInfo.GetInt32(ClientStatusCodeName);
                ErrorCode = serializationInfo.GetString(ClientErrorCodeName);
                Details =
                    (List<ErrorDetail>)serializationInfo.GetValue(ErrorDetailsName, typeof(List<ErrorDetail>));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the error details property
        /// </summary>
        public List<ErrorDetail> Details { get; private set; }

        /// <summary>
        ///     Gets the error code property
        /// </summary>
        public string ErrorCode { get; private set; }

        /// <summary>
        ///     Gets the status code property
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Required override to add in the serialized parameters
        /// </summary>
        /// <param name="info">serialization information</param>
        /// <param name="context">streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(ClientStatusCodeName, (int)StatusCode);
            info.AddValue(ClientErrorCodeName, ErrorCode);
            info.AddValue(ErrorDetailsName, Details);

            base.GetObjectData(info, context);
        }

        /// <summary>
        ///     Required override to properly display the exception extension properties if traced
        /// </summary>
        /// <returns>string representing the exception including the base and extended properties</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "HttpStatusCode: {0}, ErrorCode: {1}, {2}",
                    StatusCode.ToString(),
                    ErrorCode,
                    base.ToString()));

            if (Details != null && Details.Count > 0)
            {
                foreach (var detail in Details)
                {
                    builder.Append(Environment.NewLine);
                    builder.Append(string.Format(CultureInfo.InvariantCulture, "Detail: '{0}'", detail));
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}
