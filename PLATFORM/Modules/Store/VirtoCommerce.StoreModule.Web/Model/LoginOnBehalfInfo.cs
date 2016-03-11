namespace VirtoCommerce.StoreModule.Web.Model
{
    /// <summary>
    /// Represent result for checking of possibility login on behalf request
    /// </summary>
    public class LoginOnBehalfInfo
    {
        public string UserName { get; set; }
        public bool CanLoginOnBehalf { get; set; }
    }
}
