using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.ActionConstraints;

public sealed class HasFormValueAttribute : ActionMethodSelectorAttribute
{
    private readonly string _name;
    private readonly string[] _allowedMethods = ["POST"];

    public HasFormValueAttribute(string name)
    {
        _name = name;
    }

    public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
    {
        var request = routeContext.HttpContext.Request;

        return _allowedMethods.ContainsIgnoreCase(request.Method) &&
               !string.IsNullOrEmpty(request.ContentType) &&
               request.ContentType.StartsWithIgnoreCase("application/x-www-form-urlencoded") &&
               !string.IsNullOrEmpty(request.Form[_name]);
    }
}
