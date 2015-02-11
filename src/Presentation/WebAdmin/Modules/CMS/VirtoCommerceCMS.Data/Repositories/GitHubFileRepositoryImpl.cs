using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data;
using VirtoCommerceCMS.Data.Converters;
using VirtoCommerceCMS.Data.Models;

namespace VirtoCommerceCMS.Data.Repositories
{
	public class GitHubFileRepositoryImpl : IFileRepository
	{
		private GitHubClient _client;

		private string _ownerName;
		private string _repositoryName;


		public GitHubFileRepositoryImpl(string login, string password, string productHeaderValue, string ownerName, string repositoryName)
		{
			_client = new GitHubClient(new ProductHeaderValue(productHeaderValue), new Uri("https://github.com/"))
			{
				Credentials = new Credentials(login, password)
			};

			_repositoryName = repositoryName;
			_ownerName = ownerName;
		}

		public ContentItem[] GetContentItems(string path)
		{
			List<ContentItem> items = new List<ContentItem>();
			var result = _client.Repository.Content.GetContents(_ownerName, _repositoryName, path).Result.Where(s => s.Type == Octokit.ContentType.Dir || s.Type == Octokit.ContentType.File);
			foreach (var item in result)
			{
				var addedItem = ContentItemConverter.RepositoryContent2ContentItem(item);
				if (addedItem != null)
				{
					items.Add(addedItem);
				}
			}

			return items.ToArray();
		}


		public ContentItem GetContentItem(string path)
		{
			ContentItem retVal = new ContentItem();
			var result = _client.Repository.Content.GetContents(_ownerName, _repositoryName, path).Result;

			var item = result.SingleOrDefault();
			if(item != null)
			{
				retVal = ContentItemConverter.RepositoryContent2ContentItem(item);
			}

			return retVal;
		}

		public void SaveContentItem(ContentItem item)
		{
			var existingItem = GetItem(item.Path).Result;

			//var contentUTF8Bytes = Encoding.UTF8.GetBytes(item.Content);
			var sha = String.Empty;

			if (existingItem == null) // create new
			{
				var response = _client.Repository.Content.CreateFile(
					_ownerName,
					_repositoryName,
					item.Path,
					new CreateFileRequest("Updating file from admin", item.Content)).Result;
			}
			else // update existing
			{
				var response = _client.Repository.Content.UpdateFile(
					_ownerName,
					_repositoryName,
					item.Path,
					new UpdateFileRequest("Updating file from admin", item.Content, existingItem.Sha)).Result;
			};
		}

		public void DeleteContentItem(ContentItem item)
		{
			var existingItem = GetItem(item.Path).Result;
			if (existingItem != null)
			{
				_client.Repository.Content.DeleteFile(
					_ownerName,
					_repositoryName,
					item.Path,
					new DeleteFileRequest("Updating file from admin", existingItem.Sha)).ConfigureAwait(true);
			}
		}

		private async Task<RepositoryContent> GetItem(string path)
		{
			try
			{
				var existingItems = await _client.Repository.Content.GetContents(_ownerName, _repositoryName, path);
				if (existingItems.Count == 0)
				{
					return null;
				}

				return existingItems.SingleOrDefault();
			}
			catch (NotFoundException)
			{
				return null;
			}
		}
	}
}
