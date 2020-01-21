using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class ActionResultExtensions
    {
        /// <summary>
        /// If the ActionResult is ObjectResult, then try to convert its payload from IdentityResult to SecurityResult, otherwise do nothing.
        /// </summary>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static ActionResult<SecurityResult> ToSecurityResult(this ActionResult actionResult)
        {
            if ((actionResult as ObjectResult)?.Value is IdentityResult identityResult)
            {
                (actionResult as ObjectResult).Value = new SecurityResult()
                {
                    Succeeded = identityResult.Succeeded,
                    Errors = identityResult.Errors.Select(x => x.Description)
                };
            }
            return actionResult;
        }

        /// <summary>
        /// Convert <see cref="ActionResult&lt;IdentityResult&gt;"/> to <see cref="ActionResult&lt;SecurityResult&gt;"/>.
        /// </summary>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static ActionResult<SecurityResult> ToSecurityResult(this ActionResult<IdentityResult> actionResult)
        {
            return ToSecurityResult(actionResult.Result);
        }

    }
}
