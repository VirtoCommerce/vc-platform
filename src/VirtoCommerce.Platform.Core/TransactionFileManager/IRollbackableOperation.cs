namespace VirtoCommerce.Platform.Core.TransactionFileManager
{
    /// <summary>
    /// Represents a transactional file operation.
    /// </summary>
    public interface IRollbackableOperation
    {
        /// <summary>
        /// Executes the operation.
        /// </summary>
        void Execute();

        /// <summary>
        /// Rolls back the operation, restores the original state.
        /// </summary>
        void Rollback();
    }
}
