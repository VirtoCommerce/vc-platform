using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Orders.Exceptions
{
    public class PaymentException : OrderException
    {
        /// <summary>
        /// Represents error type.
        /// </summary>
        public struct ErrorType
        {
            public static string ConnectionFailed = "connection-failed";
            public static string ConfigurationError = "configuration-error";
            public static string ProviderError = "provider-error";
			public static string PaymentTotalError = "payment-total-error";
        }

        private string _ErrorCode = String.Empty;

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public string ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }

        private string _ErrorType = String.Empty;

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public string Type
        {
            get { return _ErrorType; }
            set { _ErrorType = value; }
        }


        private Dictionary<string, string> _ResponseMessages = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the response messages.
        /// </summary>
        /// <value>The response messages.</value>
        public Dictionary<string, string> ResponseMessages
        {
            get { return _ResponseMessages; }
            set { _ResponseMessages = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        public PaymentException(string type, string code, string message)
            : base(message)
        {
            _ErrorCode = code;
            _ErrorType = type;
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>
        /// A string representation of the current exception.
        /// </returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override string ToString()
        {
            string message = base.ToString();
            message = String.Format("{0}. \n\r Error Code: {1}. \n\r Error Type: {2}.", message, this.ErrorCode, this.Type);

            if (ResponseMessages != null && ResponseMessages.Count > 0)
            {
                message += String.Format("\n\r Messages:");
                foreach (string key in ResponseMessages.Keys)
                    message += String.Format("\n\r{0}:{1}", key, ResponseMessages[key]);
            }

            return message;
        }
    }

}
