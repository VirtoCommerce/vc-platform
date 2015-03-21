namespace VirtoCommerce.Content.Data.Repositories
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
	using System.Text;

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

		public async Task<ContentItem> GetContentItem(string path)
		{
			var fullPath = GetFullPath(path);

			var retVal = new ContentItem();
			var result = await this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, fullPath);

			var item = result.SingleOrDefault();
			if (item != null)
			{
				retVal = item.ToContentItem();
				retVal.Path = path;
			}

			return retVal;
		}

		public async Task<IEnumerable<Theme>> GetThemes(string storePath)
		{
			var fullPath = GetFullPath(storePath);

			var result = await this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, fullPath);

			var themes = result.Where(s => s.Type == ContentType.Dir);

			List<Theme> list = new List<Theme>();

			foreach (var theme in themes)
			{
				var commits = await this._client.
					Repository.
					Commits.
					GetAll(this._ownerName, this._repositoryName, new CommitRequest { Path = theme.Path });

				var commit = commits.First();
				var date = commit.Commit.Committer.Date;

				list.Add(new Theme
				{
					Name = theme.Name,
					ThemePath = FixPath(theme.Path),
					ModifiedDate = date.DateTime
				});
			}

			return list;
		}

		public async Task<IEnumerable<ContentItem>> GetContentItems(string path, GetThemeAssetsCriteria criteria)
		{
			var fullPath = GetFullPath(path);

			var result = await this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, fullPath);

			var items = result.Where(s => s.Type == ContentType.Dir || s.Type == ContentType.File);

			var directories = items.Where(s => s.Type == ContentType.Dir).Select(s => s.Path);
			var files = items.Where(s => s.Type == ContentType.File).Select(file => file.ToContentItem()).ToList();

			var directoriesQueue = new Queue<string>();

			foreach (var directory in directories)
			{
				directoriesQueue.Enqueue(directory);
			}

			while (directoriesQueue.Count > 0)
			{
				var directory = directoriesQueue.Dequeue();
				result = await this._client.Repository.Content.GetContents(this._ownerName, this._repositoryName, directory);

				var results = result.Where(s => s.Type == ContentType.Dir || s.Type == ContentType.File);

				var newDirectories = results.Where(s => s.Type == ContentType.Dir).Select(s => s.Path);
				var newFiles = results.Where(s => s.Type == ContentType.File).Select(file => file.ToContentItem());

				foreach (var newDirectory in newDirectories)
				{
					directoriesQueue.Enqueue(newDirectory);
				}

				files.AddRange(newFiles);
			}

			foreach (var file in files)
			{
				file.Path = FixPath(file.Path);
			}

			if (criteria.LoadContent)
			{
				Parallel.ForEach(files, async file =>
				{
					var fullFile = await GetContentItem(file.Path);
					file.Content = fullFile.Content;
				});
			}

			return files;
		}

		public async Task<bool> SaveContentItem(string path, ContentItem item)
		{
			var fullPath = GetFullPath(path);

			var existingItem = this.GetItem(fullPath).Result;

			var sha = String.Empty;

			if (existingItem == null) // create new
			{
				var response = await
					this._client.Repository.Content.CreateFile(
						this._ownerName,
						this._repositoryName,
						fullPath,
						new CreateFileRequest("Updating file from admin", item.Content));
			}
			else // update existing
			{
				var response = await
					this._client.Repository.Content.UpdateFile(
						this._ownerName,
						this._repositoryName,
						fullPath,
						new UpdateFileRequest("Updating file from admin", Encoding.UTF8.GetString(item.ByteContent), existingItem.Sha));
			}

			return true;
		}

		public async Task<bool> DeleteContentItem(string path)
		{
			var fullPath = GetFullPath(path);

			var existingItem = this.GetItem(fullPath).Result;
			if (existingItem != null)
			{
				await this._client.Repository.Content.DeleteFile(
					this._ownerName,
					this._repositoryName,
					fullPath,
					new DeleteFileRequest("Updating file from admin", existingItem.Sha));
			}

			return true;
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

		private string GetFullPath(string path)
		{
			return string.Format("{0}{1}", _mainPath, path);
		}

		private string FixPath(string path)
		{
			return path.Replace(_mainPath, string.Empty).TrimStart('/');
		}

		#endregion
	}
}