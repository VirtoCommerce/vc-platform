using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions
{
	public abstract class DynamicExpression : IDynamicExpression
	{
		public DynamicExpression()
		{
			Id = this.GetType().Name;
		}

		#region IDynamicExpression Members

		public string Id { get; set; }
	
		#endregion
	}
}