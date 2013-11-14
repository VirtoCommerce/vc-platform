using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	public interface IFactory
	{
		/// <summary>
		/// Creates the type of the entity for.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		object CreateEntityForType(Type type);
		/// <summary>
		/// Creates the type of the entity for.
		/// </summary>
		/// <param name="typeName">Name of the type string.</param>
		/// <returns></returns>
		object CreateEntityForType(string typeName);
		/// <summary>
		/// Gets the name of the entity type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		string GetEntityTypeStringName(Type type);
		/// <summary>
		/// Get type by entity string name 
		/// </summary>
		/// <param name="typeName">Name of the type string.</param>
		/// <returns></returns>
		Type GetEntityTypeByStringName(string typeName);
		/// <summary>
		/// Gets the type overridden by overrideType 
		/// </summary>
		/// <param name="overrideType">Type of the override.</param>
		/// <returns></returns>
		Type GetBaseType(Type overrideType);

	    T CreateEntity<T>();

        IEnumerable<Type> GetKnownTypes();
	}
}
