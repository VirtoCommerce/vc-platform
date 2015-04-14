using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class DynamicContentFolder : Entity
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public string Path
		{
			get
			{
				return ParentFolder == null ? Name : ParentFolder.Path + "\\" + Name;
			}
		}
		public string Outline
		{
			get
			{
				return ParentFolder == null ? Id : ParentFolder.Outline + ";" + Id;
			}
		}

		public string ParentFolderId { get; set; }
		public DynamicContentFolder ParentFolder { get; set; }
	}
}
