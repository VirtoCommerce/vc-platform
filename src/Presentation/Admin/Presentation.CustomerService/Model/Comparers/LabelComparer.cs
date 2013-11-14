using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.ManagementClient.Customers.Model.Comparers
{
	public class LabelComparer : IEqualityComparer<VirtoCommerce.Foundation.Customers.Model.Label>
	{

		public bool Equals(VirtoCommerce.Foundation.Customers.Model.Label x, VirtoCommerce.Foundation.Customers.Model.Label y)
		{
			if (Object.ReferenceEquals(x, y)) return true;

			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;

			return x.LabelId == y.LabelId;
		}

		public int GetHashCode(VirtoCommerce.Foundation.Customers.Model.Label obj)
		{
			if (Object.ReferenceEquals(obj, null)) return 0;

			int hashLabelId = (obj as VirtoCommerce.Foundation.Customers.Model.Label).LabelId.GetHashCode();

			int hashLabelsName = (obj as VirtoCommerce.Foundation.Customers.Model.Label).Name.GetHashCode();

			return hashLabelId ^ hashLabelsName;
		}
	}
}
