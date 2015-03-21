#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Cart : Drop
    {
        #region Constructors and Destructors
        public Cart()
        {
            this.Items = new List<LineItem>();
        }
        #endregion

        #region Public Properties
        [DataMember]
        public string Attributes { get; set; }

        [DataMember]
        public int ItemCount
        {
            get
            {
                if (this.Items == null)
                {
                    return 0;
                }

                return this.Items.Count();
            }
        }

        [DataMember]
        public List<LineItem> Items { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public decimal TotalPrice
        {
            get
            {
                if (this.Items == null)
                {
                    return 0;
                }
                return this.Items.Sum(x => x.Quantity * x.Price);
            }
        }

        [DataMember]
        public decimal TotalWeight
        {
            get
            {
                return 0;
            }
        }
        #endregion
    }
}