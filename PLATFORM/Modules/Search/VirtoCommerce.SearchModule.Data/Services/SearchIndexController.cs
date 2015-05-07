using System;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.SearchModule.Data.Services
{
    public class SearchIndexController : ISearchIndexController
    {
        private readonly ISearchIndexBuilder[] _indexBuilders;
        private readonly ISettingsManager _settingManager;

        public SearchIndexController(ISettingsManager settingManager, params ISearchIndexBuilder[] indexBuilders)
        {
            _settingManager = settingManager;
            _indexBuilders = indexBuilders;
        }

        #region ISearchIndexController

        /// <summary>
        /// Processes the staged indexes.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="documentType"></param>
        /// <param name="rebuild"></param>
        public void Process(string scope, string documentType, bool rebuild)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }

            var validBulders = _indexBuilders.Where(x => String.Equals(x.DocumentType, documentType, StringComparison.InvariantCultureIgnoreCase));

            var lastBuildTimeName = string.Format(CultureInfo.InvariantCulture, "VirtoCommerce.Search.LastBuildTime_{0}_{1}", scope, documentType);
            var lastBuildTime = _settingManager.GetValue(lastBuildTimeName, DateTime.MinValue);
            var nowUtc = DateTime.UtcNow;

            foreach (var indexBuilder in validBulders)
            {
                var partitions = indexBuilder.GetPartitions(scope, lastBuildTime);
                foreach (var partition in partitions)
                {
                    if (partition.OperationType == OperationType.Remove)
                    {
                        indexBuilder.RemoveDocuments(scope, partition.Keys);
                    }
                    else
                    {
                        // create index docs
                        var docs = indexBuilder.CreateDocuments(partition);

                        // submit docs to the provider
                        var docsArray = docs.ToArray();
                        indexBuilder.PublishDocuments(scope, docsArray);
                    }
                }
            }

            _settingManager.SetValue(lastBuildTimeName, nowUtc);
        }

        #endregion
    }
}
