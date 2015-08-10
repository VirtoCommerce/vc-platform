namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Menu link
	/// </summary>
	public class MenuLink
	{
		/// <summary>
		/// Id of menu link
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Title of menu link
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Url of menu link
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Priority of menu link
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// Is menu link active
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Menu link list parent id
		/// </summary>
		public string MenuLinkListId { get; set; }
	}
}