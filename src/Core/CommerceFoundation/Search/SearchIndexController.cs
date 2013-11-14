using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.Foundation.Search.Model;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using System.Threading;
using VirtoCommerce.Foundation.Frameworks.CQRS.Events;
using System.Diagnostics.Contracts;

namespace VirtoCommerce.Foundation.Search
{
	public class SearchIndexController : ISearchIndexController
	{
		#region Private Variables
		private readonly IConsumerFactory _consumerFactory;
		private readonly ISystemObserver _observer;
		private readonly IQueueReader _queueReader;
		private readonly IMessageSender _messageSender;
		private readonly CancellationTokenSource _source = new CancellationTokenSource();
		private readonly IBuildSettingsRepository _repository;
        private readonly ISearchProvider _searchProvider;
        private readonly ISearchIndexBuilder[] _indexBuilders;
		#endregion

		#region ctor
        /// <summary>
        /// Azures the index builder.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="searchProvider">The search provider.</param>
        /// <param name="messageSender">The message sender.</param>
        /// <param name="consumerFactory">The consumer factory.</param>
        /// <param name="observer">The observer.</param>
        /// <param name="queueReader">The queue reader.</param>
        /// <param name="indexBuilders">The index builders.</param>
        public SearchIndexController(IBuildSettingsRepository repository, ISearchProvider searchProvider, IMessageSender messageSender, IConsumerFactory consumerFactory, ISystemObserver observer, IQueueReader queueReader, ISearchIndexBuilder[] indexBuilders)
		{
			_repository = repository;
			_messageSender = messageSender;
			_observer = observer;
			_queueReader = queueReader;
			_consumerFactory = consumerFactory;
            _searchProvider = searchProvider;
            _indexBuilders = indexBuilders;
		}
		#endregion

        #region ISearchIndexController
        /// <summary>
        /// Stages indexes. Should run only using one process.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="documentType"></param>
        /// <param name="rebuild"></param>
        /// <exception cref="System.ArgumentNullException">scope</exception>
        public void Prepare(string scope, string documentType = "", bool rebuild = false)
        {
            if (String.IsNullOrEmpty(scope))
                throw new ArgumentNullException("scope");

            foreach (var builder in _indexBuilders)
            {
                // skip not requested indexers or index using all if index is not specified
                if (!String.IsNullOrEmpty(documentType) && !documentType.Equals(builder.DocumentType))
                    continue;

                // Execute builder, which will create partitions and put them in the queue
                var queueName = new QueueName("index-{0}-{1}-in", scope, builder.DocumentType);

                var config = GetBuildConfig(_repository, queueName.Scope, queueName.DocumentType);

                var lastBuild = DateTime.UtcNow;
                var newBuildDate = lastBuild;
                if (config.Status == BuildStatus.NeverStarted.GetHashCode() || rebuild) // build was never started, so set min date
                {
                    rebuild = true;
                    lastBuild = DateTime.MinValue;
                    config.LastBuildDate = DateTime.UtcNow.AddYears(-30); // have to set the date to something repository won't complain
                }
                else
                {
                    lastBuild = config.LastBuildDate.AddSeconds(-30); // make sure we get all the changes 
                }

                // Delete all the records
                if (rebuild)
                {
                    _searchProvider.RemoveAll(queueName.Scope, queueName.DocumentType);
                }

                var partitions = builder.CreatePartitions(queueName.Scope, lastBuild);

                var newPartitionsExist = false; // tells if there are any partitions that has been processed
                foreach (var partition in partitions)
                {
                    newPartitionsExist = true;
                    //_observer.Notify(new ConsumeBegin(msg, consumer, envelope.QueueName));
                   _messageSender.Send(queueName.ToString(), partition);
                }

                var newBuildStatus = BuildStatus.Started;
                if (newPartitionsExist)
                {
                    _messageSender.Send(queueName.ToString(), new SearchIndexStatusMessage(queueName.Scope, queueName.DocumentType, BuildStatus.Completed));
                }
                else
                {
                    newBuildStatus = BuildStatus.Completed;
                }

                config.LastBuildDate = newBuildDate;
                config.Status = newBuildStatus.GetHashCode();
                _repository.UnitOfWork.Commit();
            }
        }

        /// <summary>
        /// Processes the staged indexes.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="documentType"></param>
        public void Process(string scope, string documentType = "")
        {
            Contract.Requires(!String.IsNullOrEmpty(scope));

            var queues = new List<QueueName>();
            foreach (var builder in _indexBuilders)
            {
                // skip not requested indexers or index using all if indexer is not specified
                if (!String.IsNullOrEmpty(documentType) && !documentType.Equals(builder.DocumentType))
                    continue;

                // Execute builder, which will create partitions and put them in the queue
                var queueName = new QueueName("index-{0}-{1}-in", scope, builder.DocumentType);

                // Add the queue
                queues.Add(queueName);
            }

            // Now start the queue messaging
            if (queues.Count > 0)
            {
                // Process messages
                ReceiveMessages(queues.ToArray());
            }
        }
        #endregion

		#region Private Methods
		/// <summary>
		/// Receives the messages.
		/// </summary>
		/// <param name="queues">The queues.</param>
		protected void ReceiveMessages(IEnumerable<QueueName> queues)
		{
		    foreach (var queue in queues)
			{
			    MessageEnvelope envelope;
			    while (_queueReader.TakeMessage(queue.ToString(), _source.Token, out envelope))
				{
					try
					{
						var msg = envelope.MessageReference.Message;
						var consumers = _consumerFactory.GetMessageConsumers(msg);
						foreach (var consumer in consumers)
						{
							_observer.Notify(new ConsumeBegin(msg, consumer, envelope.QueueName));
							consumer.Consume(msg);
							_observer.Notify(new ConsumeEnd(msg, consumer, envelope.QueueName));
						}
					}
					catch (Exception ex)
					{
                        Trace.TraceError(String.Format("Failed to process index messages with exception: {0}", ex.ToString()));
						_observer.Notify(new FailedToConsumeMessage(ex, envelope.EnvelopeId, envelope.QueueName));
					}
					try
					{
						_queueReader.DeleteMessage(envelope);
					}
					catch (Exception ex)
					{
                        Trace.TraceError(String.Format("Failed to delete index message with exception: {0}", ex.ToString()));
						// not a big deal. Message will be processed again.
						_observer.Notify(new FailedToAckMessage(ex, envelope.EnvelopeId, envelope.QueueName));
					}
				}
			}
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

		public class QueueName
		{
			public string Format { get; private set; }
			public string Scope { get; private set; }
			public string DocumentType { get; private set; }

			public QueueName(string format, string scope, string documentType)
			{
				Format = format;
				Scope = scope;
				DocumentType = documentType;
			}

			public override string ToString()
			{
				return String.Format(Format, Scope, DocumentType);
			}
		}

		#endregion
    }
}
