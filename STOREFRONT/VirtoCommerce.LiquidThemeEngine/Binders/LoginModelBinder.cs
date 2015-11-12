using System.Web;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Binders
{
    public class LoginModelBinder : BaseModelBinder<Login>
    {
        protected override void ComplementModel(Login model, HttpRequestBase request)
        {
            model.Email = request["customer[email]"];
            model.Password = request["customer[password]"];
        }
    }
}
