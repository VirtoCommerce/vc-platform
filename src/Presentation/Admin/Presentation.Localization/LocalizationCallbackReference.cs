namespace VirtoCommerce.ManagementClient.Localization
{
    /// <summary>
    /// Contains a reference to a <see cref="LocalizationCallback"/>.
    /// </summary>
    public class LocalizationCallbackReference
    {
        /// <summary>
        /// A reference to a <see cref="LocalizationCallback"/>.
        /// </summary>
        public event LocalizationCallback Callback;

        /// <summary>
        /// Gets the callback reference.
        /// </summary>
        /// <returns>The reference.</returns>
        internal LocalizationCallback GetCallback()
        {
            return Callback;
        }
    }
}
