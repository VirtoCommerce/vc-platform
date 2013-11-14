using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Messages
{
	[DataContract]
	public sealed class MessageReference
	{
		[DataMember]
		public string StorageReference;
		[DataMember]
		public string StorageContainer;
		[DataMember]
		public string MessageTypeName;
		[DataMember]
		public IMessage Message;


		public MessageReference()
		{
		}

		public MessageReference(IMessage message)
		{
			Message = message;
		}
		public MessageReference(string storageContainer, string storageReference, string messageTypeName)
		{
			StorageReference = storageReference;
			StorageContainer = storageContainer;
			MessageTypeName = messageTypeName;
		}

		public bool IsLoaded
		{
			get
			{
				return Message != null;
			}
		}

	}
}
