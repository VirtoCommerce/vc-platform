using System.Collections.Generic;
using System.Linq;
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

            var mappings = new List<MappingItem>();
            foreach (var map in core.PropertiesMap)
            {
                var webMap = new MappingItem();
                webMap.InjectFrom(map);
                webMap.Id = map.MappingItemId;
                webMap.ImportJobId = core.ImportJobId;
                mappings.Add(webMap);
            }

            if (mappings.Any())
            {
                retVal.PropertiesMap = mappings;
            }
            
            return retVal;
        }

        public static foundation.ImportJob ToFoundation(this ImportJob webEntity)
        {
            var retVal = new foundation.ImportJob();
            retVal.InjectFrom(webEntity);
            retVal.ImportJobId = webEntity.Id;

            if (webEntity.PropertiesMap != null)
            {
                foreach (var map in webEntity.PropertiesMap)
                {
                    var coreMap = new foundation.MappingItem();
                    coreMap.InjectFrom(map);
                    coreMap.MappingItemId = map.Id;
                    coreMap.ImportJobId = webEntity.Id;
                    coreMap.ImportJob = retVal;
                    retVal.PropertiesMap.Add(coreMap);
                }
            }
            return retVal;
        }
    }
}
