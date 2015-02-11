using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerceCMS.Data.Models;

namespace VirtoCommerceCMS.Data.Converters
{
	public static class ContentItemConverter
	{
		public static ContentItem RepositoryContent2ContentItem(RepositoryContent repositoryContent)
		{
			ContentItem retVal = null;

			if (repositoryContent.Type == Octokit.ContentType.Dir || repositoryContent.Type == Octokit.ContentType.File)
			{
				retVal = new ContentItem();

				retVal.Content = repositoryContent.Content;
				retVal.ContentType = repositoryContent.Type == Octokit.ContentType.Dir ?
					Models.ContentType.Directory :
					Models.ContentType.File;

				retVal.Name = repositoryContent.Name;
				retVal.Path = repositoryContent.Path;
			}

			return retVal;
		}
	}
}
