using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using webModel = VirtoCommerce.QuoteModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.QuoteModule.Web.Converters
{
    public static class QuoteAttachmentConverter
    {
        public static webModel.QuoteAttachment ToWebModel(this coreModel.QuoteAttachment attachment)
        {
            var retVal = new webModel.QuoteAttachment();
            retVal.InjectFrom(attachment);

            return retVal;
        }

        public static coreModel.QuoteAttachment ToCoreModel(this webModel.QuoteAttachment attachment)
        {
            var retVal = new coreModel.QuoteAttachment();
            retVal.InjectFrom(attachment);
            return retVal;
        }


    }
}