using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Orders.Exceptions
{
    [Serializable]
    public class OrderException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderException"/> class.
        /// </summary>
        public OrderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public OrderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public OrderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected OrderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

}
