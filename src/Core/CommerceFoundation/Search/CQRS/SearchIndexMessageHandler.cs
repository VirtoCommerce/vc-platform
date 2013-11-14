using System.Collections.Generic;
using System.Diagnostics;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Model;
using System;
using System.Linq;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using Microsoft.Practices.ServiceLocation;
using System.Diagnostics.Contracts;

namespace VirtoCommerce.Foundation.Search.CQRS
{
	[MessageType(typeof(SearchIndexMessage))]
    [MessageType(typeof(SearchIndexStatusMessage))]
	public class SearchIndexMessageHandler : IConsume
	{
		protected IMessageSender MessageSender;
        private readonly IBuildSettingsRepository _repository;
        private readonly ISearchIndexBuilder[] _indexBuilders;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchIndexMessageHandler"/> class.
        /// </summary>
        /// <param name="messageSender">The message sender.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="indexBuilders">The index builders.</param>
        public SearchIndexMessageHandler(IMessageSender messageSender, IBuildSettingsRepository repository, ISearchIndexBuilder[] indexBuilders)
		{
			MessageSender = messageSender;
            _repository = repository;
            _indexBuilders = indexBuilders;
		}

        /// <summary>
        /// Consumes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
		public virtual void Consume(IMessage message)
		{
			if (message is SearchIndexMessage)
			{
				var itemsIndexMessage = message as SearchIndexMessage;

                var builder = GetIndexBuilder(itemsIndexMessage.DocumentType);

                if (builder == null)
                    return;

			    if (itemsIndexMessage.Partition == null)
			    {
                    Trace.TraceInformation(String.Format("No partition contained in the message"));
			        return;
			    }

			    if (itemsIndexMessage.Partition.OperationType == OperationType.Remove)
                {
                    //Trace.TraceInformation(String.Format("Removing documents with specified keys from {0}", itemsIndexMessage.Scope));
                    builder.RemoveDocuments(itemsIndexMessage.Scope, itemsIndexMessage.Partition.Keys);
                }
                else
                {
                    // create index docs
                    //Trace.TraceInformation(String.Format("Creating docs for the partition for {0}", itemsIndexMessage.Scope));
                    var docs = builder.CreateDocuments(itemsIndexMessage.Partition);

                    // submit docs to the provider
                    var docsArray = docs.ToArray();
                    //Trace.TraceInformation(String.Format("Submitting {0} documents to search provider for {1}, {2} of {3}", docsArray.Count(), itemsIndexMessage.Scope, itemsIndexMessage.Partition.Start, itemsIndexMessage.Partition.Total));
                    builder.PublishDocuments(itemsIndexMessage.Scope, docsArray);
                }
			}
            else if (message is SearchIndexStatusMessage) // need to save the status
            {
                var m = message as SearchIndexStatusMessage;
                Trace.TraceInformation(String.Format("Updating index status for {0} and type {1}", m.Scope, m.DocumentType));
                var config = GetBuildConfig(_repository, m.Scope, m.DocumentType);
                config.Status = m.Status.GetHashCode();
                _repository.UnitOfWork.Commit();
            }
		}

        /// <summary>
        /// Gets the index builder.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
		private ISearchIndexBuilder GetIndexBuilder(string documentType)
		{
            Contract.Requires(!String.IsNullOrEmpty(documentType));

            foreach (var indexer in _indexBuilders)
            {
                // skip not requested indexers or index using all if indexer is not specified
                if (!String.IsNullOrEmpty(documentType) && !documentType.Equals(indexer.DocumentType))
                    continue;

                return indexer;
            }

            return null;
		}

        /// <summary>
        /// Gets the build config.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
        private BuildSettings GetBuildConfig(IBuildSettingsRepository repository, string scope, string documentType)
        {
            Contract.Requires(repository != null);
            Contract.Requires(!String.IsNullOrEmpty(documentType));
            Contract.Requires(!String.IsNullOrEmpty(scope));

            var key = String.Format("build_{0}_{1}", scope, documentType);

            var buildConfig = repository.BuildSettings.Where(x => (x.Scope.Equals(scope, StringComparison.OrdinalIgnoreCase) && x.DocumentType.Equals(documentType, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();

            if (buildConfig == null)
            {
                buildConfig = new BuildSettings(scope, documentType)
                    {
                        BuildSettingId = key,
                        Status = BuildStatus.NeverStarted.GetHashCode()
                    };
                repository.Add(buildConfig);
            }

            return buildConfig;
        }
	}
}
