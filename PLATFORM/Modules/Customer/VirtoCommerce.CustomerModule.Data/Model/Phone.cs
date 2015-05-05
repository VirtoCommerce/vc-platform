using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public class Phone : Entity
	{
		[StringLength(64)]
		public string Number { get; set; }

		[StringLength(64)]
		public string Type { get; set; }


		#region Navigation Properties

		[StringLength(128)]
		public string MemberId { get; set; }

        [ForeignKey("MemberId")]
		public virtual Member Member { get; set; }


		#endregion

		

	}
}
