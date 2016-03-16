using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using dataModel = VirtoCommerce.CustomerModule.Data.Model;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
    public static class MemberConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public static coreModel.Member ToCoreModel(this dataModel.Member dbEntity, coreModel.Member member)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");
            var memberType = member.MemberType;
            member.InjectFrom(dbEntity);
            member.MemberType = memberType;

            member.Addresses = dbEntity.Addresses.OrderBy(x=>x.Id).Select(x => x.ToCoreModel()).ToList();
            member.Emails = dbEntity.Emails.OrderBy(x => x.Id).Select(x => x.Address).ToList();
            member.Notes = dbEntity.Notes.OrderBy(x => x.Id).Select(x => x.ToCoreModel()).ToList();
            member.Phones = dbEntity.Phones.OrderBy(x => x.Id).Select(x => x.Number).ToList();
            return member;
        }


        public static dataModel.Member ToDataModel(this coreModel.Member member, dataModel.Member dbEntity)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            dbEntity.InjectFrom(member);

            if (member.Phones != null)
            {
                dbEntity.Phones = new ObservableCollection<dataModel.Phone>(member.Phones.Select(x => new dataModel.Phone
                {
                    Number = x,
                    MemberId = member.Id
                }));
            }

            if (member.Emails != null)
            {
                dbEntity.Emails = new ObservableCollection<dataModel.Email>(member.Emails.Select(x => new dataModel.Email
                {
                    Address = x,
                    MemberId = member.Id
                }));
            }

            if (member.Addresses != null)
            {
                dbEntity.Addresses = new ObservableCollection<dataModel.Address>(member.Addresses.Select(x => x.ToDataModel()));
                foreach (var address in dbEntity.Addresses)
                {
                    address.MemberId = member.Id;
                }
            }

            if (member.Notes != null)
            {
                dbEntity.Notes = new ObservableCollection<dataModel.Note>(member.Notes.Select(x => x.ToDataModel()));
                foreach (var note in dbEntity.Notes)
                {
                    note.MemberId = member.Id;
                }
            }
            return dbEntity;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.Member source, dataModel.Member target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.Member>(x => x.Name);
            target.InjectFrom(patchInjection, source);

            if (!source.Phones.IsNullCollection())
            {
                var phoneComparer = AnonymousComparer.Create((dataModel.Phone x) => x.Number);
                source.Phones.Patch(target.Phones, phoneComparer, (sourcePhone, targetPhone) => targetPhone.Number = sourcePhone.Number);
            }

            if (!source.Emails.IsNullCollection())
            {
                var addressComparer = AnonymousComparer.Create((dataModel.Email x) => x.Address);
                source.Emails.Patch(target.Emails, addressComparer, (sourceEmail, targetEmail) => targetEmail.Address = sourceEmail.Address);
            }

            if (!source.Addresses.IsNullCollection())
            {
                source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
            }

            if (!source.Notes.IsNullCollection())
            {
                var noteComparer = AnonymousComparer.Create((dataModel.Note x) => x.Id ?? x.Body);
                source.Notes.Patch(target.Notes, noteComparer, (sourceNote, targetNote) => sourceNote.Patch(targetNote));
            }

            if (!source.MemberRelations.IsNullCollection())
            {
                var relationComparer = AnonymousComparer.Create((dataModel.MemberRelation x) => x.AncestorId);
                source.MemberRelations.Patch(target.MemberRelations, relationComparer, (sourceRel, targetRel) => { /*Nothing todo*/ });
            }
        }
    }
}
