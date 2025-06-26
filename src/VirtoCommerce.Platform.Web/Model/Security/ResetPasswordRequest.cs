namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
        public bool ForcePasswordChangeOnNextSignIn { get; set; }
    }
}
