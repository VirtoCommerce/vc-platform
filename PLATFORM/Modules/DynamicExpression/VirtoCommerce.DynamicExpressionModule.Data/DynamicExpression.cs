using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DynamicExpressionModule.Data
{
	public abstract class DynamicExpression : IDynamicExpression
	{
		public DynamicExpression()
		{
			Id = this.GetType().Name;
			AvailableChildren = new List<DynamicExpression>();
			Children = new List<DynamicExpression>();
		}

		#region IDynamicExpression Members

		public string Id { get; set; }
	
		#endregion
		[JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
		public ICollection<DynamicExpression> AvailableChildren { get; set; }
		[JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
		public ICollection<DynamicExpression> Children { get; set; }

		public T FindAvailableExpression<T>() where T : IDynamicExpression
		{
			var retVal = this.Traverse(x => x.AvailableChildren).SelectMany(x => x.AvailableChildren).OfType<T>().FirstOrDefault();
			return retVal;
		}
		public T FindChildrenExpression<T>() where T : IDynamicExpression
		{
			var retVal = this.Traverse(x => x.Children).SelectMany(x => x.Children).OfType<T>().FirstOrDefault();
			return retVal;
		}
	}
}