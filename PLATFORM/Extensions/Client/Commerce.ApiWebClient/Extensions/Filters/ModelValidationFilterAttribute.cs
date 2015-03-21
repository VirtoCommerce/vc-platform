using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace VirtoCommerce.ApiWebClient.Extensions.Filters
{
	/// <summary>
	/// Class ModelValidationFilterAttribute.
	/// </summary>
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
		/// <summary>
		/// Occurs after the action method is invoked.
		/// </summary>
		/// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var modelState = actionExecutedContext.ActionContext.ModelState;
            if (!modelState.IsValid)
            {
                var errors =
                      from x in modelState.Keys
                      where modelState[x].Errors.Count > 0
                      select new
                      {
                          key = x.Substring(x.IndexOf(".", System.StringComparison.Ordinal) + 1),
                          errors = modelState[x].Errors.
                                                 Select(y => y.ErrorMessage).
                                                 ToArray()
                      };
                //var errors = modelState
                //    .Where(s => s.Value.Errors.Count > 0)
                //    .Select(s => new KeyValuePair<string, string>(s.Key.Substring(s.Key.IndexOf(".") + 1), s.Value.Errors.First().ErrorMessage.Localize()))
                //    .ToArray();

                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}