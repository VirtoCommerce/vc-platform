namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ResetPasswordInfo: ResetPasswordTokenInfo
    {
        public string NewPassword { get; set; }

        public bool ForcePasswordChangeOnFirstLogin { get; set; }
    }
}
