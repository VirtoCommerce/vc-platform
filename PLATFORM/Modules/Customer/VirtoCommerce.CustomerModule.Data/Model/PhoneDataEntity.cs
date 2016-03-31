using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public class PhoneDataEntity : Entity
	{
		[StringLength(64)]
		public string Number { get; set; }

		[StringLength(64)]
		public string Type { get; set; }


		#region Navigation Properties

		public string MemberId { get; set; }

		public virtual MemberDataEntity Member { get; set; }


		#endregion

		

	}
}
