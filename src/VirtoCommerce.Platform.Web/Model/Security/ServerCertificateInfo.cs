namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ServerCertificateInfo
    {
        public bool IsNearToBeExpired { get; set; }
        public bool IsDefaultVirtoSelfSigned { get; set; }
        public bool IsStoredInDb { get; set; }
    }
}
