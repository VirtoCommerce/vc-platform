using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace VirtoCommerce.Foundation.Frameworks.ConventionInjections
{
	public class PatchInjection<T> : ConventionInjection
	{
		private List<string> _propertyNames = new List<string>();
		public PatchInjection(params Expression<Func<T, object>>[] propertyNames)
		{
			if (propertyNames != null)
			{
				foreach (var property in propertyNames)
				{
					var expression = property.Body as MemberExpression;
					if (expression != null)
					{
						var memberName = expression.Member.Name;
						_propertyNames.Add(memberName);
					}
				}
			}
		}

		protected override bool Match(ConventionInfo c)
		{
			var retVal = c.SourceProp.Name == c.TargetProp.Name &&
					   c.SourceProp.Type == c.TargetProp.Type &&
					   c.SourceProp.Value != null && _propertyNames.Contains(c.SourceProp.Name);
			return retVal;
		}
	}
}
