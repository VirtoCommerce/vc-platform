using DotLiquid;

namespace VirtoCommerce.Web.Models
{
    public class Language : Drop
    {
        public string Name { get; set; }

        public string NativeName { get; set; }

        public string DisplayName { get; set; }

        public string EnglishName { get; set; }

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}