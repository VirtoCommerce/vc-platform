using System.Web;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Binders
{
    public class RegisterModelBinder : BaseModelBinder<Register>
    {
        protected override void ComplementModel(Register model, HttpRequestBase request)
        {
            model.FirstName = request["customer[first_name]"];
            model.LastName = request["customer[last_name]"];
            model.Email = request["customer[email]"];
            model.Password = request["customer[password]"];
        }
    }
}
