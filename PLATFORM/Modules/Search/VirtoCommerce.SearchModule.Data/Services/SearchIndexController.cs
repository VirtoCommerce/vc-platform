using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Diagnostics.Contracts;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.SearchModule.Data.Services
{
	public class SearchIndexController : ISearchIndexController
	{
		private readonly ISearchProvider _searchProvider;
		private readonly ISearchIndexBuilder[] _indexBuilders;
		private readonly ISettingsManager _settingManager;

		public SearchIndexController(ISettingsManager settingManager, ISearchProvider searchProvider, ISearchIndexBuilder[] indexBuilders)
		{
			_settingManager = settingManager;
			_searchProvider = searchProvider;
			_indexBuilders = indexBuilders;
		}

		#region ISearchIndexController

		/// <summary>
		/// Processes the staged indexes.
		/// </summary>
		/// <param name="scope"></param>
		/// <param name="documentType"></param>
		public void Process(string scope, string documentType = "", bool rebuild = false)
		{
			if (scope == null)
			{
				throw new ArgumentNullException("scope");
			}

			var validBulders = _indexBuilders.Where(x => String.Equals(x.DocumentType, documentType, StringComparison.InvariantCultureIgnoreCase));

			var lastIndexTimeSetting = new SettingDescriptor
			{
				Name = String.Format("build_{0}_{1}", scope, documentType),
				ValueType = SettingValueType.DateTime
			};
			var lastBuildTime = _settingManager.GetValue<DateTime>(lastIndexTimeSetting.Name, DateTime.UtcNow);
			lastIndexTimeSetting.Value = lastBuildTime.ToString();

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

			_settingManager.SaveSettings(new SettingDescriptor[] { lastIndexTimeSetting });
		}
		#endregion


	}
}
