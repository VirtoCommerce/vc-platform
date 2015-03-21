using System.Globalization;

namespace VirtoCommerce.Client.Globalization
{
    /// <summary>
    /// Class Element.
    /// </summary>
    public class Element
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// The _culture
        /// </summary>
        private string _culture;
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public string Culture
        {
            get
            {
                if (string.IsNullOrEmpty(_culture))
                {
                    _culture = "en-US";
                }
                return _culture;
            }
            set
            {
                _culture = value;
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            get;
            set;
        }

        /// <summary>
        /// The _ empty
        /// </summary>
        static Element _empty;
        /// <summary>
        /// Gets the empty element.
        /// </summary>
        /// <value>The empty element.</value>
        public static Element Empty
        {
            get
            {
                return _empty ?? (_empty = new Element
                    {
                        Name = "",
                        Value = "",
                        Category = "",
                        Culture = ""
                    });
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets the culture info.
        /// </summary>
        /// <returns>CultureInfo.</returns>
        public virtual CultureInfo GetCultureInfo()
        {
            return CultureInfo.GetCultureInfo(Culture);
        }
        #endregion
    }
}
