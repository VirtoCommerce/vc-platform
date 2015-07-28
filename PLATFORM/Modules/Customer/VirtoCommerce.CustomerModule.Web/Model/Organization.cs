using System.Collections.Generic;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class Organization : Member
    {
        public Organization()
            : base("Organization")
        {
        }

        public override string DisplayName
        {
            get { return Name; }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string BusinessCategory { get; set; }
        public string OwnerId { get; set; }
        public string ParentId { get; set; }

    }
}
