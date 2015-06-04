using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Factories
{
	public class DomainAssemblyScannerConsumerFactory : IConsumerFactory, IKnownSerializationTypes
	{
		private List<MessageInfo> _messageInfos = new List<MessageInfo>();
		private List<ConsumerInfo> _consumerInfos = new List<ConsumerInfo>();
		private readonly List<Assembly> _assemblies = new List<Assembly>();
		private readonly MethodInfo _consumingMethod;
		private readonly IUnityContainer _container;

		[InjectionConstructor]
		public DomainAssemblyScannerConsumerFactory(IUnityContainer container)
			: this(new Assembly[] { Assembly.GetExecutingAssembly() }, container)
		{
		}

		public DomainAssemblyScannerConsumerFactory(Assembly[] assemblies, IUnityContainer container)
		{
			//TODO: Add support user assemblies
			_assemblies.AddRange(assemblies);

			_consumingMethod = typeof(IConsume).GetMethod("Consume");
			_container = container;

			Initialize();
		}

		#region IConsumerFactory Members

		public IEnumerable<IConsume> GetMessageConsumers(IMessage message)
		{
			var retval = new List<IConsume>();
			var messageInfo = _messageInfos.FirstOrDefault(x => x.MessageType == message.GetType());
			if (messageInfo != null)
			{
				foreach (var consumerType in messageInfo.AllConsumers)
				{
					var consumerInstance = _container.Resolve(consumerType) as IConsume;
					if (consumerInstance != null)
					{
						retval.Add(consumerInstance);
					}
				}
			}
			return retval;
		}

		#endregion

		private void Initialize()
		{
			if (null == _consumingMethod)
				throw new InvalidOperationException("Consuming method has not been defined");

			if (!_assemblies.Any())
				throw new InvalidOperationException("There are no assemblies to scan");

			var types = _assemblies
				.SelectMany(a => {
				    try
				    {
				        return a.GetExportedTypes();
				    } catch{}
				    return new Type[0];
				}).Where(x=>x!=null)
				.ToList();

			var messageTypes = types.Where(x => !x.IsInterface && typeof(IMessage).IsAssignableFrom(x)).ToArray();
			var consumerTypes = types.Where(x => typeof(IConsume).IsAssignableFrom(x)).ToArray();

			var consumingDirectly = consumerTypes
				.SelectMany(consumerType =>
					GetConsumedMessages(consumerType)
						.Select(messageType => new MessageMapping(consumerType, messageType, true)))
				.ToArray();

			var result = new HashSet<MessageMapping>();

			foreach (var m in consumingDirectly)
			{
				result.Add(m);
			}
			var allMessages = result.Select(m => m.Message).ToList();
			foreach (var messageType in messageTypes)
			{
				if (!allMessages.Contains(messageType))
				{
					allMessages.Add(messageType);
					result.Add(new MessageMapping(typeof(MessageMapping.BusNull), messageType, true));
				}
			}

			_consumerInfos = result
				.GroupBy(x => x.Consumer)
				.Select(x =>
				{
					var directs = x
						.Where(m => m.Direct)
						.Select(m => m.Message)
						.Distinct();

					var assignables = x
						.Select(m => m.Message)
						.Where(t => directs.Any(d => d.IsAssignableFrom(t)))
						.Distinct();

					return new ConsumerInfo(x.Key, assignables.ToArray());
				}).ToList();


			_messageInfos = result
				.ToLookup(x => x.Message)
				.Select(x =>
				{
					var domainConsumers = x
						.Where(t => t.Consumer != typeof(MessageMapping.BusNull))
						.ToArray();

					return new MessageInfo
					{
						MessageType = x.Key,
						AllConsumers = domainConsumers.Select(m => m.Consumer).Distinct().ToArray(),
						DerivedConsumers = domainConsumers.Where(m => !m.Direct).Select(m => m.Consumer).Distinct().ToArray(),
						DirectConsumers = domainConsumers.Where(m => m.Direct).Select(m => m.Consumer).Distinct().ToArray(),
					};
				}).ToList();

			var includedTypes = _messageInfos
				.Select(m => m.MessageType).ToList();

			// message directory should still include all messages for the serializers
			var orphanedMessages = result
				.Where(m => !includedTypes.Contains(m.Message))
				.Select(m => new MessageInfo
				{
					MessageType = m.Message,
					AllConsumers = Type.EmptyTypes,
					DerivedConsumers = Type.EmptyTypes,
					DirectConsumers = Type.EmptyTypes
				});

			_messageInfos.AddRange(orphanedMessages);


		}

		static IEnumerable<Type> GetConsumedMessages(Type consumerType)
		{
			var msgTypeAttrs = consumerType.GetCustomAttributes(typeof(MessageTypeAttribute), false).OfType<MessageTypeAttribute>();
			return msgTypeAttrs.Select(x => x.SupportMessageType).Distinct();
		}


		#region IKnownSerializationTypes Members

		public IEnumerable<Type> GetKnownTypes()
		{
			return _messageInfos.Select(x => x.MessageType).Distinct();
		}

		#endregion
	}

	// rp: for test purposes, doesn't use unity container
	public class SimpleConsumerFactory : IConsumerFactory
	{
		private readonly IConsume consume;
		public SimpleConsumerFactory(IConsume consume)
		{
			this.consume = consume;
		}

		public IEnumerable<IConsume> GetMessageConsumers(IMessage message)
		{
			var retval = new List<IConsume> { consume };
			return retval;
		}
	}

}
