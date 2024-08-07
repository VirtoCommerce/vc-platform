namespace VirtoCommerce.Platform.Core.Security
{
    public class PasswordLoginOptions
    {
        /// <summary>
        /// Determines whether the user authentication by username/password is enabled.
        /// Always enabled unless explicitly disabled by the platform options.
        /// </summary>
        public bool Enabled { get; set; } = true;

        public string AuthenticationType { get; set; } = "Password";

        public bool DetailedErrors { get; set; }

        public int Priority { get; set; }
    }
}
