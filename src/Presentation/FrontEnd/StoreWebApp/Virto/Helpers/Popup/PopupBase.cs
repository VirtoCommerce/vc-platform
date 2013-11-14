namespace VirtoCommerce.Web.Virto.Helpers.Popup
{
	public enum PopupType
	{
		Button, 
        LinkButton
	}

    /// <summary>
    /// Base class for models that are shown in popup window
    /// </summary>
	public class PopupBase
	{
        /// <summary>
        /// Gets or sets the button text.
        /// </summary>
        /// <value>
        /// The button text.
        /// </value>
		public string ButtonText { get; set; }
        /// <summary>
        /// Gets or sets the button CSS class.
        /// </summary>
        /// <value>
        /// The button CSS class.
        /// </value>
		public string ButtonCssClass { get; set; }
        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
		public string ViewName { get; set; }
        /// <summary>
        /// Gets or sets the button click.
        /// </summary>
        /// <value>
        /// The button click.
        /// </value>
		public string ButtonClick { get; set; }
        /// <summary>
        /// Gets or sets the popup title.
        /// </summary>
        /// <value>
        /// The popup title.
        /// </value>
		public string PopupTitle { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
		public PopupType Type { get; set; }
	}
}