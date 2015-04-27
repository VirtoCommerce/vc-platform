namespace VirtoCommerce.Content.Web.Models
{
	public class MenuLink
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public int Priority { get; set; }
		public string Type { get; set; }
		public bool IsActive { get; set; }
		public string Language { get; set; }
		public string MenuLinkListId { get; set; }
	}
}