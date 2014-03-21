using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions
{
	/// <summary>
	/// Class HtmlHelperExtensions.
	/// </summary>
    public static class HtmlHelperExtensions
    {
		/// <summary>
		/// Selects the option.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="html">The HTML.</param>
		/// <param name="currentValue">The current value.</param>
		/// <param name="optionValue">The option value.</param>
		/// <param name="text">The text.</param>
		/// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString SelectOption<T>(this HtmlHelper html, T currentValue, T optionValue, string text)
        {
            return SelectOption(html, optionValue, Equals(optionValue, currentValue), text);
        }

		/// <summary>
		/// Selects the option.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="html">The HTML.</param>
		/// <param name="currentValue">The current value.</param>
		/// <param name="optionValue">The option value.</param>
		/// <param name="text">The text.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString SelectOption<T>(this HtmlHelper html, T currentValue, T optionValue, string text, object htmlAttributes)
        {
            return SelectOption(html, optionValue, Equals(optionValue, currentValue), text, htmlAttributes);
        }

		/// <summary>
		/// Selects the option.
		/// </summary>
		/// <param name="html">The HTML.</param>
		/// <param name="optionValue">The option value.</param>
		/// <param name="selected">if set to <c>true</c> [selected].</param>
		/// <param name="text">The text.</param>
		/// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString SelectOption(this HtmlHelper html, object optionValue, bool selected, string text)
        {
            return SelectOption(html, optionValue, selected, text, null);
        }

		/// <summary>
		/// Selects the option.
		/// </summary>
		/// <param name="html">The HTML.</param>
		/// <param name="optionValue">The option value.</param>
		/// <param name="selected">if set to <c>true</c> [selected].</param>
		/// <param name="text">The text.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString SelectOption(this HtmlHelper html, object optionValue, bool selected, string text, object htmlAttributes)
        {
            var builder = new TagBuilder("option");

            if (optionValue != null)
                builder.MergeAttribute("value", optionValue.ToString());

            if (selected)
                builder.MergeAttribute("selected", "selected");

            builder.SetInnerText(text);

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

		/// <summary>
		/// Serializes o json
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="value">The value.</param>
		/// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString JsonSerialize(this HtmlHelper htmlHelper, object value)
        {
            return MvcHtmlString.Create(new JavaScriptSerializer().Serialize(value));
        }

		/// <summary>
		/// Titles the specified HTML helper.
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="title">The title.</param>
		/// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString Title(this HtmlHelper htmlHelper, string title)
		{
		    return MvcHtmlString.Create(htmlHelper.ViewBag.Title as string != null ? ((string)htmlHelper.ViewBag.Title).Title() : title.Title());
		}

	    /// <summary>
		/// Titles the specified HTML helper.
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="title">The title.</param>
		/// <param name="formatString">The format string.</param>
		/// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString Title(this HtmlHelper htmlHelper, string title, string formatString)
        {
            return MvcHtmlString.Create(htmlHelper.ViewBag.Title as string != null ? ((string)htmlHelper.ViewBag.Title).Title(formatString) : title.Title(formatString));
        }

		/// <summary>
		/// LabelFor extension
		/// </summary>
		/// <typeparam name="TModel">The type of the t model.</typeparam>
		/// <typeparam name="TValue">The type of the t value.</typeparam>
		/// <param name="html">The HTML.</param>
		/// <param name="expression">The expression.</param>
		/// <param name="labelText">The label text.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <returns>MvcHtmlString.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LabelForEx<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText = null, object htmlAttributes = null)
        {
            return LabelHelper(html, ModelMetadata.FromLambdaExpression(expression, html.ViewData), ExpressionHelper.GetExpressionText(expression), labelText, new RouteValueDictionary(htmlAttributes));
        }

        private static MvcHtmlString _version;
		/// <summary>
		/// Versions the specified HTML.
		/// </summary>
		/// <param name="html">The HTML.</param>
		/// <returns>MvcHtmlString.</returns>
		public static MvcHtmlString Version(this HtmlHelper html)
		{
		    if (_version == null)
		    {
		        var assembly = Assembly.GetExecutingAssembly();
		        _version = new MvcHtmlString(String.Format("{0} (Build {1})", assembly.GetInformationalVersion(), assembly.GetFileVersion()));
		    }

		    return _version;
		}

        #region PageData

        [ThreadStatic]
        private static ControllerBase _pageDataController;
        [ThreadStatic]
        private static PageData _pageData;

        /// <summary>
        /// ViewBag shared in parent controller. Everything set from partial views is visible here
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static dynamic SharedViewBag(this HtmlHelper html)
        {
            ControllerBase controller = html.ViewContext.Controller;
            return SharedViewBag(html.ViewContext.Controller);
        }


        public static dynamic SharedViewBag(this ControllerBase controller)
        {
            while (controller.ControllerContext.IsChildAction)
            {
                controller = controller.ControllerContext.ParentActionViewContext.Controller;
            }
            if (_pageDataController == controller)
            {
                return _pageData;
            }
            _pageDataController = controller;
            _pageData = new PageData(() => controller.ViewData);
            return _pageData;
        }

        #endregion

        /// <summary>
		/// Labels the helper.
		/// </summary>
		/// <param name="html">The HTML.</param>
		/// <param name="metadata">The metadata.</param>
		/// <param name="htmlFieldName">Name of the HTML field.</param>
		/// <param name="labelText">The label text.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <returns>MvcHtmlString.</returns>
        internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText, IDictionary<string, object> htmlAttributes)
        {
	        labelText = string.IsNullOrEmpty(labelText) ? metadata.GetDisplayName().Localize() : labelText.Localize();

	        if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            var sb = new StringBuilder(html.Encode(labelText));

            if (metadata.IsRequired && (metadata.IsNullableValueType || !metadata.ModelType.IsValueType))
            {
                sb.Append("<sup class='required'>*</sup>");
            }

            var tag = new TagBuilder("label");


            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.InnerHtml = sb.ToString();


            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}