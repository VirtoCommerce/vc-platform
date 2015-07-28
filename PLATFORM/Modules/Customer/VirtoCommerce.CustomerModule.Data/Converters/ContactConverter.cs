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
    public static class ContactConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public static coreModel.Contact ToCoreModel(this dataModel.Contact dbEntity)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");

            var retVal = new coreModel.Contact();
            retVal.InjectFrom(dbEntity);

            retVal.Addresses = dbEntity.Addresses.Select(x => x.ToCoreModel()).ToList();
            retVal.Emails = dbEntity.Emails.Select(x => x.Address).ToList();
            retVal.Notes = dbEntity.Notes.Select(x => x.ToCoreModel()).ToList();
            retVal.Phones = dbEntity.Phones.Select(x => x.Number).ToList();
            retVal.Organizations = dbEntity.MemberRelations.Select(x => x.Ancestor).OfType<dataModel.Organization>().Select(x => x.Id).ToList();
            return retVal;

        }


        public static dataModel.Contact ToDataModel(this coreModel.Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException("contact");

            var retVal = new dataModel.Contact();

            retVal.InjectFrom(contact);

            if (contact.Phones != null)
            {
                retVal.Phones = new ObservableCollection<dataModel.Phone>(contact.Phones.Select(x => new dataModel.Phone
                {
                    Number = x,
                    MemberId = contact.Id
                }));
            }

            if (contact.Emails != null)
            {
                retVal.Emails = new ObservableCollection<dataModel.Email>(contact.Emails.Select(x => new dataModel.Email
                {
                    Address = x,
                    MemberId = contact.Id
                }));
            }

            if (contact.Addresses != null)
            {
                retVal.Addresses = new ObservableCollection<dataModel.Address>(contact.Addresses.Select(x => x.ToDataModel()));
                foreach (var address in retVal.Addresses)
                {
                    address.MemberId = contact.Id;
                }
            }

            if (contact.Notes != null)
            {
                retVal.Notes = new ObservableCollection<dataModel.Note>(contact.Notes.Select(x => x.ToDataModel()));
                foreach (var note in retVal.Notes)
                {
                    note.MemberId = contact.Id;
                }
            }

            if (contact.Organizations != null)
            {
                retVal.MemberRelations = new ObservableCollection<dataModel.MemberRelation>();
                foreach (var organization in contact.Organizations)
                {
                    var memberRelation = new dataModel.MemberRelation
                    {
                        AncestorId = organization,
                        AncestorSequence = 1,
                        DescendantId = retVal.Id
                    };
                    retVal.MemberRelations.Add(memberRelation);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.Contact source, dataModel.Contact target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.Contact>(x => x.BirthDate, x => x.DefaultLanguage,
                                                                           x => x.FullName, x => x.Salutation,
                                                                           x => x.TimeZone);
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
                var relationComparer = AnonymousComparer.Create((dataModel.MemberRelation x) => x.Id);
                source.MemberRelations.Patch(target.MemberRelations, relationComparer, (sourceRel, targetRel) => { /*Nothing todo*/ });
            }
        }
    }
}
