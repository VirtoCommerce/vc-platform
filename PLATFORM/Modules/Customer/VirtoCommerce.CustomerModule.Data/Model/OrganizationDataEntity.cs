using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.Common;
using System.Collections.ObjectModel;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class OrganizationDataEntity : MemberDataEntity
	{
		public int OrgType { get; set; }

 		[StringLength(256)]
		public string Description { get; set; }

 		[StringLength(64)]
		public string BusinessCategory { get; set; }

  		[StringLength(128)]
		public string OwnerId { get; set; }

        public override Member ToMember(Member member)
        {
            //Call base converter first
            base.ToMember(member);

            var organization = member as Organization;
            if (this.MemberRelations.Any())
            {
                organization.ParentId = this.MemberRelations.FirstOrDefault().AncestorId;
            }
            return member;
        }

        public override MemberDataEntity FromMember(Member member, PrimaryKeyResolvingMap pkMap)
        {
            var organization = member as Organization;
            pkMap.AddPair(organization, this);

            if (organization.ParentId != null)
            {
                this.MemberRelations = new ObservableCollection<MemberRelationDataEntity>();
                var memberRelation = new MemberRelationDataEntity
                {
                    AncestorId = organization.ParentId,
                    DescendantId = organization.Id,
                    AncestorSequence = 1
                };
                this.MemberRelations.Add(memberRelation);
            }

            //Call base converter
            return base.FromMember(member, pkMap);
        }

        public override void Patch(MemberDataEntity target)
        {
            var patchInjection = new PatchInjection<OrganizationDataEntity>(x => x.Name, x => x.Description,
                                                                       x => x.OwnerId, x => x.OrgType,
                                                                       x => x.BusinessCategory);
            target.InjectFrom(patchInjection, this);

            base.Patch(target);
        }
    }
}
