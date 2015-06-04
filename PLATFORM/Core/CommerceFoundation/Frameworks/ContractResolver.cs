using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace VirtoCommerce.Foundation.Frameworks
{
	public class ContractResolver : DataContractResolver
	{
		private const string DefaultNamespace = "global";
		readonly Dictionary<Type, Tuple<string, string>> _typeToNames = new Dictionary<Type, Tuple<string, string>>();
		readonly Dictionary<string, Dictionary<string, Type>> _namesToType = new Dictionary<string, Dictionary<string, Type>>();

		private readonly IFactory _entityFactory;
		private readonly IKnownSerializationTypes _knowTypes;

		public ContractResolver(IFactory entityFactory, IKnownSerializationTypes knowTypes)
		{
			_entityFactory = entityFactory;
			_knowTypes = knowTypes;

			foreach (Type type in ReflectTypes().Concat(_knowTypes.GetKnownTypes()))
			{
				string typeNamespace = GetNamespace(type);
				string typeName = GetName(type);

				_typeToNames[type] = new Tuple<string, string>(typeNamespace, typeName);

				if (_namesToType.ContainsKey(typeNamespace) == false)
				{
					_namesToType[typeNamespace] = new Dictionary<string, Type>();
				}
				_namesToType[typeNamespace][typeName] = type;
			}
		}

		#region DataContractResolver overrides

		public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
		{
			//Debug.WriteLine("ResolveName ", typeName);

			Type retVal = null;

			if (_namesToType.ContainsKey(typeNamespace))
			{
				if (_namesToType[typeNamespace].ContainsKey(typeName))
				{
					retVal = _namesToType[typeNamespace][typeName];
				}
			}
			else
			{
				retVal = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
			}

			return retVal;
		}

		public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out System.Xml.XmlDictionaryString typeName, out System.Xml.XmlDictionaryString typeNamespace)
		{
			Debug.WriteLine("TryResolveType ", type.Name);
			var retVal = _typeToNames.ContainsKey(type);
			if (retVal)
			{
				XmlDictionary dictionary = new XmlDictionary();
				typeNamespace = dictionary.Add(_typeToNames[type].Item1);
				typeName = dictionary.Add(_typeToNames[type].Item2);
			}
			else
			{
				retVal = knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace);
			}

			return retVal;
		}

		#endregion

		private string GetNamespace(Type type)
		{
			string retVal = DefaultNamespace;
			if (_entityFactory != null)
			{
				type = _entityFactory.GetBaseType(type);
			}

			if (type != null)
			{
				retVal = type.Namespace;
			}
			return retVal;
		}

		private string GetName(Type type)
		{
			string retVal = null;
			if (_entityFactory != null)
			{
				retVal = _entityFactory.GetEntityTypeStringName(type);
			}

			if (string.IsNullOrEmpty(retVal))
			{
				retVal = type.Name;
			}
			return retVal;
		}

		private Type[] ReflectTypes()
		{
			List<Type> types = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				var assemblyTypes = GetTypes(assembly);
				types.AddRange(assemblyTypes);
			}

			return types.ToArray();
		}


		private static Type[] GetTypes(Assembly assembly, bool publicOnly = true)
		{
			List<Type> types = new List<Type>();
			try
			{
				Type[] allTypes = assembly.GetTypes();
				foreach (Type type in allTypes.Where(x => x.GetCustomAttributes(typeof(DataContractAttribute), true).FirstOrDefault() != null))
				{
					if (type.IsEnum == false &&
					   type.IsInterface == false &&
					   type.IsGenericTypeDefinition == false)
					{
						if (publicOnly == true && type.IsPublic == false)
						{
							if (type.IsNested == false)
							{
								continue;
							}
							if (type.IsNestedPrivate == true)
							{
								continue;
							}
						}
						types.Add(type);
					}
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
			return types.ToArray();
		}

	
	}
}
