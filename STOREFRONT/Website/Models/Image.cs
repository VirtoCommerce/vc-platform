#region
using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

    [DataContract]
    public class Image : Drop
    {
        #region Public Properties
        [DataMember]
        public string Alt { get; set; }

        [DataMember]
        public int Position { get; set; }
        
        [DataMember]
        public string Src { get; set; }

        [DataMember]
        public string Name { get; set; }
        #endregion

        #region Public Methods and Operators
        public override string ToString()
        {
            return this.Src;
        }
        #endregion

        //image.variants
    }
}