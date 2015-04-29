using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Converters;

namespace VirtoCommerce.Content.Data.Repositories
{
	public class GitHubPagesRepositoryImpl : IPagesRepository
	{
		private readonly GitHubClient _client;

		private readonly string _ownerName;

		private readonly string _repositoryName;

		private readonly string _baseDirectoryPath;

		public GitHubPagesRepositoryImpl(
			string login,
			string password,
			string productHeaderValue,
			string ownerName,
			string repositoryName,
			string baseDirectoryPath)
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
			this._baseDirectoryPath = baseDirectoryPath;
		}

		public Models.Page GetPage(string path)
		{
			var fullPath = GetFullPath(path);

			var retVal = new Models.Page();
			var result = this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, fullPath).Result;

			var item = result.SingleOrDefault();
			if (item != null)
			{
				retVal = item.ToPageModel();
			}

			return retVal;
		}

		public IEnumerable<Models.Page> GetPages(string path)
		{
			var retVal = new List<Models.Page>();

			var fullPath = GetFullPath(path);

			var result = this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, fullPath).Result;

			var files = result.Where(s => s.Type == ContentType.File);

			Parallel.ForEach(files, file =>
			{
				var commits = this._client.
					Repository.
					Commits.
					GetAll(this._ownerName, this._repositoryName, new CommitRequest { Path = file.Path }).Result;

				var commit = commits.First();
				var date = commit.Commit.Committer.Date;

				retVal.Add(file.ToShortModel(date.DateTime));
			});

			return retVal;
		}

		public void SavePage(string path, Models.Page page)
		{
			var fullPath = GetFullPath(path);

			var existingItem = this.GetItem(fullPath).Result;

			var sha = String.Empty;

			if (existingItem == null) // create new
			{
				var response =
					this._client.Repository.Content.CreateFile(
						this._ownerName,
						this._repositoryName,
						fullPath,
						new CreateFileRequest("Updating file from admin", page.Content)).Result;
			}
			else // update existing
			{
				var response =
					this._client.Repository.Content.UpdateFile(
						this._ownerName,
						this._repositoryName,
						fullPath,
						new UpdateFileRequest("Updating file from admin", page.Content, existingItem.Sha)).Result;
			}
		}

		public void DeletePage(string path)
		{
			var fullPath = GetFullPath(path);

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

		private string GetFullPath(string path)
		{
			return string.Format("{0}{1}", _baseDirectoryPath, path);
		}
	}
}
