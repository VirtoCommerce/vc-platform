using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent language for specified culture
    /// </summary>
    public class Language 
    {
        private Language()
            :this(CultureInfo.InvariantCulture.Name)
        {
        }

        public Language(string cultureName)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            if (!string.IsNullOrEmpty(cultureName))
            {
                culture = CultureInfo.GetCultureInfo(cultureName);
            }
          
            CultureName = culture.Name;
            ThreeLeterLanguageName = culture.ThreeLetterISOLanguageName;
            TwoLetterLanguageName = culture.TwoLetterISOLanguageName;
            NativeName = culture.NativeName;
            if (culture != CultureInfo.InvariantCulture)
            {
                var regionInfo = new RegionInfo(culture.LCID);
                TwoLetterRegionName = regionInfo.TwoLetterISORegionName;
                ThreeLetterRegionName = regionInfo.ThreeLetterISORegionName;
            }
        }

        public static Language InvariantLanguage
        {
            get
            {
                return new Language();
            }
        }

        public bool IsInvariant
        {
            get
            {
                return CultureName == CultureInfo.InvariantCulture.Name;
            }
        }
        /// <summary>
        /// culture name format (e.g. en-US)
        /// </summary>
        public string CultureName { get; private set; }
        public string NativeName { get; private set; }
        /// <summary>
        ///  Gets the ISO 639-2 three-letter code for the language 
        /// </summary>
        public string ThreeLeterLanguageName { get; private set; }
        /// <summary>
        ///   Gets the ISO 639-1 two-letter code for the language
        /// </summary>
        public string TwoLetterLanguageName { get; private set; }
        /// <summary>
        ///  Gets the two-letter code defined in ISO 3166 for the country/region.
        /// </summary>
        public string TwoLetterRegionName { get; private set; }
        /// <summary>
        ///  Gets the three-letter code defined in ISO 3166 for the country/region.
        /// </summary>
        public string ThreeLetterRegionName { get; private set; }


        public static bool operator ==(Language left, Language right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Language left, Language right)
        {
            return !(left == right);
        }

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            var other = obj as Language;
            var code = obj as string;
            if (other != null)
            {
                return String.Equals(CultureName, other.CultureName, StringComparison.InvariantCultureIgnoreCase);
            }
            if (code != null)
            {
                return String.Equals(CultureName, code, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            return CultureName.ToUpper().GetHashCode();
        }



    }
}
