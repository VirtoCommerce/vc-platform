using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerceCMS.Data.Repositories
{
	public class FileSystemFileRepositoryImpl : IFileRepository
	{
		private string _mainPath;

		//public FileSystemFileRepositoryImpl(string mainPath)
		//{
		//	_mainPath = mainPath;
		//}

		public Models.ContentItem[] GetContentItems(string path)
		{
			throw new NotImplementedException();
		}

		public Models.ContentItem GetContentItem(string path)
		{
			throw new NotImplementedException();
		}

		public void SaveContentItem(Models.ContentItem item)
		{
			throw new NotImplementedException();
		}

		public void DeleteContentItem(Models.ContentItem item)
		{
			throw new NotImplementedException();
		}

		//private string 
	}
}
