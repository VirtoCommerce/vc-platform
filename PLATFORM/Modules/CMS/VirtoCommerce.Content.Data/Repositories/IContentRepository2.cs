using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Repositories
{
    public interface IContentRepository2
    {
        /// <summary>
        /// Returns a complete content file by relative url.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        Task<ContentItem> GetContent(string relativePath);

        /// <summary>
        /// Creates new file in the repository.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<ContentChangeSet> CreateFile(string relativePath, ContentItem item);

        /// <summary>
        /// Creates or updates file in the repository.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<ContentChangeSet> UpdateFile(string relativePath, ContentItem item);

        /// <summary>
        /// Deletes existing file in the repository.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        Task DeleteFile(string relativePath);

        /// <summary>
        /// Returns changes made since last update specified in the criteria.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<ContentChangeSet> GetChangeSet(string path, IContentLoadCriteria criteria);
    }

    public class ContentChangeSet
    {
        public ContentItemInfo[] Items { get; private set; }

        public string CommitId { get; private set; }

        public ContentChangeSet(string commitId, ContentItemInfo[] contentItems)
        {
            Items = contentItems;
            CommitId = commitId;
        }
    }

    public class ContentItemInfo
    {
        public ContentItem ContentItem { get; private set; }

        public ContentItemInfo(ContentItem item, bool deleted = false)
        {
            ContentItem = item;
            IsDeleted = deleted;
        }

        public bool IsDeleted { get; private set; }
    }

    public interface IContentLoadCriteria
    {
        bool LoadContents { get; set; }

        bool IsRecursive { get; set; }

        string LastCommitId { get; set; }
    }
}