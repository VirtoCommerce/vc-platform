using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions
{
	public abstract class DynamicBlockExpression : DynamicExpression
	{
		[JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
		public IDynamicExpression[] AvailableChildren { get; set; }
		[JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
		public IDynamicExpression[] Children { get; set; }
	}
}