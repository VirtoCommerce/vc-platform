using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class ContactDataEntity : MemberDataEntity
    {
        public ContactDataEntity()
        {
            BirthDate = DateTime.Now;
        }

        #region UserProfile members
        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string MiddleName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(254)]
        [Required]
        public string FullName { get; set; }

        [StringLength(32)]
        public string TimeZone { get; set; }

        [StringLength(32)]
        public string DefaultLanguage { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(64)]
        public string TaxpayerId { get; set; }

        [StringLength(64)]
        public string PreferredDelivery { get; set; }

        [StringLength(64)]
        public string PreferredCommunication { get; set; }

        public byte[] Photo { get; set; }

        [StringLength(256)]
        public string Salutation { get; set; }

        #endregion

        public override Member ToMember(Member member)
        {
            //Call base converter first
            base.ToMember(member);
            var contact = member as Contact;
            contact.Organizations = this.MemberRelations.Select(x => x.Ancestor).OfType<OrganizationDataEntity>().Select(x => x.Id).ToList();
            member.Name = contact.FullName;
            return member;
        }

        public override MemberDataEntity FromMember(Member member, PrimaryKeyResolvingMap pkMap)
        {
            var contact = member as Contact;
            if (contact != null)
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Name = contact.FullName;
                }
                pkMap.AddPair(contact, this);
                if (contact.Organizations != null)
                {
                    this.MemberRelations = new ObservableCollection<MemberRelationDataEntity>();
                    foreach (var organization in contact.Organizations)
                    {
                        var memberRelation = new MemberRelationDataEntity
                        {
                            AncestorId = organization,
                            AncestorSequence = 1,
                            DescendantId = this.Id,
                        };
                        this.MemberRelations.Add(memberRelation);
                    }
                }
            }
            //Call base converter
            return base.FromMember(member, pkMap);
        }

        public override void Patch(MemberDataEntity target)
        {
            var patchInjection = new PatchInjection<ContactDataEntity>(x => x.FirstName, x => x.MiddleName, x => x.LastName, x => x.BirthDate, x => x.DefaultLanguage,
                                                                        x => x.FullName, x => x.Salutation,
                                                                        x => x.TimeZone);
            target.InjectFrom(patchInjection, this);

            base.Patch(target);
        }
    }
}
