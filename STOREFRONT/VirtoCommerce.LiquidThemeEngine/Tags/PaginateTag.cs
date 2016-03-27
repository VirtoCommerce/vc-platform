using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using PagedList;
using VirtoCommerce.LiquidThemeEngine.Extensions;
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
        private static readonly Regex _syntax = R.B(R.Q(@"({0})\s*by\s*({0}+)?"), DotLiquid.Liquid.QuotedFragment);

        private string _collectionName;
        private int _pageSize;
        #region Public Methods and Operators

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var match = _syntax.Match(markup);

            if (match.Success)
            {
                _collectionName = match.Groups[1].Value;
                var pageSize =  match.Groups[2].Value;
                if(!string.IsNullOrEmpty(pageSize))
                {
                    int.TryParse(pageSize, out _pageSize);
                }
            }
            else
            {
                throw new SyntaxException("PaginateSyntaxException");
            }

            _pageSize = _pageSize > 0 ? _pageSize : 20;

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var mutablePagedList = context[_collectionName] as IMutablePagedList;
            var collection = context[_collectionName] as ICollection;
            var pagedList = context[_collectionName] as IPagedList;
            Uri requestUrl;
            Uri.TryCreate(context["request_url"] as string, UriKind.RelativeOrAbsolute, out requestUrl);
            var pageNumber = (int)context["current_page"];
 
            if (mutablePagedList != null)
            {
                mutablePagedList.Slice(pageNumber, _pageSize > 0 ? _pageSize : 20);
                pagedList = mutablePagedList;
            }
            else if (collection != null)
            {
                pagedList = new PagedList<Drop>(collection.OfType<Drop>().AsQueryable(), pageNumber, _pageSize);                
                //TODO: Need find way to replace ICollection instance in liquid context to paged instance
                //var hash = context.Environments.FirstOrDefault(s => s.ContainsKey(_collectionName));
                //hash[_collectionName] = pagedList;
            }

            if (pagedList != null)
            {
                var paginate = new Paginate(pagedList);
                context["paginate"] = paginate;

                for (int i = 1; i <= pagedList.PageCount; i++)
                {
                    paginate.Parts.Add(new Part { IsLink = i != pagedList.PageNumber, Title = i.ToString(), Url = requestUrl != null ? requestUrl.SetQueryParameter("page", i.ToString()).ToString() : i.ToString() });
                }
                RenderAll(NodeList, context, result);
            }
        }

        #endregion
    }
}
