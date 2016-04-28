using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
    /// <summary>
    /// Represent a summary content statistics 
    /// </summary>
    public class ContentStatistic
    {
        public string ActiveThemeName { get; set; }
        public int ThemesCount { get; set; }
        public int PagesCount { get; set; }
        public int BlogsCount { get; set; }
    }
}