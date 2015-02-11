using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerceCMS.Data.Models;

namespace VirtoCommerceCMS.Data.Repositories
{
	public interface IFileRepository
	{
		ContentItem[] GetContentItems(string path);
		ContentItem GetContentItem(string path);
		void SaveContentItem(ContentItem item);
		void DeleteContentItem(ContentItem item);
	}
}
