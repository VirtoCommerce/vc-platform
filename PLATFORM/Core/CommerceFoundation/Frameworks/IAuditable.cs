using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks
{
	public interface IAuditable
	{
		DateTime CreatedDate { get; set; }
		string CreatedBy { get; set; }
		DateTime? ModifiedDate { get; set; }
		string ModifiedBy { get; set; }
	}
}
