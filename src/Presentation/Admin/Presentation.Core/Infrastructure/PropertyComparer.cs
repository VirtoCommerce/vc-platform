using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public class PropertyComparer<T> : IEqualityComparer<T>
	{
		private readonly PropertyInfo _PropertyInfo;

		/// <summary>
		/// Creates a new instance of PropertyComparer.
		/// </summary>
		/// <param name="propertyName">The name of the property on type T 
		/// to perform the comparison on.</param>
		public PropertyComparer(string propertyName)
		{
			//store a reference to the property info object for use during the comparison
			_PropertyInfo = typeof(T).GetProperty(propertyName,
		BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
			if (_PropertyInfo == null)
			{
				throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
			}
		}

		#region IEqualityComparer<T> Members

		public bool Equals(T x, T y)
		{
			//get the current value of the comparison property of x and of y
			object xValue = _PropertyInfo.GetValue(x, null);
			object yValue = _PropertyInfo.GetValue(y, null);

			//if the xValue is null then we consider them equal if and only if yValue is null
			if (xValue == null)
				return yValue == null;

			//use the default comparer for whatever type the comparison property is.
			return xValue.Equals(yValue);
		}

		public int GetHashCode(T obj)
		{
			//get the value of the comparison property out of obj
			object propertyValue = _PropertyInfo.GetValue(obj, null);

			if (propertyValue == null)
				return 0;

			else
				return propertyValue.GetHashCode();
		}

		#endregion
	}  
}
