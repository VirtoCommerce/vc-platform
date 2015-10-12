using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.StoreModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Tax.Model;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.StoreModule.Data.Converters
{
    public static class StoreTaxProviderConverter
    {

        public static dataModel.StoreTaxProvider ToDataModel(this coreModel.TaxProvider taxProvider)
        {
            if (taxProvider == null)
                throw new ArgumentNullException("taxProvider");

            var retVal = new dataModel.StoreTaxProvider();

            retVal.InjectFrom(taxProvider);

            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.StoreTaxProvider source, dataModel.StoreTaxProvider target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            var patchInjectionPolicy = new PatchInjection<dataModel.StoreTaxProvider>(x => x.LogoUrl, x => x.Name,
                                                                           x => x.Description, x => x.Priority,
                                                                           x => x.IsActive);
            target.InjectFrom(patchInjectionPolicy, source);
        }


    }
}
