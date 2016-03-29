using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.CustomerModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
    public static class AddressConverter
    {
        public static Domain.Commerce.Model.Address ToCoreModel(this dataModel.Address entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var retVal = new Domain.Commerce.Model.Address();
            retVal.InjectFrom(entity);
            retVal.Phone = entity.DaytimePhoneNumber;
            retVal.AddressType = EnumUtility.SafeParse(entity.Type, AddressType.BillingAndShipping);

            return retVal;
        }

        public static dataModel.Address ToDataModel(this Domain.Commerce.Model.Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var retVal = new dataModel.Address();
            retVal.InjectFrom(address);
            retVal.DaytimePhoneNumber = address.Phone;
            retVal.Type = address.AddressType.ToString();
            return retVal;
        }      
    }
}
