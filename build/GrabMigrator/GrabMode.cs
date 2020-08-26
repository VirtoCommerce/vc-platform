using System.Text.Json.Serialization;

namespace GrabMigrator
{
    /// <summary>
    /// How many migrations should be grabbed
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GrabMode
    {
        /// <summary>
        /// Only v2->v3 upgrade migrations
        /// </summary>
        V2V3,
        /// <summary>
        /// All module migrations
        /// </summary>
        All
    }
}
