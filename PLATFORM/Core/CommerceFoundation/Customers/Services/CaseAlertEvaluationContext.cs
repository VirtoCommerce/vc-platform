using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.Foundation.Customers.Services
{
	public class CaseAlertEvaluationContext: ICaseAlertEvaluationContext
	{
		#region ICaseAlertEvaluationContext
		public Case CurrentCase 
		{ 
			get; 
			set; 
		}

		public object ContextObject
		{
			get;
			set;
		}
		#endregion
		
		public string CaseStatus
		{
			get
			{
				//TODO get Case status (open/pending/closed)
				if (CurrentCase != null)
					return CurrentCase.Status;
				return string.Empty;
			}
		}

		private IDictionary<string, object> Context
		{
			get
			{
				return ContextObject as IDictionary<string, object>;
			}
		}
	}
}
