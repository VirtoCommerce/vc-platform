using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Reviews.Model
{
	[DataContract]
	[EntitySet("ReportHelpfulElements")]
	public class ReportHelpfulElement : ReportElementBase
	{
		private bool _IsHelpful;
		[DataMember]
		public bool IsHelpful
		{
			get
			{
				return _IsHelpful;
			}
			set
			{
				SetValue(ref _IsHelpful, () => this.IsHelpful, value);
			}
		}
	}
}
