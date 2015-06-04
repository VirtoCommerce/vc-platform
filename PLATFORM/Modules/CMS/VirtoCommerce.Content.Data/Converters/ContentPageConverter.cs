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
	public static class ContentPageConverter
	{
		public static ContentPage ToShortModel(this RepositoryContent repositoryContent, DateTime modifiedDate)
		{
			var retVal = new ContentPage();

			retVal.Name = Path.GetFileNameWithoutExtension(repositoryContent.Name);
			retVal.ModifiedDate = modifiedDate;
			retVal.Language = GetLanguageFromFullPath(repositoryContent.Path);

			return retVal;
		}

		public static ContentPage ToPageModel(this RepositoryContent repositoryContent)
		{
			var retVal = new ContentPage();

			retVal.Name = Path.GetFileNameWithoutExtension(repositoryContent.Name);
			if (!string.IsNullOrEmpty(repositoryContent.Content))
			{
				retVal.ByteContent = Encoding.UTF8.GetBytes(repositoryContent.Content);
			}
			retVal.Language = GetLanguageFromFullPath(repositoryContent.Path);

			return retVal;
		}

		private static string GetLanguageFromFullPath(string fullPath)
		{
			var steps = fullPath.Split('/');
			var language = steps[steps.Length - 2];

			return language;
		}

		public static Page AsPage(this ContentPage contentPage)
		{
			var retVal = new Page();

			retVal.Id = contentPage.Path;
			retVal.PageName = contentPage.Name;
			retVal.ByteContent = contentPage.ByteContent;
			retVal.Language = contentPage.Language;
			retVal.Updated = contentPage.ModifiedDate.HasValue ? contentPage.ModifiedDate.Value : contentPage.CreatedDate;
			retVal.ContentType = contentPage.ContentType;

			return retVal;
		}

		public static ContentPage AsContentPage(this Page page)
		{
			var retVal = new ContentPage();

			retVal.Id = page.Id;
			retVal.Language = page.Language;
			retVal.Path = page.Id;
			retVal.Name = page.PageName;
			retVal.ByteContent = page.ByteContent;
			retVal.ContentType = page.ContentType;

			return retVal;
		}
	}
}
