using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.Content.Data.Converters
{
    public static class MenuLinkConverter
    {
        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this MenuLink source, MenuLink target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjectionPolicy = new PatchInjection<MenuLink>(x => x.IsActive, x=>x.Priority, x=>x.Title, x=>x.Url);

            target.AssociatedObjectId = source.AssociatedObjectId;
            target.AssociatedObjectName = source.AssociatedObjectName;
            target.AssociatedObjectType = source.AssociatedObjectType;

            target.InjectFrom(patchInjectionPolicy, source);
        }
    }
}
