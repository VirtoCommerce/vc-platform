using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.CatalogModule.Web.Model.Notifications;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class ImportJob
    {
	
        public string Id { get; set; }
        public string Name { get; set; }
        public string CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string TemplatePath { get; set; }
        public int MaxErrorsCount { get; set; }
        public int ImportStep { get; set; }
        public int ImportCount { get; set; }
        public int StartIndex { get; set; }
        public string ColumnDelimiter { get; set; }
        public string EntityImporter { get; set; } // product, sku, bundle, package, dynamickit, category, association, price, customer, inventory, itemrelation
        public string PropertySetId { get; set; }
        public ICollection<MappingItem> PropertiesMap { get; set; }
        public string[] AvailableCsvColumns { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ProgressStatus ProgressStatus { get; set; }

		public JobProgressInfo ProgressInfo { get; set; }

		[JsonIgnore]
		public ImportNotifyEvent NotifyEvent { get; set; }
		[JsonIgnore]
		public INotifier Notifier { get; set; }
		[JsonIgnore]
		public IImportService ImportService { get; set; }
		[JsonIgnore]
		public CancellationTokenSource CancellationToken;
		[JsonIgnore]
		public bool CanBeCanceled
		{
			get
			{
				return ProgressStatus == Model.ProgressStatus.Running;
			}
		}


    }
}
