using Omu.ValueInjecter;
using VirtoCommerce.CatalogModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class MappingItemConverter
    {
        public static MappingItem ToWebModel(this foundation.MappingItem core)
        {
            var retVal = new MappingItem();
            retVal.InjectFrom(core);
            retVal.Id = core.MappingItemId;
            return retVal;
        }

        public static foundation.MappingItem ToFoundation(this MappingItem webEntity)
        {
            var retVal = new foundation.MappingItem();
            retVal.InjectFrom(webEntity);
            retVal.MappingItemId = webEntity.Id;
            return retVal;
        }
    }
}
