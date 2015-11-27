using System.Web;
using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Binders
{
    public class AddressModelBinder : BaseModelBinder<Address>
    {
        protected override void ComplementModel(Address model, HttpRequestBase request)
        {
            model.FirstName = request["address[first_name]"];
            model.LastName = request["address[last_name]"];
            model.Organization = request["address[company]"];
            model.Line1 = request["address[address1]"];
            model.Line2 = request["address[address2]"];
            model.City = request["address[city]"];
            model.CountryName = request["address[country]"];
            model.RegionName = request["address[province]"];
            model.Zip = request["address[zip]"];
            model.Phone = request["address[phone]"];

            model.Method = request["_method"];
        }
    }
}
