using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks
{
	public abstract class Entity
	{
		private int? _requestedHashCode;

		public string Id { get; set; }

		public bool IsTransient()
		{
			return this.Id == null;
		}

		#region Overrides Methods

		/// <summary>
		/// <see cref="M:System.Object.Equals"/>
		/// </summary>
		/// <param name="obj"><see cref="M:System.Object.Equals"/></param>
		/// <returns><see cref="M:System.Object.Equals"/></returns>
		public override bool Equals(object obj)
		{
			var entity = obj as Entity;
			if (entity == null)
				return false;

			if (Object.ReferenceEquals(this, obj))
				return true;

			return entity.Id == this.Id;
		}

		/// <summary>
		/// <see cref="M:System.Object.GetHashCode"/>
		/// </summary>
		/// <returns><see cref="M:System.Object.GetHashCode"/></returns>
		public override int GetHashCode()
		{
			if (!IsTransient())
			{
				if (!_requestedHashCode.HasValue)
					_requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

				return _requestedHashCode.Value;
			}
			else
				return base.GetHashCode();
		}

		public static bool operator ==(Entity left, Entity right)
		{
			if (Object.Equals(left, null))
				return (Object.Equals(right, null)) ? true : false;
			else
				return left.Equals(right);
		}

		public static bool operator !=(Entity left, Entity right)
		{
			return !(left == right);
		}

		#endregion
	}
}
