using System.Linq;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using webModel = VirtoCommerce.CustomerModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Web.Converters
{
    public static class OrganizationConverter
    {
        public static webModel.Organization ToWebModel(this coreModel.Organization organization)
        {
            var retVal = new webModel.Organization();
            retVal.InjectFrom(organization);

            if (organization.Phones != null)
                retVal.Phones = organization.Phones;
            if (organization.Emails != null)
                retVal.Emails = organization.Emails;
            if (organization.Notes != null)
                retVal.Notes = organization.Notes.Select(x => x.ToWebModel()).ToList();
            if (organization.Addresses != null)
                retVal.Addresses = organization.Addresses.Select(x => x.ToWebModel()).ToList();
			if (organization.DynamicProperties != null)
				retVal.DynamicProperties = organization.DynamicProperties;

            return retVal;
        }

        public static coreModel.Organization ToCoreModel(this webModel.Organization organization)
        {
            var retVal = new coreModel.Organization();
            retVal.InjectFrom(organization);


            if (organization.Phones != null)
                retVal.Phones = organization.Phones;
            if (organization.Emails != null)
                retVal.Emails = organization.Emails;
            if (organization.Notes != null)
                retVal.Notes = organization.Notes.Select(x => x.ToCoreModel()).ToList();
            if (organization.Addresses != null)
                retVal.Addresses = organization.Addresses.Select(x => x.ToCoreModel()).ToList();
			if (organization.DynamicProperties != null)
				retVal.DynamicProperties = organization.DynamicProperties;

            return retVal;
        }


    }
}
