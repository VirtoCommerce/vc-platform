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
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public abstract class MemberDataEntity : AuditableEntity
	{
		public MemberDataEntity()
		{
			Notes = new NullCollection<NoteDataEntity>();
			Addresses = new NullCollection<AddressDataEntity>();
			MemberRelations = new NullCollection<MemberRelationDataEntity>();
			Phones = new NullCollection<PhoneDataEntity>();
			Emails = new NullCollection<EmailDataEntity>();
        }

        [StringLength(64)]
        public string MemberType { get; set; }
        
        [StringLength(128)]
        public string Name { get; set; }

        #region NavigationProperties

        public ObservableCollection<NoteDataEntity> Notes { get; set; }

		public ObservableCollection<AddressDataEntity> Addresses { get; set; }

		public ObservableCollection<MemberRelationDataEntity> MemberRelations { get; set; }

		public ObservableCollection<PhoneDataEntity> Phones { get; set; }

		public ObservableCollection<EmailDataEntity> Emails { get; set; }

        #endregion

        public virtual Member ToMember(Member member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            //preserve member type 
            var memberType = member.MemberType;
            member.InjectFrom(this);
            member.MemberType = memberType;

            member.Addresses = this.Addresses.OrderBy(x => x.Id).Select(x => x.ToAddress(new Address())).ToList();
            member.Emails = this.Emails.OrderBy(x => x.Id).Select(x => x.Address).ToList();
            member.Notes = this.Notes.OrderBy(x => x.Id).Select(x => x.ToNote(new Note())).ToList();
            member.Phones = this.Phones.OrderBy(x => x.Id).Select(x => x.Number).ToList();

            return member;
        }


        public virtual MemberDataEntity FromMember(Member member, PrimaryKeyResolvingMap pkMap)
        {
            if (member == null)
                throw new ArgumentNullException("member");
          
            this.InjectFrom(member);

            if (member.Phones != null)
            {
                this.Phones = new ObservableCollection<PhoneDataEntity>(member.Phones.Select(x => new PhoneDataEntity
                {
                    Number = x,
                    MemberId = member.Id
                }));
            }

            if (member.Emails != null)
            {
                this.Emails = new ObservableCollection<EmailDataEntity>(member.Emails.Select(x => new EmailDataEntity
                {
                    Address = x,
                    MemberId = member.Id
                }));
            }

            if (member.Addresses != null)
            {
                this.Addresses = new ObservableCollection<AddressDataEntity>(member.Addresses.Select(x => new AddressDataEntity().FromAddress(x)));
                foreach (var address in this.Addresses)
                {
                    address.MemberId = member.Id;
                }
            }

            if (member.Notes != null)
            {
                this.Notes = new ObservableCollection<NoteDataEntity>(member.Notes.Select(x => new NoteDataEntity().FromNote(x)));
                foreach (var note in this.Notes)
                {
                    note.MemberId = member.Id;
                }
            }
            return this;
        }

      
        public virtual void Patch(MemberDataEntity target)
        {
            var patchInjection = new PatchInjection<MemberDataEntity>(x => x.Name);
            target.InjectFrom(patchInjection, this);

            if (!this.Phones.IsNullCollection())
            {
                var phoneComparer = AnonymousComparer.Create((PhoneDataEntity x) => x.Number);
                this.Phones.Patch(target.Phones, phoneComparer, (sourcePhone, targetPhone) => targetPhone.Number = sourcePhone.Number);
            }

            if (!this.Emails.IsNullCollection())
            {
                var addressComparer = AnonymousComparer.Create((EmailDataEntity x) => x.Address);
                this.Emails.Patch(target.Emails, addressComparer, (sourceEmail, targetEmail) => targetEmail.Address = sourceEmail.Address);
            }

            if (!this.Addresses.IsNullCollection())
            {
                this.Addresses.Patch(target.Addresses, new AddressDataEntityComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
            }

            if (!this.Notes.IsNullCollection())
            {
                var noteComparer = AnonymousComparer.Create((NoteDataEntity x) => x.Id ?? x.Body);
                this.Notes.Patch(target.Notes, noteComparer, (sourceNote, targetNote) => sourceNote.Patch(targetNote));
            }

            if (!this.MemberRelations.IsNullCollection())
            {
                var relationComparer = AnonymousComparer.Create((MemberRelationDataEntity x) => x.AncestorId);
                this.MemberRelations.Patch(target.MemberRelations, relationComparer, (sourceRel, targetRel) => { /*Nothing todo*/ });
            }
        }
    }
}
