using System.Collections.Generic;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	/// <summary>
	/// The interface that models a tree-like hierarchy. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IHierarchy
	{
		/// <summary>
		/// Modifies the hierarchy by adding another child to the given parent. 
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="child">The child.</param>
		void AddChild(object parent, object child);

		/// <summary>
		/// Determines whether the given item is part of this hierarchy. 
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
		/// </returns>
		bool Contains(object item);

		/// <summary>
		/// Returns an enumerable over the children of the provided item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="endIndex">The end index.</param>
		/// <returns></returns>
		IEnumerable<object> GetChildren(object item, int startIndex, int endIndex);

		/// <summary>
		/// Returns an enumerable over the children of the provided item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		IEnumerable<object> GetChildren(object item);

		/// <summary>
		/// Returns the parent item of the item or Root if child is a top-level item. 
		/// </summary>
		/// <param name="child">The child.</param>
		/// <returns></returns>
		object GetParent(object child);

		/// <summary>
		/// Returns whether the given item is considered a leaf item. 
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		///   <c>true</c> if the specified item is leaf; otherwise, <c>false</c>.
		/// </returns>
		bool IsLeaf(object item);

		/// <summary>
		/// Removes the given item from its parent and this hierarchy. 
		/// </summary>
		/// <param name="child">The child.</param>
		void Remove(object child);

		/// <summary>
		/// Determines whether the given item should be considered a leaf. 
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="leaf">if set to <c>true</c> [leaf].</param>
		void SetLeaf(object item, bool leaf);

		/// <summary>
		/// Reparents a child item that already belongs to this hierarchy instance to a new parent. 
		/// </summary>
		/// <param name="child">The child.</param>
		/// <param name="parent">The parent.</param>
		void SetParent(object child, object parent);

		/// <summary>
		/// Gets the root item of the hierarchy. 
		/// </summary>
		object Root { get; }

		/// <summary>
		/// Returns an instance that implements the given type or null. 
		/// </summary>
		object Item { get; }
	}
}
