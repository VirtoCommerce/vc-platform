using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klarna.PaymentGatewaysModule.Web.Exceptions
{
	[Serializable]
	public class KlarnaCredentialsException : Exception
	{
		public KlarnaCredentialsException() { }
		public KlarnaCredentialsException(string message) : base(message) { }
		public KlarnaCredentialsException(string message, Exception inner) : base(message, inner) { }
		protected KlarnaCredentialsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}