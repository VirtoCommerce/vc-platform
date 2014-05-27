
namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model
{
    public class ItemTypeHomeTab
    {
        public string Caption { get; set; }

        /// <summary>
        /// Localization category for the Caption
        /// </summary>
        public string Category { get; set; }
        public IViewModel ViewModel { get; set; }
        public int Order { get; set; }
        public string IdTab { get; set; }
    }
}
