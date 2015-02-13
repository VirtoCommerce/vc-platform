﻿namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Octokit;

	using VirtoCommerce.Content.Data.Converters;
	using VirtoCommerce.Content.Data.Models;

	using ContentType = Octokit.ContentType;

	#endregion

	public class GitHubFileRepositoryImpl : IFileRepository
	{
		#region Fields

		private readonly GitHubClient _client;

		private readonly string _ownerName;

		private readonly string _repositoryName;

		private readonly string _mainPath;

		#endregion

		#region Constructors and Destructors

		public GitHubFileRepositoryImpl(
			string login,
			string password,
			string productHeaderValue,
			string ownerName,
			string repositoryName,
			string mainPath)
		{
			this._client = new GitHubClient(new ProductHeaderValue(productHeaderValue), new Uri("https://github.com/"))
						   {
							   Credentials
								   =
								   new Credentials
								   (
								   login,
								   password)
						   };

			this._repositoryName = repositoryName;
			this._ownerName = ownerName;
			this._mainPath = mainPath;

		}

		#endregion

		#region Public Methods and Operators

		public ContentItem GetContentItem(string themePath, string path)
		{
			var fullPath = GetFullPath(themePath, path);

			var retVal = new ContentItem();
			var result = this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, fullPath).Result;

			var item = result.SingleOrDefault();
			if (item != null)
			{
				retVal = ContentItemConverter.RepositoryContent2ContentItem(item);
				retVal.Path = FixPath(themePath, retVal.Path);
			}

			return retVal;
		}

		public ContentItem[] GetContentItems(string themePath, string path)
		{
			var fullPath = GetFullPath(themePath, path);

			var items = new List<ContentItem>();
			var result =
				this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, fullPath)
					.Result.Where(s => s.Type == ContentType.Dir || s.Type == ContentType.File);
			foreach (var item in result)
			{
				var addedItem = ContentItemConverter.RepositoryContent2ContentItem(item);
				if (addedItem != null)
				{
					items.Add(addedItem);
					addedItem.Path = FixPath(themePath, addedItem.Path);
				}
			}

			return items.ToArray();
		}

		public void SaveContentItem(string themePath, ContentItem item)
		{
			var fullPath = GetFullPath(themePath, item.Path);

			var existingItem = this.GetItem(fullPath).Result;

			//var contentUTF8Bytes = Encoding.UTF8.GetBytes(item.Content);
			var sha = String.Empty;

			if (existingItem == null) // create new
			{
				var response =
					this._client.Repository.Content.CreateFile(
						this._ownerName,
						this._repositoryName,
						fullPath,
						new CreateFileRequest("Updating file from admin", item.Content)).Result;
			}
			else // update existing
			{
				var response =
					this._client.Repository.Content.UpdateFile(
						this._ownerName,
						this._repositoryName,
						fullPath,
						new UpdateFileRequest("Updating file from admin", item.Content, existingItem.Sha)).Result;
			}
			;
		}

		public void DeleteContentItem(string themePath, ContentItem item)
		{
			var fullPath = GetFullPath(themePath, item.Path);

			var existingItem = this.GetItem(fullPath).Result;
			if (existingItem != null)
			{
				this._client.Repository.Content.DeleteFile(
					this._ownerName,
					this._repositoryName,
					fullPath,
					new DeleteFileRequest("Updating file from admin", existingItem.Sha)).Wait();
			}
		}

		#endregion

		#region Methods

		private async Task<RepositoryContent> GetItem(string path)
		{
			try
			{
				var existingItems =
					await this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, path);
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

		private string GetFullPath(string themePath, string path)
		{
			return string.Format("{0}{1}{2}", _mainPath, themePath, path);
		}

		private string FixPath(string themePath, string path)
		{
			return path.Replace(_mainPath, string.Empty).Replace(themePath, string.Empty).TrimStart('/');
		}

		#endregion
	}
}