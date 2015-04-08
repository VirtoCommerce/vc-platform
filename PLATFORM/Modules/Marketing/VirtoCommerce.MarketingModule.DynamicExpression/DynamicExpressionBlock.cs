using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MarketingModule.DynamicExpression
{
	public abstract class DynamicExpressionBlock : DynamicExpression
	{
		public DynamicExpressionBlock()
		{
			AvailableChildren = new List<IDynamicExpression>();
			Children = new List<IDynamicExpression>();
		}

		public T FindAvailableExpression<T>() where T : IDynamicExpression
		{
			var retVal = this.Traverse(x => x.AvailableChildren.OfType<DynamicExpressionBlock>()).SelectMany(x => x.AvailableChildren).OfType<T>().FirstOrDefault();
			return retVal;
		}
		public T FindChildrenExpression<T>() where T : IDynamicExpression
		{
			var retVal =  this.Traverse(x => x.Children.OfType<DynamicExpressionBlock>()).SelectMany(x => x.Children).OfType<T>().FirstOrDefault();
			return retVal;
		}

		[JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
		public ICollection<IDynamicExpression> AvailableChildren { get; set; }
		[JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
		public ICollection<IDynamicExpression> Children { get; set; }
	}
}