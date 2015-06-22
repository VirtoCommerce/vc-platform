using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Web.Model
{
	public class StoreExportConfiguration
	{
		public string StoreId { get; set; }
        public bool IsDisablePamentMethods { get; set; }
        public bool IsDisableShipmentMethods { get; set; }
        public bool IsDisableSeo { get; set; }
        public bool IsDisableLanguages { get; set; }
        public bool IsDisableCurrencies { get; set; }
    }

}