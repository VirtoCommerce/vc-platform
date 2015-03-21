using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Messages
{
	[DataContract]
	public class MessageEnvelope
	{
		[DataMember]
		public string EnvelopeId;
		[DataMember]
		public DateTimeOffset DeliverOn;
		[DataMember]
		public MessageReference MessageReference;
		[DataMember]
		public string QueueName;
		[DataMember]
		public object TransportMessage;

		public MessageEnvelope()
		{
		}
		public MessageEnvelope(string envelopeId, string queueName, MessageReference messageReference, DateTimeOffset deliverOn)
		{
			EnvelopeId = envelopeId;
			DeliverOn = deliverOn;
			MessageReference = messageReference;
			QueueName = queueName;
		}
	}
}
