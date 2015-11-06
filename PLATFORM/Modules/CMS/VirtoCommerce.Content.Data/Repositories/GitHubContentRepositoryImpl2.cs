using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Converters;
using System.IO;

namespace VirtoCommerce.Content.Data.Repositories
{
    public class GitHubContentRepositoryImpl2 : IContentRepository2
    {
        private readonly GitHubClient _client;
        private readonly string _ownerName;
        private readonly string _repositoryName;
        //private readonly string _branchName = "master";
        private readonly string _mainPath;

        public GitHubContentRepositoryImpl2(
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

        public async Task<ContentItem> GetContent(string relativePath)
        {
            var fullPath = GetFullPath(relativePath);

            var result = await GetAllContents(fullPath);

            if (result == null)
            {
                return null;
            }

            var retVal = new ContentItem();
            var item = result.SingleOrDefault();
            if (item != null)
            {
                retVal = item.ToContentItem();
                retVal.Path = relativePath;
            }

            return retVal;
        }

        public async Task<ContentChangeSet> CreateFile(string relativePath, ContentItem item)
        {
            var fullPath = GetFullPath(relativePath);

            var response = await
                this._client.Repository.Content.CreateFile(
                    this._ownerName,
                    this._repositoryName,
                    fullPath,
                    new CreateFileRequest(string.Format("Create {0}", relativePath), Encoding.UTF8.GetString(item.ByteContent)));

            var createdItem = response.Content.ToContentItem();
            createdItem.Path = relativePath;

            var changeSet = new ContentChangeSet(response.Commit.Sha, new[] { new ContentItemInfo(createdItem) });
            return changeSet;
        }

        public async Task<ContentChangeSet> UpdateFile(string relativePath, ContentItem item)
        {
            var fullPath = GetFullPath(relativePath);
            var existingItem = await this.GetAllContents(fullPath);
            if (existingItem == null || existingItem.Count == 0)
            {
                throw new FileNotFoundException(string.Format("Can't update \"{0}\", file doesn't exist", relativePath));
            }

            var response = await this._client.Repository.Content.UpdateFile(
                    this._ownerName,
                    this._repositoryName,
                    fullPath,
                    new UpdateFileRequest(string.Format("Update {0}", relativePath), Encoding.UTF8.GetString(item.ByteContent), existingItem[0].Sha));


            var createdItem = response.Content.ToContentItem();
            createdItem.Path = relativePath;

            var changeSet = new ContentChangeSet(response.Commit.Sha, new[] { new ContentItemInfo(createdItem) });
            return changeSet;
        }

        public async Task DeleteFile(string relativePath)
        {
            var fullPath = GetFullPath(relativePath);

            var existingItem = await this.GetAllContents(fullPath);

            if (existingItem == null || existingItem.Count == 0)
            {
                return;
            }

            await this._client.Repository.Content.DeleteFile(
                this._ownerName,
                this._repositoryName,
                fullPath,
                new DeleteFileRequest(string.Format("Delete {0}", relativePath), existingItem[0].Sha));
        }

        public async Task<ContentChangeSet> GetChangeSet(string relativePath, IContentLoadCriteria criteria)
        {
            // try to get contents of the path first
            var result = await GetAllContents(GetFullPath(relativePath));

            // return nothing if there is not result
            if (result == null || result.Count == 0)
            {
                return null;
            }

            var sha1 = result[0].Sha;

            // get list of all sub trees
            var trees = await this._client.GitDatabase.Tree.GetRecursive(_ownerName, this._repositoryName, sha1);

            // get all blob entries
            var blobs = trees.Tree.Where(x => x.Type == TreeType.Blob);

            // TODO: sha1 here is incorrect, need to get the latest commit sha1 instead
            var items = blobs.Select(blob => blob.ToContentItem()).ToArray();
            var changeSet = new ContentChangeSet(sha1, items.Select(x => new ContentItemInfo(x)).ToArray());

            return changeSet;
        }

        #region private methods
        private string GetFullPath(string path)
        {
            return string.Format("{0}{1}", _mainPath, path);
        }

        private async Task<IReadOnlyList<RepositoryContent>> GetAllContents(string path)
        {
            IReadOnlyList<RepositoryContent> result = null;

            try
            {
                result = await this._client.Repository.Content.GetAllContents(this._ownerName, this._repositoryName, path);
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
