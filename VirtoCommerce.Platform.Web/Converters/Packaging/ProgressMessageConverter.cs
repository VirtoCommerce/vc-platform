using webModel = VirtoCommerce.Platform.Web.Model.Packaging;
using moduleModel = VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Web.Converters.Packaging
{
    public static class ProgressMessageConverter
    {
        public static webModel.ProgressMessage ToWebModel(this moduleModel.ProgressMessage message)
        {
            var retVal = new webModel.ProgressMessage();
            retVal.Message = message.Message;
            retVal.Level = message.Level.ToString();
            return retVal;
        }
    }
}
