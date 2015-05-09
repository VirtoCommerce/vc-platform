namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public enum ChangeLogPolicy
    {
        /// <summary>
        /// Each object contains only one change record 
        /// </summary>
        Cumulative,
        /// <summary>
        /// Write new record for any object change
        /// </summary>
        Historical
    }
}
