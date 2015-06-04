using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Serialization
{
	public class DataContractMessageSerializer : IMessageSerializer
	{
		readonly IDictionary<string, Type> _contract2Type = new Dictionary<string, Type>();
		readonly IEnumerable<Type> _knownTypes;
		readonly IDictionary<Type, string> _type2Contract = new Dictionary<Type, string>();

		[InjectionConstructor]
		public DataContractMessageSerializer(IUnityContainer container)
			: this(container.ResolveAll<IKnownSerializationTypes>().SelectMany(x => x.GetKnownTypes()).Distinct())
		{
		}

		public DataContractMessageSerializer(IEnumerable<Type> knowTypes)
		{
			if (knowTypes.Count() == 0)
			{
				throw new InvalidOperationException("DataContractMessageSerializer requires some known types to serialize. Have you forgot to supply them?");
			}
			_knownTypes = knowTypes;
			DataContractUtility.ThrowOnMessagesWithoutDataContracts(_knownTypes);

			foreach (var type in _knownTypes)
			{
				var reference = DataContractUtility.GetContractReference(type);
				_contract2Type.Add(reference, type);
				_type2Contract.Add(type, reference);
			}
		}


		/// <summary>
		/// Serializes the object to the specified stream
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="destinationStream">The destination stream.</param>
		public void Serialize(object message, Stream destinationStream)
		{
			var serializer = new DataContractSerializer(message.GetType(), _knownTypes);

			//using (var compressed = destination.Compress(true))
			using (var writer = XmlDictionaryWriter.CreateBinaryWriter(destinationStream, null, null, false))
			{
				serializer.WriteObject(writer, message);
			}
		}

		/// <summary>
		/// Deserializes the object from specified source stream.
		/// </summary>
		/// <param name="sourceStream">The source stream.</param>
		/// <param name="type">The type of the object to deserialize.</param>
		/// <returns>deserialized object</returns>
		public object Deserialize(Stream sourceStream, Type type)
		{
			var serializer = new DataContractSerializer(type, _knownTypes);

			using (var reader = XmlDictionaryReader.CreateBinaryReader(sourceStream, XmlDictionaryReaderQuotas.Max))
			{
				return serializer.ReadObject(reader);
			}
		}

		/// <summary>
		/// Gets the contract name by the type
		/// </summary>
		/// <param name="messageType">Type of the message.</param>
		/// <returns>contract name (if found)</returns>
		public string GetContractNameByType(Type messageType)
		{
			return _type2Contract[messageType];
		}

		/// <summary>
		/// Gets the type by contract name.
		/// </summary>
		/// <param name="contractName">Name of the contract.</param>
		/// <returns>type that could be used for contract deserialization (if found)</returns>
		public Type GetTypeByContractName(string contractName)
		{
			return _contract2Type[contractName];
		}


	}
}
