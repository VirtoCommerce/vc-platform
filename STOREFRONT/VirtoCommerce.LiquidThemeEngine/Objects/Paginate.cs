using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using PagedList;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/paginate
    /// The paginate tag's navigation is built using the attributes of the paginate object. You can also use the default_pagination filter for a quicker alternative.
    /// </summary>
    public class Paginate : Drop
    {
        private readonly IPagedList _pagedList;
    
        public Paginate(IPagedList pagedList)
        {
            _pagedList = pagedList;
            Parts = new List<Part>();
        }
        /// <summary>
        /// Returns the number of the current page.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return _pagedList.PageNumber;
            }
        }
        /// <summary>
        /// Returns the total number of items that are on the pages previous to the current one. For example, if you are paginating by 5 and are on the third page, paginate.current_offset would return 10.
        /// </summary>
        public int CurrentOffset
        {
            get
            {
                return _pagedList.FirstItemOnPage;
            }
        }
        /// <summary>
        /// Returns the total number of items to be paginated. For example, if you are paginating a collection of 120 products, paginate.items would return 120.
        /// </summary>
        public int Items
        {
            get
            {
                return _pagedList.TotalItemCount;
            }
        }
        /// <summary>
        /// Returns the number of items displayed per page.
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pagedList.PageSize;
            }
        }
        /// <summary>
        /// Returns the part variable for the Next link in the pagination navigation.
        /// </summary>
        public Part Next {
            get
            {
                Part retVal = null;
                if (!_pagedList.IsLastPage)
                {
                    retVal = Parts[CurrentPage];
                }
                return retVal;
            }
        }
        /// <summary>
        /// Returns the part variable for the Previous link in the pagination navigation.
        /// </summary>
        public Part Previous
        {
            get
            {
                Part retVal = null;
                if (!_pagedList.IsFirstPage)
                {
                    retVal = Parts[CurrentPage - 2];
                }
                return retVal;
            }
        }
        /// <summary>
        /// Returns the number of pages created by the pagination tag.
        /// </summary>
        public int Pages
        {
            get
            {
                return _pagedList.PageCount;
            }
        }
        /// <summary>
        /// Returns an array of all parts of the pagination. A part is a component used to build the navigation for the pagination.
        /// </summary>
        public List<Part> Parts { get; set; }
    }

}
