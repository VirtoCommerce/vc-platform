using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace VirtoCommerce.Foundation.Frameworks
{
	public class DataContractSurrogate : IDataContractSurrogate
	{
		private readonly IFactory _entityFactory;
		private readonly IKnownSerializationTypes _knowTypes;

		public DataContractSurrogate(IFactory entityFactory, IKnownSerializationTypes knowTypes)
		{
			_entityFactory = entityFactory;
			_knowTypes = knowTypes;
		}

		#region IDataContractSurrogate Members

		public object GetCustomDataToExport(Type clrType, Type dataContractType)
		{
			return null;
		}

		public object GetCustomDataToExport(System.Reflection.MemberInfo memberInfo, Type dataContractType)
		{
			return null;
		}

		public Type GetDataContractType(Type type)
		{
			var retVal = type;
			var typeName = _entityFactory.GetEntityTypeStringName(type);
			if (!String.IsNullOrEmpty(typeName))
			{
				retVal = _entityFactory.GetEntityTypeByStringName(typeName);
			}
			//Debug.WriteLine(String.Format("GetDataContractType {0} -> {1}", type.Name, retVal.Name));
			return retVal;
		}

		public object GetDeserializedObject(object obj, Type targetType)
		{
			var retVal = obj;
			return obj;
		}

		public void GetKnownCustomDataTypes(System.Collections.ObjectModel.Collection<Type> customDataTypes)
		{
		}

		public object GetObjectToSerialize(object obj, Type targetType)
		{
			return obj;
		}

		public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
		{
			return null;
		}

		public System.CodeDom.CodeTypeDeclaration ProcessImportedType(System.CodeDom.CodeTypeDeclaration typeDeclaration, System.CodeDom.CodeCompileUnit compileUnit)
		{
			return typeDeclaration;
		}

		#endregion
	}
}
