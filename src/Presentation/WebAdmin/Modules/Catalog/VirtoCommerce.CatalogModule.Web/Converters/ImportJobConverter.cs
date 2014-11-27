using Omu.ValueInjecter;
using VirtoCommerce.CatalogModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class ImportJobConverter
    {
        public static ImportJob ToWebModel(this foundation.ImportJob core)
        {
            var retVal = new ImportJob();
            retVal.InjectFrom(core);
            retVal.Id = core.ImportJobId;
            return retVal;
        }

        public static foundation.ImportJob ToFoundation(this ImportJob webEntity)
        {
            var retVal = new foundation.ImportJob();
            retVal.InjectFrom(webEntity);
            retVal.ImportJobId = webEntity.Id;
            return retVal;
        }
    }
}
