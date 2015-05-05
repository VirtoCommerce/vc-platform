using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.Content.Web.Models;
using coreModels = VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using System.Text;
using VirtoCommerce.Content.Data.Utility;

namespace VirtoCommerce.Content.Web.Converters
{
	public static class PageConverter
	{
		public static coreModels.Page ToCoreModel(this webModels.Page page)
		{
			var retVal = new coreModels.Page();

			if (page.ByteContent != null)
				retVal.ByteContent = page.ByteContent;
			else if (!string.IsNullOrEmpty(page.Content))
				retVal.ByteContent = Encoding.UTF8.GetBytes(page.Content);

			retVal.PageName = page.Name;
			retVal.Language = page.Language;
			retVal.ContentType = page.ContentType;

			return retVal;
		}

		public static webModels.Page ToWebModel(this coreModels.Page page)
		{
			var retVal = new webModels.Page();

			retVal.Name = page.PageName;
			if (ContentTypeUtility.IsImageContentType(page.ContentType))
			{
				if (page.ByteContent != null)
				{
					//retVal.Content = ContentTypeUtility.ConvertImageToBase64String(item.ByteContent, item.ContentType);
					retVal.ByteContent = page.ByteContent;
				}
				//else
				//{
				//	retVal.Content = retVal.AssetUrl = item.AssetUrl;
				//}
			}
			else if (ContentTypeUtility.IsTextContentType(page.ContentType))
			{
				if (page.ByteContent != null)
				{
					retVal.Content = Encoding.UTF8.GetString(page.ByteContent);
				}
			}
			else // treat as a binary file for now
			{
				if (page.ByteContent != null)
				{
					retVal.ByteContent = page.ByteContent;
				}
			}
			retVal.Language = page.Language;
			retVal.ModifiedDate = page.Updated;
			retVal.Id = page.Id;
			retVal.ContentType = page.ContentType;

			return retVal;
		}

		public static webModels.GetPagesResponse ToWebModel(this IEnumerable<coreModels.Page> pages)
		{
			var retVal = new webModels.GetPagesResponse();

			pages = pages.ToArray();

			var folders = pages.Where(i => i.Id.Split('/').Length > 1).Select(i => i.Id.Split('/')[0]).Distinct();

			foreach (var folder in folders)
			{
				var pageFolder = new webModels.PageFolder
				{
					FolderName = folder
				};

				var addedPages = pages.Select(i => i.ToWebModel()).Where(i => i.Id.StartsWith(string.Format("{0}/", folder)));

				foreach (var page in addedPages)
				{
					var steps = page.Name.Split('/');
					if (steps.Length > 1)
					{
						steps = steps.Skip(1).ToArray();
					}
					page.Name = string.Join("/", steps);

					pageFolder.Pages.Add(page);
				}

				retVal.Folders.Add(pageFolder);
			}

			var files = pages.Where(i => i.Id.Split('/').Length == 1);

			foreach (var file in files)
			{
				var addedFile = file.ToWebModel();

				var steps = addedFile.Name.Split('/');
				if (steps.Length > 1)
				{
					steps = steps.Skip(1).ToArray();
				}
				addedFile.Name = string.Join("/", steps);

				retVal.Pages.Add(addedFile);
			}

			return retVal;
		}
	}
}