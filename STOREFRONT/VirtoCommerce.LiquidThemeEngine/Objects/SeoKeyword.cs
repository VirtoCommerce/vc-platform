using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class SeoKeyword : Drop
    {
        private readonly Storefront.Model.SeoInfo _seoInfo;

        public SeoKeyword(Storefront.Model.SeoInfo seoInfo)
        {
            _seoInfo = seoInfo;
        }

        public string ImageAltDescription
        {
            get
            {
                return _seoInfo.ImageAltDescription;
            }
        }

        public string Keyword
        {
            get
            {
                return _seoInfo.Slug;
            }
        }

        public string Language
        {
            get
            {
                return _seoInfo.Language;
            }
        }

        public string MetaDescription
        {
            get
            {
                return _seoInfo.MetaDescription;
            }
        }

        public string MetaKeywords
        {
            get
            {
                return _seoInfo.MetaKeywords;
            }
        }

        public string Title
        {
            get
            {
                return _seoInfo.Title;
            }
        }
    }
}
