using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Web.Tests
{
    public static class FluentDataExtractors
    {
        public static TValue ExtractFromOkResult<TValue>(this ActionResult<TValue> actionResult)
        {
            return actionResult.ExtractActionResultValue<TValue, OkObjectResult>();
        }

        public static TValue ExtractActionResultValue<TValue, TResult>(this ActionResult<TValue> actionResult) where TResult : ObjectResult
        {
            return actionResult.Result.As<TResult>().Value.As<TValue>();
        }
    }
}
