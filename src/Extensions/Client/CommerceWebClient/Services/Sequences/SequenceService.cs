using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Frameworks.Sequences;

namespace VirtoCommerce.Web.Client.Services.Sequences
{
    /// <summary>
    /// Class SequenceService.
    /// </summary>
    public class SequenceService : ISequenceService
    {
        /// <summary>
        /// The _client
        /// </summary>
        private readonly SequencesClient _client;
        ///
        public SequenceService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceService"/> class.
        /// </summary>
        /// <param name="client">The sequences client.</param>
        public SequenceService(SequencesClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets the next sequence member.
        /// </summary>
        /// <param name="fullTypeName">Full name of the type.</param>
        /// <returns>Next unique sequence member</returns>
        public string GetNext(string fullTypeName)
        {
			return _client.GenerateNext(fullTypeName);
        }
    }
}
