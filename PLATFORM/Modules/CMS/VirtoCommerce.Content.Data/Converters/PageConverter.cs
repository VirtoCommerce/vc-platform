using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Converters
{
	public static class PageConverter
	{
		public static Page ToShortModel(this RepositoryContent repositoryContent, DateTime modifiedDate)
		{
			var retVal = new Page();

			retVal.Name = Path.GetFileNameWithoutExtension(repositoryContent.Name);
			retVal.ModifiedDate = modifiedDate;
			retVal.Language = GetLanguageFromFullPath(repositoryContent.Path);

			return retVal;
		}

		public static Page ToPageModel(this RepositoryContent repositoryContent)
		{
			var retVal = new Page();

			retVal.Name = Path.GetFileNameWithoutExtension(repositoryContent.Name);
			retVal.Content = repositoryContent.Content;
			retVal.Language = GetLanguageFromFullPath(repositoryContent.Path);

			return retVal;
		}

		private static string GetLanguageFromFullPath(string fullPath)
		{
			var steps = fullPath.Split('/');
			var language = steps[steps.Length - 2];

			return language;
		}
	}
}
