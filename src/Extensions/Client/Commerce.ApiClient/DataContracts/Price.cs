using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class Price
    {
        public string PricelistId { get; set; }
        public string ProductId { get; set; }
        public decimal? Sale { get; set; }
        public decimal List { get; set; }
        public int MinQuantity { get; set; }
    }
}
