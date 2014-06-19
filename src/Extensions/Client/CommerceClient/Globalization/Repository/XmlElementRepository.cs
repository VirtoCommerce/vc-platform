using System;
using System.IO;
using System.Linq;

namespace VirtoCommerce.Client.Globalization.Repository
{
    /// <summary>
    /// Class XmlElementRepository.
    /// </summary>
    public class XmlElementRepository : IElementRepository, IDisposable
    {
        #region Fields
        /// <summary>
        /// The path
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// The ResXResource
        /// </summary>
        readonly ResXResource _resx;

        private const string StatusDateKeyInner = "~1ntern@|_st@tus_c@che_key!";
        private const string DefaultResource = "CM.en-US";
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlElementRepository" /> class.
        /// </summary>
        public XmlElementRepository()
            : this(Path.Combine(Settings.BaseDirectory, "I18N"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlElementRepository" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public XmlElementRepository(string path)
        {
            _path = path;
            _resx = new ResXResource(path);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enabled languages.
        /// </summary>
        /// <returns>IQueryable{CultureInfo}.</returns>
        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return _resx.GetCultures().Distinct().AsQueryable();
        }

        /// <summary>
        /// Elements of this instance.
        /// </summary>
        /// <returns>IQueryable{Element}.</returns>
        public IQueryable<Element> Elements()
        {
            return _resx.GetElements();
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Element.</returns>
        public Element Get(string name, string category, string culture)
        {
            return _resx.GetElement(name, category, culture);
        }

        public DateTime GetStatusDate()
        {
            DateTime result;
            var element = _resx.GetElement(StatusDateKeyInner, null, DefaultResource);
            return (element == null || !DateTime.TryParse(element.Value, out result)) ? DateTime.MinValue : result;
        }

        public void SetStatusDate(DateTime lastModified)
        {
            var element = _resx.GetElement(StatusDateKeyInner, null, DefaultResource);
            if (element == null)
            {
                element = new Element { Name = StatusDateKeyInner, Culture = DefaultResource, Value = DateTime.UtcNow.ToString() };
                _resx.AddResource(element);
            }
            else
            {
                element.Value = DateTime.UtcNow.ToString();
                _resx.UpdateResource(element);
            }
        }

        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool Add(Element element)
        {
            if (element == null)
            {
                return false;
            }

            if (_resx.GetElement(element.Name, element.Category, element.Culture) == null)
            {
                _resx.AddResource(element);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool Update(Element element)
        {
            return element != null && _resx.UpdateResource(element);
        }

        /// <summary>
        /// Removes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool Remove(Element element)
        {
            return element != null && _resx.RemoveResource(element);
        }

        /// <summary>
        /// Removes the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool RemoveCategory(string category, string culture)
        {
            return _resx.RemoveCategory(category, culture);
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        public void AddCategory(string category, string culture)
        {
            _resx.AddCategory(category, culture);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            var dir = new DirectoryInfo(_path);
            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles("*.resx"))
                {
                    file.Delete();
                }
            }
        }

        #region IElementRepository Members


        /// <summary>
        /// Categorieses this instance.
        /// </summary>
        /// <returns>IQueryable{ElementCategory}.</returns>
        public IQueryable<ElementCategory> Categories()
        {
            return _resx.GetCategories().AsQueryable();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        #endregion

        #endregion

    }
}
