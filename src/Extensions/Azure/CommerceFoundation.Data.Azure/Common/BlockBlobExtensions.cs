using System;

namespace VirtoCommerce.Foundation.Data.Azure.Common
{
    using System.IO;

    using Microsoft.WindowsAzure.Storage.Blob;

    public static class BlockBlobExtensions
    {
        /// <summary>
        /// Creates the empty block blob.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        [CLSCompliant(false)]
        public static void Create(this CloudBlockBlob blob)
        {
            using (var ms = new MemoryStream())
            {
                blob.UploadFromStream(ms);//Empty memory stream. Will create an empty blob.
            }
        }
    }
}
