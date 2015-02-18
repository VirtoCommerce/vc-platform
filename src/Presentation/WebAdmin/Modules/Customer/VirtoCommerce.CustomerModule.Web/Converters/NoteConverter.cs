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
	public static class NoteConverter
	{
		public static webModel.Note ToWebModel(this coreModel.Note note)
		{
			var retVal = new webModel.Note();
			retVal.InjectFrom(note);
			return retVal;
		}

		public static coreModel.Note ToCoreModel(this webModel.Note note)
		{
			var retVal = new coreModel.Note();
			retVal.InjectFrom(note);
			return retVal;
		}


	}
}
