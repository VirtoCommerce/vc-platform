using System.Web;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Binders
{
    public class ResetPasswordModelBinder : BaseModelBinder<ResetPassword>
    {
        protected override void ComplementModel(ResetPassword model, HttpRequestBase request)
        {
            model.Password = request["customer[password]"];
            model.PasswordConfirmation = request["customer[password_confirmation]"];
        }
    }
}
