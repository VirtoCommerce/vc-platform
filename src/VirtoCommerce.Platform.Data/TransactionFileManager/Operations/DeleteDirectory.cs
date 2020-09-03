using System;
using System.IO;
using VirtoCommerce.Platform.Core.TransactionFileManager;

namespace VirtoCommerce.Platform.Data.TransactionFileManager.Operations
{
    /// <summary>
    /// Deletes the specified directory and all its contents.
    /// </summary>
    sealed class DeleteDirectory : IRollbackableOperation, IDisposable
    {
        private readonly string path;
        private string backupPath;
        // tracks whether Dispose has been called
        private bool disposed;

        /// <summary>
        /// Instantiates the class.
        /// </summary>
        /// <param name="path">The directory path to delete.</param>
        public DeleteDirectory(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Disposes the resources used by this class.
        /// </summary>
        ~DeleteDirectory()
        {
            InnerDispose();
        }

        public void Execute()
        {
            if (Directory.Exists(path))
            {
                var temp = TransactionFileUtils.GetTempFileName(string.Empty);
                TransactionFileUtils.EnsureTempFolderExists();
                MoveDirectory(path, temp);
                backupPath = temp;
            }
        }

        public void Rollback()
        {
            if (Directory.Exists(backupPath))
            {
                string parentDirectory = Path.GetDirectoryName(path);
                if (!Directory.Exists(parentDirectory))
                {
                    Directory.CreateDirectory(parentDirectory);
                }
                MoveDirectory(backupPath, path);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            InnerDispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Moves a directory, recursively, from one path to another.
		/// This is a version of <see cref="Directory.Move"/> that works across volumes.
        /// </summary>
        private static void MoveDirectory(string sourcePath, string destinationPath)
        {
            if (Directory.GetDirectoryRoot(sourcePath) == Directory.GetDirectoryRoot(destinationPath))
            {
                // The source and destination volumes are the same, so we can do the much less expensive Directory.Move.
                Directory.Move(sourcePath, destinationPath);
            }
            else
            {
                // The source and destination volumes are different, so we have to resort to a copy/delete.
                CopyDirectory(new DirectoryInfo(sourcePath), new DirectoryInfo(destinationPath));
                Directory.Delete(sourcePath, true);
            }
        }

        private static void CopyDirectory(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
            }

            foreach (FileInfo sourceFile in sourceDirectory.GetFiles())
            {
                sourceFile.CopyTo(Path.Combine(destinationDirectory.FullName, sourceFile.Name));
            }

            foreach (DirectoryInfo sourceSubDirectory in sourceDirectory.GetDirectories())
            {
                string destinationSubDirectoryPath = Path.Combine(destinationDirectory.FullName, sourceSubDirectory.Name);
                CopyDirectory(sourceSubDirectory, new DirectoryInfo(destinationSubDirectoryPath));
            }
        }

        /// <summary>
        /// Disposes the resources of this class.
        /// </summary>
        private void InnerDispose()
        {
            if (!disposed)
            {
                if (Directory.Exists(backupPath))
                {
                    Directory.Delete(backupPath, true);
                }

                disposed = true;
            }
        }
    }
}
