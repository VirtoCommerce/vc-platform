using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	public class FactoryBase : IFactory, IKnownSerializationTypes
	{
		protected List<Tuple<Type, Type>> OverriddenTypeMap = new List<Tuple<Type, Type>>();
		protected Dictionary<Type, string> Type2StringMap = new Dictionary<Type, string>();
		protected Dictionary<string, Type> String2TypeMap = new Dictionary<string, Type>();

		#region IEntityFactory Members
		public virtual object CreateEntityForType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			var typeName = GetEntityTypeStringName(type);
			return CreateEntityForType(typeName);
		}

		public virtual object CreateEntityForType(string typeName)
		{
			object retVal = null;
			Type resultType = GetEntityTypeByStringName(typeName);
			if (resultType != null)
			{
				retVal = Activator.CreateInstance(resultType);
			}
			return retVal;
		}

		public string GetEntityTypeStringName(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			string retVal;

			var overridePair = OverriddenTypeMap.FirstOrDefault(x => x.Item2 == type);
			if (overridePair != null)
			{
				type = overridePair.Item1;
			}

			Type2StringMap.TryGetValue(type, out retVal);
			return retVal;
		}

		public Type GetEntityTypeByStringName(string typeName)
		{
			Type retVal = null;
			String2TypeMap.TryGetValue(typeName, out retVal);
			if (retVal != null)
			{
				//Overrides
				var overridePair = OverriddenTypeMap.FirstOrDefault(x => x.Item1 == retVal);
				if (overridePair != null)
				{
					retVal = overridePair.Item2;
				}
			}

			return retVal;
		}

		public Type GetBaseType(Type overrideType)
		{
			if (overrideType == null)
			{
				throw new ArgumentNullException("overrideType");
			}
			var retVal = overrideType;
			var overridePair = OverriddenTypeMap.FirstOrDefault(x => x.Item2 == overrideType);
			if (overridePair != null)
			{
				retVal = overridePair.Item1;
			}
			return retVal;
		}
		#endregion

		#region IKnownSerializationTypes Members

		public IEnumerable<Type> GetKnownTypes()
		{
			return Type2StringMap.Keys.Concat(OverriddenTypeMap.Select(x => x.Item2)).Distinct();
		}

		#endregion

		public void RegisterStorageType(Type type, string typeName)
		{
			Type2StringMap.Add(type, typeName);
			String2TypeMap.Add(typeName, type);
		}

		public void OverrideType(Type type, Type newType)
		{
			var existOverride = OverriddenTypeMap.FirstOrDefault(x => x.Item1 == type);
			if (existOverride != null)
			{
				OverriddenTypeMap.Remove(existOverride);
			}
			OverriddenTypeMap.Add(new Tuple<Type, Type>(type, newType));
		}

        public T CreateEntity<T>()
        {
            return (T)CreateEntityForType(GetEntityTypeStringName(typeof(T)));
        }
    }
}
