using System;
using System.Net.Http;

namespace VirtoCommerce.ApiClient
{
    public sealed class CommerceClients
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the CommerceClients class.
        /// </summary>
        internal CommerceClients()
        {
        }

        #endregion

        #region Public Properties

        public Func<MessageProcessingHandler> CreateMessageProcessingHandler { get; set; }

        #endregion
    }
}
