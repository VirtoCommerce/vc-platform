using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klarna.PaymentGatewaysModule.Web.Exceptions
{
	[Serializable]
	public class KlarnaActivateException : Exception
	{
		public KlarnaActivateException() { }
		public KlarnaActivateException(string message) : base(message) { }
		public KlarnaActivateException(string message, Exception inner) : base(message, inner) { }
		protected KlarnaActivateException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}