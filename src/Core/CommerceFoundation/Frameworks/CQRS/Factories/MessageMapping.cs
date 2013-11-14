using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Factories
{
	internal sealed class MessageMapping
	{
		public readonly Type Consumer;
		public readonly Type Message;
		public readonly bool Direct;

		public MessageMapping(Type consumer, Type message, bool direct)
		{
			Consumer = consumer;
			Message = message;
			Direct = direct;
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", Consumer.Name, Direct ? "==>" : "-->", Message.Name);
		}

		public bool Equals(MessageMapping other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.Consumer, Consumer) && Equals(other.Message, Message) && other.Direct.Equals(Direct);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(MessageMapping)) return false;
			return Equals((MessageMapping)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (Consumer != null ? Consumer.GetHashCode() : 0);
				result = (result * 397) ^ (Message != null ? Message.GetHashCode() : 0);
				result = (result * 397) ^ Direct.GetHashCode();
				return result;
			}
		}

		/// <summary>
		/// Indicates that the message is orphaned (not consumed)
		/// </summary>
		public abstract class BusNull { }
	}
}
