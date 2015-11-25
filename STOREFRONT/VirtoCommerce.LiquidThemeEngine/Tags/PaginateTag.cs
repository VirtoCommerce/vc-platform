using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using PagedList;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Tags
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/tags/theme-tags#paginate
    /// Splitting products, blog articles, and search results across multiple pages is a necessary component of theme design as you are limited to 50 results per page in any for loop.
    /// The paginate tag works in conjunction with the for tag to split content into numerous pages.It must wrap a for tag block that loops through an array, as shown in the example below:
    /// </summary>
    public class PaginateTag : Block
    {
        private static readonly Regex Syntax = R.B(R.Q(@"({0})\s*by\s*({0}+)?"), DotLiquid.Liquid.QuotedFragment);

        private string _collectionName;
       
        #region Public Methods and Operators
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var match = Syntax.Match(markup);

            if (match.Success)
            {
                _collectionName = match.Groups[1].Value;
            }
            else
            {
                throw new SyntaxException("PaginateSyntaxException");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var pagedList = context[this._collectionName] as IStorefrontPagedList;
            if (pagedList == null)
            {
                return;
            }
            var paginate = new Paginate(pagedList);
            context["paginate"] = paginate;
       
            for (int i = 1; i <= pagedList.PageCount; i++)
            {
                paginate.Parts.Add(new Part { IsLink = i != pagedList.PageNumber, Title = i.ToString(), Url = pagedList.GetPageUrl(i) });
            }
            this.RenderAll(this.NodeList, context, result);
        }
        #endregion

        
      
    }
}
