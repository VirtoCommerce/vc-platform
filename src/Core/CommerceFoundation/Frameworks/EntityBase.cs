using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks
{
	public abstract class EntityBase<TEntity, TID> :
	   IEquatable<EntityBase<TEntity, TID>>
	   where TEntity : EntityBase<TEntity, TID>
	{

		public virtual TID Id { get; protected set; }



		/// <summary>
		/// Indicates whether the current object 
		/// is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal 
		/// to the <paramref name="other"/> parameter; 
		/// otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(EntityBase<TEntity, TID> other)
		{
			if (other == null)
				return false;

			// Handle the case of comparing two NEW objects    
			var otherIsTransient = Equals(other.Id, default(TID));
			var currentIsTransient = Equals(Id, default(TID));

			if (otherIsTransient && currentIsTransient)
				return ReferenceEquals(other, this);

			return other.Id.Equals(Id);
		}

		/// <summary>
		/// Equality
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as TEntity;

			return Equals(other);
		}

		/// <summary>
		/// Get hash code
		/// </summary>
		public override int GetHashCode()
		{
			var thisIsTransient = Equals(Id, default(TID));

			// When this instance is transient, we use the base GetHashCode()    
			return thisIsTransient ? base.GetHashCode() : Id.GetHashCode();
		}

		/// <summary>
		/// Equal operator
		/// </summary>
		public static bool operator ==
			(EntityBase<TEntity, TID> x, EntityBase<TEntity, TID> y)
		{
			return Equals(x, y);
		}

		/// <summary>
		/// Not equal operator
		/// </summary>
		public static bool operator !=
			(EntityBase<TEntity, TID> x, EntityBase<TEntity, TID> y)
		{
			return !(x == y);
		}

	}
}
