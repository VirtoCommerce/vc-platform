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
using VirtoCommerce.CustomerModule.Data.Converters;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public abstract class Member : AuditableEntity
	{
		public Member()
		{
			Notes = new NullCollection<Note>();
			Addresses = new NullCollection<Address>();
			MemberRelations = new NullCollection<MemberRelation>();
			Phones = new NullCollection<Phone>();
			Emails = new NullCollection<Email>();
        }

        [StringLength(64)]
        public string MemberType { get; set; }
        
        [StringLength(128)]
        public string Name { get; set; }

        #region NavigationProperties

        public ObservableCollection<Note> Notes { get; set; }

		public ObservableCollection<Address> Addresses { get; set; }

		public ObservableCollection<MemberRelation> MemberRelations { get; set; }

		public ObservableCollection<Phone> Phones { get; set; }

		public ObservableCollection<Email> Emails { get; set; }

        #endregion

        public virtual void Patch(Member target)
        {
            var patchInjection = new PatchInjection<Member>(x => x.Name);
            target.InjectFrom(patchInjection, this);

            if (!this.Phones.IsNullCollection())
            {
                var phoneComparer = AnonymousComparer.Create((Phone x) => x.Number);
                this.Phones.Patch(target.Phones, phoneComparer, (sourcePhone, targetPhone) => targetPhone.Number = sourcePhone.Number);
            }

            if (!this.Emails.IsNullCollection())
            {
                var addressComparer = AnonymousComparer.Create((Email x) => x.Address);
                this.Emails.Patch(target.Emails, addressComparer, (sourceEmail, targetEmail) => targetEmail.Address = sourceEmail.Address);
            }

            if (!this.Addresses.IsNullCollection())
            {
                this.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
            }

            if (!this.Notes.IsNullCollection())
            {
                var noteComparer = AnonymousComparer.Create((Note x) => x.Id ?? x.Body);
                this.Notes.Patch(target.Notes, noteComparer, (sourceNote, targetNote) => sourceNote.Patch(targetNote));
            }

            if (!this.MemberRelations.IsNullCollection())
            {
                var relationComparer = AnonymousComparer.Create((MemberRelation x) => x.AncestorId);
                this.MemberRelations.Patch(target.MemberRelations, relationComparer, (sourceRel, targetRel) => { /*Nothing todo*/ });
            }
        }
    }
}
