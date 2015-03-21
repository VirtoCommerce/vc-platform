namespace VirtoCommerce.ApiWebClient.Caching.Interfaces
{
    public interface IActionSettingsSerialiser
    {
        /// <summary>
        /// Implementations should serialize as string the specified action settings.
        /// </summary>
        /// <param name="actionSettings">The action settings.</param>
        /// <returns>A string representing the given <see cref="actionSettings"/></returns>
        string Serialise(ActionSettings actionSettings);

        /// <summary>
        /// Implementations should deserializes the specified serialized action settings.
        /// </summary>
        /// <param name="serialisedActionSettings">The serialized action settings.</param>
        /// <returns>An <see cref="ActionSettings"/> object</returns>
        ActionSettings Deserialise(string serialisedActionSettings);
    }
}
