using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class Vendor : Member
    {
        public string Description { get; set; }
        [StringLength(2048)]
        public string SiteUrl { get; set; }
        [StringLength(2048)]
        public string LogoUrl { get; set; }
        [StringLength(64)]
        public string GroupName { get; set; }

        public override void Patch(Member target)
        {
            var patchInjection = new PatchInjection<Vendor>(x => x.SiteUrl, x => x.LogoUrl, x => x.GroupName, x => x.Description);
            target.InjectFrom(patchInjection, this);

            base.Patch(target);
        }
    }
}
