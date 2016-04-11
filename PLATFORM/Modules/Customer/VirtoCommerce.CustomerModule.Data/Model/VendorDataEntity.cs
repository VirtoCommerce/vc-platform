using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class VendorDataEntity : MemberDataEntity
    {
        public string Description { get; set; }
        [StringLength(2048)]
        public string SiteUrl { get; set; }
        [StringLength(2048)]
        public string LogoUrl { get; set; }
        [StringLength(64)]
        public string GroupName { get; set; }

        public override Member ToMember(Member member)
        {
            //Call base converter first
            base.ToMember(member);
            return member;
        }

        public override MemberDataEntity FromMember(Member member, PrimaryKeyResolvingMap pkMap)
        {
            pkMap.AddPair(member, this);
            //Call base converter
            return base.FromMember(member, pkMap);
        }

        public override void Patch(MemberDataEntity target)
        {
            var patchInjection = new PatchInjection<VendorDataEntity>(x => x.SiteUrl, x => x.LogoUrl, x => x.GroupName, x => x.Description);
            target.InjectFrom(patchInjection, this);

            base.Patch(target);
        }
    }
}
