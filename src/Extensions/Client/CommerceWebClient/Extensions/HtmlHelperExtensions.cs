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
using VirtoCommerce.Web.Client.Globalization;
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
            return Title(htmlHelper, title, "{0} | {1}");
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
            string storeName = StoreHelper.CustomerSession.StoreName;
            if (!String.IsNullOrEmpty(storeName))
            {
                return MvcHtmlString.Create(String.Format(formatString, title, storeName));
            }

            return MvcHtmlString.Create(title);
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

		/// <summary>
		/// Versions the specified HTML.
		/// </summary>
		/// <param name="html">The HTML.</param>
		/// <returns>MvcHtmlString.</returns>
		public static MvcHtmlString Version(this HtmlHelper html)
		{
			var assembly = Assembly.GetExecutingAssembly();
			return new MvcHtmlString(String.Format("{0} (Build {1})", assembly.GetInformationalVersion(), assembly.GetFileVersion()));
		}

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
            var sb = new StringBuilder();

            if (metadata.IsRequired && (metadata.IsNullableValueType || !metadata.ModelType.IsValueType))
            {
                sb.Append("<em>*</em>");
            }

            sb.Append(html.Encode(labelText));

            var tag = new TagBuilder("label");

            if (metadata.IsRequired)
            {
                tag.AddCssClass("required");
            }

            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.InnerHtml = sb.ToString();


            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}