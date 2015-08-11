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

        /// <summary>
        /// String definition of business category
        /// </summary>
        public string BusinessCategory { get; set; }

        /// <summary>
        /// Not documented
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Parent organization id
        /// </summary>
        public string ParentId { get; set; }

    }
}
