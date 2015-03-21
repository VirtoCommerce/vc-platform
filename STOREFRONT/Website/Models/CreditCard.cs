using DotLiquid;

namespace VirtoCommerce.Web.Models
{
    public class CreditCard : Drop
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string VerificationValue { get; set; }
    }
}