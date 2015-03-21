using System;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Reviews.Model
{
	[Serializable]
	public class ReviewException : Exception
	{
		public ReviewException()
			: base()
		{
		}

		public ReviewException(string message)
			: base(message)
		{
		}

		public ReviewException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ReviewException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
