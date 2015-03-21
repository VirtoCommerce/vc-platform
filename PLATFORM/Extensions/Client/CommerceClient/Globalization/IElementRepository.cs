using System;
using System.Globalization;
using System.Linq;

namespace VirtoCommerce.Client.Globalization
{
	/// <summary>
	/// Class ElementCategory.
	/// </summary>
	public class ElementCategory : IEquatable<ElementCategory>
	{
		#region Properties

		/// <summary>
		/// Gets or sets the category.
		/// </summary>
		/// <value>The category.</value>
		public string Category { get; set; }
		/// <summary>
		/// Gets or sets the culture.
		/// </summary>
		/// <value>The culture.</value>
		public string Culture { get; set; }
		#endregion

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		public bool Equals(ElementCategory other)
		{
			return string.Equals(Category, other.Category) &&
				   string.Equals(Culture, other.Culture);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return (Category == null ? 0 : Category.GetHashCode()) ^
				(Culture == null ? 0 : Culture.GetHashCode());
		}
	}
	/// <summary>
	/// Interface IElementRepository
	/// </summary>
	public interface IElementRepository
	{
		#region Methods
		/// <summary>
		/// Enabled the languages.
		/// </summary>
		/// <returns>IQueryable{CultureInfo}.</returns>
		IQueryable<CultureInfo> EnabledLanguages();

		/// <summary>
		/// Get elements of this instance.
		/// </summary>
		/// <returns>IQueryable{Element}.</returns>
		IQueryable<Element> Elements();

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>Element.</returns>
		Element Get(string name, string category, string culture);

		/// <summary>
		/// Gets the date when localization data was updated
		/// </summary>
		/// <returns>DateTime</returns>
		DateTime GetStatusDate();

		/// <summary>
		/// Set Status Date for the repository data
		/// </summary>
		/// <param name="lastModified">the latest data modification date</param>
		void SetStatusDate(DateTime lastModified = default(DateTime));

		/// <summary>
		/// Get Categories of this instance.
		/// </summary>
		/// <returns>IQueryable{ElementCategory}.</returns>
		IQueryable<ElementCategory> Categories();

		/// <summary>
		/// Adds the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		bool Add(Element element);

		/// <summary>
		/// Updates the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		bool Update(Element element);

		/// <summary>
		/// Removes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		bool Remove(Element element);

		/// <summary>
		/// Clears this instance.
		/// </summary>
		void Clear();

		/// <summary>
		/// Adds the category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		void AddCategory(string category, string culture);

		/// <summary>
		/// Removes the category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		bool RemoveCategory(string category, string culture);
		#endregion
	}
}
