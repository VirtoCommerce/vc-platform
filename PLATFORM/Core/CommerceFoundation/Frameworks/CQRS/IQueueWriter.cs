using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;

namespace VirtoCommerce.Foundation.Frameworks.CQRS
{
	public interface IQueueWriter
	{
		void PutMessage(MessageEnvelope envelope);
	}
}
