namespace VirtoCommerce.Platform.Core.TransactionFileManager
{
    /// <summary>
    /// port from https://github.com/rsevil/Transactions
    /// </summary>
    public interface ITransactionFileManager
    {
        /// <summary>
        /// Creates all directories in the specified path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Deletes the specified directory/file and all its contents. An exception is not thrown if the directory/file does not exist.
        /// </summary>
        /// <param name="path">The directory to be deleted.</param>
        void Delete(string path);

        /// <summary>
        /// Delete all files and directories safely
        /// </summary>
        /// <param name="path"></param>
        void SafeDelete(string path);
    }
}
