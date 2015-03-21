using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using webModel = VirtoCommerce.CustomerModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Web.Converters
{
	public static class ContactConverter
	{
		public static webModel.Contact ToWebModel(this coreModel.Contact contact)
		{
			var retVal = new webModel.Contact();
			retVal.InjectFrom(contact);

			if (contact.Phones != null)
				retVal.Phones = contact.Phones;
			if (contact.Emails != null)
				retVal.Emails = contact.Emails;
			if (contact.Properties != null)
				retVal.Properties = contact.Properties.Select(x => x.ToWebModel()).ToList();
			if (contact.Notes != null)
				retVal.Notes = contact.Notes.Select(x => x.ToWebModel()).ToList();
			if (contact.Addresses != null)
				retVal.Addresses = contact.Addresses.Select(x => x.ToWebModel()).ToList();

			retVal.Organizations = contact.Organizations;

			return retVal;
		}

		public static coreModel.Contact ToCoreModel(this webModel.Contact contact)
		{
			var retVal = new coreModel.Contact();
			retVal.InjectFrom(contact);


			if (contact.Phones != null)
				retVal.Phones = contact.Phones;
			if (contact.Emails != null)
				retVal.Emails = contact.Emails;
			if (contact.Properties != null)
				retVal.Properties = contact.Properties.Select(x => x.ToCoreModel()).ToList();
			if (contact.Notes != null)
				retVal.Notes = contact.Notes.Select(x => x.ToCoreModel()).ToList();
			if (contact.Addresses != null)
				retVal.Addresses = contact.Addresses.Select(x => x.ToCoreModel()).ToList();
			retVal.Organizations = contact.Organizations;
			
			return retVal;
		}


	}
}
