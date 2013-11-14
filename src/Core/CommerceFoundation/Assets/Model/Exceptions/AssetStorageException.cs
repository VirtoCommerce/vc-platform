using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Assets.Model.Exceptions
{
	[Serializable]
	public class AssetStorageException : Exception
	{
		public AssetErrorCode ErrorCode { get; private set; }
		public AssetStorageException()
		{
		}

		public AssetStorageException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public AssetStorageException(string message, AssetErrorCode errorCode)
			: base(message)
		{
			ErrorCode = errorCode;
		}

		public AssetStorageException(string message, AssetErrorCode errorCode, Exception inner)
			: base(message)
		{
			ErrorCode = errorCode;
		}

		protected AssetStorageException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}
