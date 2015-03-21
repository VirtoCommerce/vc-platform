namespace VirtoCommerce.ApiClient.DataContracts
{
    /// <summary>
    ///     A base class for relation links
    /// </summary>
    public class Link
    {
        #region Constructors and Destructors

        public Link(string relation, string href, string title = null)
        {
            Rel = relation;
            Href = href;
            Title = title;
        }

        #endregion

        #region Public Properties

        public string Href { get; private set; }

        public string Rel { get; private set; }

        public string Title { get; private set; }

        #endregion
    }
}
