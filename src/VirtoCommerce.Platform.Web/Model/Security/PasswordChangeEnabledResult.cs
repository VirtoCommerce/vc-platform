namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class PasswordChangeEnabledResult
    {
        public bool Enabled { get; set; }

        public PasswordChangeEnabledResult()
        {
        }

        public PasswordChangeEnabledResult(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
