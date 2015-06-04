#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Tags
{

    #region
    #endregion

    /// <summary>
    ///     Allows pagination of an array/collection.
    ///     == Basic usage:
    ///     {% paginate collection by 5 %}
    ///     {% for item in paginate.items %}
    ///     {{ forloop.index }}: {{ item.name }}
    ///     {% endfor %}
    ///     {% endpaginate %}
    ///     == Exposed variables within the paginate tags:
    ///     paginate.items -> returned paged items
    ///     paginate.pages -> total page count
    ///     paginate.previous -> previous true or false
    ///     paginate.next -> next page true or false
    ///     paginate.size -> total records
    ///     paginate.current_offset -> offset of paged records
    ///     paginate.current_page -> the current page number
    ///     paginate.page_size -> records per page
    ///     == Example full markup with paging tags and current_page tag:
    ///     {% current_page = 1 %}
    ///     {% paginate collection by 2 %}
    ///     {% for item in paginate.items %}
    ///     {{ item.name }}
    ///     {% endfor %}
    ///     {% if paginate.previous %}
    ///     {% capture prev_page %}/?page={{ paginate.current_page | minus:1 }}{% endcapture %}
    ///     <a href="{{ prev_page }}">Previous</a>
    ///     {% endif %}
    ///     Showing items {{ paginate.current_offset | plus: 1 }}-{% if paginate.next %}{{ paginate.current_offset | plus:
    ///     paginate.page_size }}{% else %}{{ paginate.size }}{% endif %} of {{ paginate.size }}.
    ///     {% if paginate.next %}
    ///     {% capture next_page %}/?page={{ paginate.current_page | plus:1 }}{% endcapture %}
    ///     <a href="{{ next_page }}">Next</a>
    ///     {% endif %}
    ///     {% endpaginate %}
    /// </summary>
    public class Paginate : Block
    {
        #region Static Fields
        private static readonly Regex Syntax = R.B(R.Q(@"({0})\s*by\s*({0}+)?"), DotLiquid.Liquid.QuotedFragment);
        #endregion

        #region Fields
        private Dictionary<string, string> _attributes;

        private string _collectionName;

        private int _currentPage;

        private int _pageSize;
        #endregion

        #region Public Properties
        public string PageSize { get; set; }
        #endregion

        #region Properties
        private UrlHelper Url
        {
            get
            {
                var httpContext = HttpContext.Current;
                if (httpContext == null)
                {
                    throw new InvalidOperationException("The link tag can only be used within a valid HttpContext");
                }

                var httpContextBase = new HttpContextWrapper(httpContext);
                var routeData = new RouteData();
                var requestContext = new RequestContext(httpContextBase, routeData);

                var urlHelper = new UrlHelper(requestContext);
                return urlHelper;
            }
        }
        #endregion

        #region Public Methods and Operators
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var match = Syntax.Match(markup);

            if (match.Success)
            {
                this._collectionName = match.Groups[1].Value;

                this.PageSize = match.Groups[2].Value;
                if (this.PageSize == string.Empty)
                {
                    this.PageSize = null;
                }

                this._attributes = new Dictionary<string, string>(Template.NamingConvention.StringComparer);

                R.Scan(markup, DotLiquid.Liquid.TagAttributes, (key, value) => this._attributes[key] = value);
            }
            else
            {
                throw new SyntaxException("PaginateSyntaxException");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            if (context["current_page"] != null)
            {
                if (!Int32.TryParse(context["current_page"].ToString(), out this._currentPage))
                {
                    this._currentPage = 1;
                }
            }

            if (this._currentPage == 0)
            {
                this._currentPage = 1;
            }

            if (this.PageSize == null)
            {
                this._pageSize = 20;
            }
            else
            {
                if (context[this.PageSize] == null)
                {
                    if (!Int32.TryParse(this.PageSize, out this._pageSize))
                    {
                        this._pageSize = 20;
                    }
                }
                else
                {
                    if (!Int32.TryParse(context[this.PageSize].ToString(), out this._pageSize))
                    {
                        this._pageSize = 20;
                    }
                }
            }

            context["paginate"] =
                Hash.FromAnonymousObject(
                    new
                    {
                        page_size = this._pageSize,
                        current_page = this._currentPage,
                        current_offset = (this._currentPage - 1) * this._pageSize
                    });

            var collection = context[this._collectionName];
            if (!(collection is IEnumerable))
            {
                return;
            }

            var length = collection != null ? ((ICollection)collection).Count : 0;

            var pageCount = Math.Ceiling((double)length / this._pageSize);

            var helper = this.Url;

            var previousLink = helper.SetParameter("page", this._currentPage - 1);
            var nextLink = helper.SetParameter("page", this._currentPage + 1);

            context["paginate"] =
                Hash.FromAnonymousObject(
                    new
                    {
                        pages = pageCount,
                        parts = this.CreateParts(pageCount),
                        previous =
                            (this._currentPage > 1)
                                ? new Part() { IsLink = true, Title = "&laquo; Previous", Url = previousLink }
                                : null,
                        next =
                            (this._currentPage < pageCount)
                                ? new Part() { IsLink = true, Title = "Next &raquo;", Url = nextLink }
                                : null,
                        size = length,
                        current_offset = (this._currentPage - 1) * this._pageSize,
                        current_page = this._currentPage,
                        page_size = this._pageSize
                    });

            this.RenderAll(this.NodeList, context, result);
        }
        #endregion

        #region Methods
        private Part[] CreateParts(double pageCount)
        {
            var parts = new List<Part>();
            var index = 0d;
            var helper = this.Url;

            while (index < pageCount)
            {
                parts.Add(
                    new Part()
                    {
                        IsLink = (this._currentPage - 1) == index ? false : true,
                        Title = (index + 1).ToString(),
                        Url = helper.SetParameter("page", (index + 1))
                    });
                index++;
            }

            return parts.ToArray();
        }
        #endregion
    }
}