using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Models.Banners;
using Data = VirtoCommerce.ApiClient.DataContracts.Contents;

namespace VirtoCommerce.Web.Convertors
{
    public static class BannerConverters
    {
        public static PlaceHolder AsWebModel(this Data.DynamicContentItemGroup group)
        {
            var ph = new PlaceHolder { Name = @group.GroupName, Banners = new BannerCollection(@group.Items.AsWebModel()) };
            return ph;
        }

        public static IEnumerable<PlaceHolder> AsWebModel(this IEnumerable<Data.DynamicContentItemGroup> groups)
        {
            return groups.Select(x => x.AsWebModel());
        }

        public static Banner AsWebModel(this Data.DynamicContentItem item)
        {
            var dc = new Banner();
            dc.InjectFrom(item);
            return dc;
        }

        public static T AsWebModel<T>(this Data.DynamicContentItem item) where T : Banner, new()
        {
            var dc = new T();
            dc.InjectFrom(item);
            return dc;
        }

        public static IEnumerable<Banner> AsWebModel(this IEnumerable<Data.DynamicContentItem> items)
        {
            return items.Select(x => x.AsWebModel());
        }
    }
}