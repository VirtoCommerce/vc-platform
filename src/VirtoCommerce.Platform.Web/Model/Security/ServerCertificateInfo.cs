namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ServerCertificateInfo
    {
        /// <summary>
        /// Certificate will expire soon
        /// </summary>
        public bool IsNearToBeExpired { get; set; }
        /// <summary>
        /// Indicates the certificate is distributive default (initial run)
        /// </summary>
        public bool IsDefaultVirtoSelfSigned { get; set; }
        /// <summary>
        /// Indicates the certificate load from DB
        /// </summary>
        public bool IsStoredInDb { get; set; }

        /// <summary>
        /// Indicates currently run certificate, and the stored in database are not the same (platform instance should be restarted)
        /// </summary>
        public bool IsChanged { get; set; }
    }
}
