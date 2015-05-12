#region
using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Theme : Drop
    {
        #region Public Properties
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Role { get; set; }

        [DataMember]
        public string Path { get; set; }

        #endregion

        #region Public Methods and Operators
        public override string ToString()
        {
            return this.Name;
        }
        #endregion
    }
}