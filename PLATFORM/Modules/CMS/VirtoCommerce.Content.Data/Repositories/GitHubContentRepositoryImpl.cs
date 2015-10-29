using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Converters;
using VirtoCommerce.Platform.Core.Common;
using System.IO;

namespace VirtoCommerce.Content.Data.Repositories
{
    public class GitHubContentRepositoryImpl : IContentRepository
    {
        private readonly GitHubClient _client;
        private readonly string _ownerName;
        private readonly string _repositoryName;
        //private readonly string _branchName = "master";
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

            var result = GetAllContents(fullPath);

            if (result == null)
            {
                return null;
            }

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
            var children = this.GetTreeChildren(storePath);

            // return nothing if there is not result
            if (children == null)
            {
                return Enumerable.Empty<Theme>();
            }

            var list = new List<Theme>();

            foreach (var theme in children)
            {
                list.Add(new Theme
                {
                    Name = Path.GetFileNameWithoutExtension(theme.Path),
                    ThemePath = FixPath(theme.Path)
                });
            }

            return list;
        }

        public IEnumerable<ContentItem> GetContentItems(string path, GetThemeAssetsCriteria criteria)
        {
            var blobs = GetBlobsRecursive(path);

            // return nothing if there is not result
            if (blobs == null)
            {
                return Enumerable.Empty<Models.ContentItem>();
            }

            var files = new List<Models.ContentItem>();

            Parallel.ForEach(
                blobs,
                file =>
                {
                    files.Add(file.ToContentItem());
                });

            foreach (var file in files)
            {
                file.Path = FixPath(file.Path);
            }

            if (criteria.LoadContent)
            {
                Parallel.ForEach(files, file =>
                {
                    var fullFile = GetContentItem(file.Path);
                    if (fullFile != null)
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
                        new CreateFileRequest(string.Format("Create {0}", path), Encoding.UTF8.GetString(item.ByteContent)))).Result;
            }
            else // update existing
            {
                var response = Task.Run(() => this._client.Repository.Content.UpdateFile(
                        this._ownerName,
                        this._repositoryName,
                        fullPath,
                        new UpdateFileRequest(string.Format("Update {0}", path), Encoding.UTF8.GetString(item.ByteContent), existingItem.Sha))).Result;
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
                    new DeleteFileRequest(string.Format("Delete {0}", path), existingItem.Sha)).Wait();
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

            var result = GetAllContents(fullPath);

            if (result == null)
            {
                return null;
            }

            var item = result.SingleOrDefault();
            if (item != null)
            {
                retVal = item.ToPageModel();
            }

            return retVal;
        }

        public IEnumerable<Models.ContentPage> GetPages(string path)
        {
            var blobs = GetBlobsRecursive(path);

            // return nothing if there is not result
            if (blobs == null)
            {
                return Enumerable.Empty<Models.ContentPage>();
            }

            var retVal = new List<Models.ContentPage>();

            Parallel.ForEach(
                blobs,
                file =>
                {
                    retVal.Add(file.ToShortModel());
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
                    Task.Run(() => this._client.Repository.Content.CreateFile(
                        this._ownerName,
                        this._repositoryName,
                        fullPath,
                        new CreateFileRequest(string.Format("Create {0}", path), Encoding.UTF8.GetString(page.ByteContent)))).Result;
            }
            else // update existing
            {
                var response =
                    Task.Run(() => this._client.Repository.Content.UpdateFile(
                        this._ownerName,
                        this._repositoryName,
                        fullPath,
                        new UpdateFileRequest(string.Format("Update {0}", path), Encoding.UTF8.GetString(page.ByteContent), existingItem.Sha))).Result;
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
                    new DeleteFileRequest(string.Format("Delete {0}", path), existingItem.Sha)).Wait();
            }
        }


        public IEnumerable<ContentPage> GetPages(string path, GetPagesCriteria criteria)
        {
            return GetPages(path);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Private GitHub methods
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

        public IEnumerable<TreeItem> GetBlobsRecursive(string path)
        {
            // try to get contents of the path first
            var result = GetAllContents(GetFullPath(path));

            // return nothing if there is not result
            if (result == null || result.Count == 0)
            {
                return null;
            }

            var sha1 = result[0].Sha;

            var retVal = new List<Models.ContentPage>();

            /*
            // get all branches first
            var branch = this._client.Repository.GetBranch(_ownerName, _repositoryName, _branchName).Result;

            if (branch == null) // branch doesn't exist
                return null;

            // get the root of latest commit, which will point to the root tree nide
            var sha1 = branch.Commit.Sha;
                            */

            // get list of all sub trees
            var trees = Task.Run(() => this._client.GitDatabase.Tree.GetRecursive(_ownerName, this._repositoryName, sha1)).Result;

            // get all blob entries
            var blobs = trees.Tree.Where(x => x.Type == TreeType.Blob);

            return blobs;
        }

        public IEnumerable<TreeItem> GetTreeChildren(string path)
        {
            // try to get contents of the path first
            var result = GetAllContents(GetFullPath(path));

            // return nothing if there is not result
            if (result == null || result.Count == 0)
            {
                return null;
            }

            var sha1 = result[0].Sha;

            var retVal = new List<Models.ContentPage>();

            // get list of all sub trees
            var tree = Task.Run(() => this._client.GitDatabase.Tree.Get(_ownerName, this._repositoryName, sha1)).Result;

            // get all blob entries
            var children = tree.Tree.Where(x => x.Type == TreeType.Tree);

            return children;
        }

        private IReadOnlyList<RepositoryContent> GetAllContents(string path)
        {
            IReadOnlyList<RepositoryContent> result = null;

            try
            {
                result = Task.Run(() => this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, path)).Result;
                //result = this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, path).Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is NotFoundException)
                {
                    return null;
                }

                throw;
            }

            return result;
        }

        #endregion
    }
}
