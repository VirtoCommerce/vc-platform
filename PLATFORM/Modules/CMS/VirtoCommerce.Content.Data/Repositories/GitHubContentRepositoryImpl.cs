using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Converters;

namespace VirtoCommerce.Content.Data.Repositories
{
    public class GitHubContentRepositoryImpl : IContentRepository
    {
        private readonly GitHubClient _client;
        private readonly string _ownerName;
        private readonly string _repositoryName;
        private readonly string _mainPath;

        public GitHubContentRepositoryImpl(
            string token,
            string productHeaderValue,
            string ownerName,
            string repositoryName,
            string mainPath)
        {
            this._client = new GitHubClient(new ProductHeaderValue(productHeaderValue), new Uri("https://github.com/"))
            {
                Credentials = new Credentials(token)
            };

            this._repositoryName = repositoryName;
            this._ownerName = ownerName;
            this._mainPath = mainPath;

        }

        public ContentItem GetContentItem(string path)
        {
            var fullPath = GetFullPath(path);

            var retVal = new ContentItem();
            var result = Task.Run(() => this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, fullPath)).Result;

            var item = result.SingleOrDefault();
            if (item != null)
            {
                retVal = item.ToContentItem();
                retVal.Path = path;
            }

            return retVal;
        }

        public IEnumerable<Theme> GetThemes(string storePath)
        {
            var fullPath = GetFullPath(storePath);

            var result = Task.Run(() => this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, fullPath)).Result;

            var themes = result.Where(s => s.Type == ContentType.Dir);

            List<Theme> list = new List<Theme>();

            foreach (var theme in themes)
            {
                var commits = Task.Run(() => this._client.
                    Repository.
                    Commits.
                    GetAll(this._ownerName, this._repositoryName, new CommitRequest { Path = theme.Path })).Result;

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

        public IEnumerable<ContentItem> GetContentItems(string path, GetThemeAssetsCriteria criteria)
        {
            var fullPath = GetFullPath(path);

            IReadOnlyList<RepositoryContent> result = null;

            try
            {
                result = Task.Run(() => this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, fullPath)).Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is NotFoundException)
                {
                    return Enumerable.Empty<ContentItem>();
                }

                throw;
            }

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
                result = Task.Run(() => this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, directory)).Result;

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
                Parallel.ForEach(files, file =>
                {
                    var fullFile = GetContentItem(file.Path);
                    file.ByteContent = fullFile.ByteContent;
                });
            }

            return files;
        }

        public void SaveContentItem(string path, ContentItem item)
        {
            var fullPath = GetFullPath(path);

            var existingItem = this.GetItem(fullPath);

            var sha = String.Empty;

            if (existingItem == null) // create new
            {
                var response = Task.Run(() =>
                    this._client.Repository.Content.CreateFile(
                        this._ownerName,
                        this._repositoryName,
                        fullPath,
                        new CreateFileRequest("Updating file from admin", Encoding.UTF8.GetString(item.ByteContent)))).Result;
            }
            else // update existing
            {
                var response = this._client.Repository.Content.UpdateFile(
                        this._ownerName,
                        this._repositoryName,
                        fullPath,
                        new UpdateFileRequest("Updating file from admin", Encoding.UTF8.GetString(item.ByteContent), existingItem.Sha)).Result;
            }
        }

        public void DeleteContentItem(string path)
        {
            var fullPath = GetFullPath(path);

            var existingItem = this.GetItem(fullPath);
            if (existingItem != null)
            {
                this._client.Repository.Content.DeleteFile(
                    this._ownerName,
                    this._repositoryName,
                    fullPath,
                    new DeleteFileRequest("Updating file from admin", existingItem.Sha)).Wait();
            }
        }

        private RepositoryContent GetItem(string path)
        {
            try
            {
                var existingItems =
                    Task.Run(() => this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, path)).Result;
                if (existingItems.Count == 0)
                {
                    return null;
                }

                return existingItems.SingleOrDefault();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is NotFoundException)
                {
                    return null;
                }

                throw;
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

        public void DeleteTheme(string path)
        {
            throw new NotImplementedException();
        }

        public Models.ContentPage GetPage(string path)
        {
            var fullPath = GetFullPath(path);

            var retVal = new Models.ContentPage();
            var result = this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, fullPath).Result;

            var item = result.SingleOrDefault();
            if (item != null)
            {
                retVal = item.ToPageModel();
            }

            return retVal;
        }

        public IEnumerable<Models.ContentPage> GetPages(string path)
        {
            var retVal = new List<Models.ContentPage>();

            var fullPath = GetFullPath(path);

            var result = this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, fullPath).Result;

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

        public void SavePage(string path, Models.ContentPage page)
        {
            var fullPath = GetFullPath(path);

            var existingItem = this.GetItem(fullPath);

            var sha = String.Empty;

            if (existingItem == null) // create new
            {
                var response =
                    this._client.Repository.Content.CreateFile(
                        this._ownerName,
                        this._repositoryName,
                        fullPath,
                        new CreateFileRequest("Updating file from admin", Encoding.UTF8.GetString(page.ByteContent))).Result;
            }
            else // update existing
            {
                var response =
                    this._client.Repository.Content.UpdateFile(
                        this._ownerName,
                        this._repositoryName,
                        fullPath,
                        new UpdateFileRequest("Updating file from admin", Encoding.UTF8.GetString(page.ByteContent), existingItem.Sha)).Result;
            }
        }

        public void DeletePage(string path)
        {
            var fullPath = GetFullPath(path);

            var existingItem = this.GetItem(fullPath);
            if (existingItem != null)
            {
                this._client.Repository.Content.DeleteFile(
                    this._ownerName,
                    this._repositoryName,
                    fullPath,
                    new DeleteFileRequest("Updating file from admin", existingItem.Sha)).Wait();
            }
        }


        public IEnumerable<ContentPage> GetPages(string path, GetPagesCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
