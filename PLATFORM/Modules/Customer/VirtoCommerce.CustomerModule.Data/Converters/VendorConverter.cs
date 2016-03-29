using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using dataModel = VirtoCommerce.CustomerModule.Data.Model;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
    public static class VendorConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public static void ToCoreModel(this dataModel.Vendor dbVendor, coreModel.Vendor vendor)
        {
            if (dbVendor == null)
                throw new ArgumentNullException("dbVendor");
            if (vendor == null)
                throw new ArgumentNullException("vendor");

            //Nothing todo
        }

        public static void ToDataModel(this coreModel.Vendor vendor, dataModel.Vendor dbVendor, PrimaryKeyResolvingMap pkMap)
        {
            if (dbVendor == null)
                throw new ArgumentNullException("dbVendor");
            if (vendor == null)
                throw new ArgumentNullException("vendor");
    
            pkMap.AddPair(vendor, dbVendor);
        }
    }
}
