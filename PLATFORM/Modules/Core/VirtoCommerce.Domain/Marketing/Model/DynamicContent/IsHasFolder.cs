using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public interface IsHasFolder
	{
		string FolderId { get; set; }
		DynamicContentFolder Folder { get; set; }
			 
	}
}
