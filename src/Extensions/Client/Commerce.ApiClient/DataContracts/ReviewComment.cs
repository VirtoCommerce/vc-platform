using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class ReviewComment
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int PositiveFeedbackCount { get; set; }

        public int NegativeFeedbackCount { get; set; }

        public int AbuseCount { get; set; }
    }
}
