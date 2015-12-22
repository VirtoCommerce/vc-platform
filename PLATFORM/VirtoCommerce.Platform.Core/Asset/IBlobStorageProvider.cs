using System.IO;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Asset
{
    /// <summary>
    /// Represent abstraction for working with binary data
    /// </summary>
    public interface IBlobStorageProvider
    {
        /// <summary>
        /// Search blobs by specified criteria
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        BlobSearchResult Search(string folderUrl, string keyword);

        /// <summary>
        /// Get blog info by url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        BlobInfo GetBlobInfo(string blobUrl);

        /// <summary>
        /// Create blob folder in specified provider
        /// </summary>
        /// <param name="folder"></param>
        void CreateFolder(BlobFolder folder);

        /// <summary>
        /// Open blob for reading
        /// </summary>
        /// <param name="blobUrl">Realative or absolute blob url (tmp/blob.txt) </param>
        /// <returns></returns>
        Stream OpenRead(string blobUrl);

        /// <summary>
        /// Open blob for writing
        /// </summary>
        /// <param name="blobUrl">Realative or absolute blob url (tmp/blob.txt)</param>
        /// <returns></returns>
        Stream OpenWrite(string blobUrl);

        /// <summary>
        /// Remove secified blobs
        /// </summary>
        /// <param name="urls"></param>
        void Remove(string[] urls);
    }
}
