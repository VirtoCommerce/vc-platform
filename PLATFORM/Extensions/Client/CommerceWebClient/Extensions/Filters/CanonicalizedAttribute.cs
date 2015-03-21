using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Filters
{
	[CLSCompliant(false)]
	public class CanonicalizedAttribute : ActionFilterAttribute
	{
		private readonly bool _usePermanentRedirect;
		private readonly Type[] _skipControllers;

		public CanonicalizedAttribute(params Type[] skipControllers) : this(true, skipControllers) { }
		public CanonicalizedAttribute(bool usePermanentRedirect, params Type[] skipControllers)
		{
			_usePermanentRedirect = usePermanentRedirect;
			_skipControllers = skipControllers;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (_skipControllers.Any(x => filterContext.ActionDescriptor.ControllerDescriptor.ControllerType == x))
			{
				return;
			}
			var context = filterContext.HttpContext;

			// don't 'rewrite' POST requests, child action and ajax requests
			if (context.Request.RequestType != "GET" || filterContext.IsChildAction || context.Request.IsAjaxRequest())
			{
				return;
			}

			if (context.Request.Url != null)
			{
				var baseUri = string.Format(
					"{0}://{1}{2}",
					context.Request.Url.Scheme,
					context.Request.Url.Host,
					context.Request.Url.Port == 80 ? "" : ":" + context.Request.Url.Port);

				var path = HttpUtility.UrlDecode(string.Concat(baseUri, context.Request.Url.AbsolutePath));

				if (!string.IsNullOrEmpty(path))
				{
					var query = HttpUtility.UrlDecode(context.Request.Url.Query);
					var queryString = context.Request.QueryString;
					var needRedirect = false;

					//Make sure we allways use same virtual path as Route provides
					var routePath = filterContext.RouteData.Route.GetVirtualPath(filterContext.RequestContext,
						filterContext.RouteData.Values);

					if (routePath != null && !string.IsNullOrEmpty(routePath.VirtualPath))
					{
						var absoluteRoutePath = HttpUtility.UrlDecode(string.Concat(baseUri, context.Request.ApplicationPath, context.Request.ApplicationPath != "/" ? "/" : "", routePath.VirtualPath));

						if (!string.IsNullOrEmpty(absoluteRoutePath) && !absoluteRoutePath.Equals(path, StringComparison.InvariantCultureIgnoreCase))
						{
							path = absoluteRoutePath;
							needRedirect = true;
						}
					}

					//Process query string
					if (!string.IsNullOrEmpty(query))
					{
						//Rebuild querystring from scratch
						var newQuery = string.Empty;

						//First goes search filter ordered based on document
						var helper = StoreHelper.SearchFilter;
						var urlHelper = new UrlHelper(context.Request.RequestContext);

						var parameters = helper.Filters.Where(f => !(f is PriceRangeFilter) || ((PriceRangeFilter)f).Currency.Equals(StoreHelper.CustomerSession.Currency, StringComparison.OrdinalIgnoreCase))
							.Select(filter => queryString.AllKeys
							.FirstOrDefault(k => k.Equals(urlHelper.GetFacetKey(filter.Key), StringComparison.InvariantCultureIgnoreCase)))
							.Where(key => !string.IsNullOrEmpty(key))
							.ToDictionary<string, string, object>(key => key, key => queryString[key]);

						if (parameters.Any())
						{
							newQuery = urlHelper.SetQueryParameters(newQuery, parameters);
						}

						//Order remaining parameters
						var otherParams = queryString.AllKeys.Where(key => !parameters.ContainsKey(key)).OrderBy(k => k)
							.ToDictionary<string, string, object>(key => key, key => queryString[key]);

						if (otherParams.Any())
						{
							newQuery = urlHelper.SetQueryParameters(newQuery, otherParams);
						}

						if (!string.IsNullOrEmpty(newQuery) && !newQuery.StartsWith("?"))
						{
							newQuery = string.Concat("?", newQuery);
						}

						newQuery = HttpUtility.UrlDecode(newQuery);

						if (!string.Equals(query, newQuery, StringComparison.InvariantCultureIgnoreCase))
						{
							query = newQuery;
							needRedirect = true;
						}
					}


					//make language code allways be five symbols
					if (filterContext.RouteData.Values.ContainsKey(Routing.Constants.Language) &&
						filterContext.RouteData.Values[Routing.Constants.Language] as string != null)
					{
						var lang = filterContext.RouteData.Values[Routing.Constants.Language].ToString();
						if (lang.Length < 5)
						{
							try
							{
								var cult = CultureInfo.CreateSpecificCulture(lang);
								if (!path.ToLowerInvariant().Contains(cult.Name.ToLowerInvariant()))
								{
									path = path.Replace(lang, cult.Name);
									needRedirect = true;
								}
							}
							catch
							{
								//Something wrong with language??
							}
						}
					}

					//make path segments allways encoded
					var encodedPath = path;
					encodedPath = ProcessSegment(filterContext.RouteData.Values, encodedPath, Routing.Constants.Store);
					encodedPath = ProcessSegment(filterContext.RouteData.Values, encodedPath, Routing.Constants.Category);
					encodedPath = ProcessSegment(filterContext.RouteData.Values, encodedPath, Routing.Constants.Item);

					// check for any upper-case letters:
					if (path != encodedPath.ToLowerInvariant())
					{
						path = encodedPath.ToLowerInvariant();
						needRedirect = true;
					}

					//If request is for root dont chech this rule
					if (!context.Request.Url.AbsolutePath.Equals(context.Request.ApplicationPath))
					{
						// make sure request ends with a "/"
						if (path.EndsWith("/"))
						{
							needRedirect = true;
						}
					}


					if (needRedirect)
					{
						Redirect(context, path, query);
						return;
					}
				}
			}

			base.OnActionExecuting(filterContext);

		}

		private string ProcessSegment(RouteValueDictionary routeValues, string path, string key)
		{
			SeoUrlKeywordTypes type;

			if (routeValues.ContainsKey(key) &&
						  routeValues[key] as string != null && Enum.TryParse(key, true, out type))
			{
				var value = routeValues[key].ToString();
				var lang = routeValues.ContainsKey(Routing.Constants.Language) ? routeValues[Routing.Constants.Language] as string : null;
				var valueEncoded = SettingsHelper.SeoEncode(value, type, lang).ToLowerInvariant();

				//If encoded and used value does not match (keyword exist) and requested path contains not encoded value replace with encoded
				if (!value.Equals(valueEncoded, StringComparison.InvariantCultureIgnoreCase) && path.ToLowerInvariant().Contains(value.ToLowerInvariant()))
				{
					path = path.Replace(value, valueEncoded);
				}
			}

			return path;
		}



		// correct as many 'rules' as possible per redirect to avoid
		// issuing too many redirects per request.
		private void Redirect(HttpContextBase context, string path, string query)
		{
			var newLocation = path;

			if (newLocation.EndsWith("/"))
				newLocation = newLocation.Substring(0, newLocation.Length - 1);

			newLocation = newLocation.ToLower(CultureInfo.InvariantCulture);

			if (_usePermanentRedirect)
			{
				context.Response.RedirectPermanent(newLocation + query, true);
			}
			else
			{
				context.Response.Redirect(newLocation + query, true);
			}
		}

	}
}