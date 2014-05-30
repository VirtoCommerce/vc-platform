using System;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ConfigurationUtility.Application.Infrastructure
{
    class DummyElementRepository : IElementRepository
    {
        public IQueryable<CultureInfo> EnabledLanguages()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Element> Elements()
        {
            throw new NotImplementedException();
        }

        public Element Get(string name, string category, string culture)
        {
            return null;
        }

        public DateTime GetStatusDate()
        {
            throw new NotImplementedException();
        }

        public void SetStatusDate(DateTime lastModified = new DateTime())
        {
            throw new NotImplementedException();
        }

        public IQueryable<ElementCategory> Categories()
        {
            throw new NotImplementedException();
        }

        public bool Add(Element element)
        {
            return true;
        }

        public bool Update(Element element)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Element element)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void AddCategory(string category, string culture)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCategory(string category, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
