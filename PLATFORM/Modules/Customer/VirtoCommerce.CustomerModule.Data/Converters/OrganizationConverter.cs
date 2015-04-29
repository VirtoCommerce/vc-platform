using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using foundationModel = VirtoCommerce.CustomerModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
    public static class OrganizationConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="catalogBase"></param>
        /// <returns></returns>
        public static coreModel.Organization ToCoreModel(this foundationModel.Organization dbEntity)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");

            var retVal = new coreModel.Organization();
            retVal.InjectFrom(dbEntity);

            retVal.Addresses = dbEntity.Addresses.Select(x => x.ToCoreModel()).ToList();
            retVal.Emails = dbEntity.Emails.Select(x => x.Address).ToList();
            retVal.Notes = dbEntity.Notes.Select(x => x.ToCoreModel()).ToList();
            retVal.Phones = dbEntity.Phones.Select(x => x.Number).ToList();
            if (dbEntity.MemberRelations.Any())
            {
                retVal.ParentId = dbEntity.MemberRelations.FirstOrDefault().AncestorId;
            }

            return retVal;

        }


        public static foundationModel.Organization ToFoundation(this coreModel.Organization organization)
        {
            if (organization == null)
                throw new ArgumentNullException("organization");

            var retVal = new foundationModel.Organization();

            retVal.InjectFrom(organization);

            retVal.Phones = new NullCollection<foundationModel.Phone>();
            if (organization.Phones != null)
            {
                retVal.Phones = new ObservableCollection<foundationModel.Phone>(organization.Phones.Select(x => new foundationModel.Phone
                {
                    Number = x,
                    MemberId = organization.Id
                }));
            }

            retVal.Emails = new NullCollection<foundationModel.Email>();
            if (organization.Emails != null)
            {
                retVal.Emails = new ObservableCollection<foundationModel.Email>(organization.Emails.Select(x => new foundationModel.Email
                {
                    Address = x,
                    MemberId = organization.Id
                }));
            }

            retVal.Addresses = new NullCollection<foundationModel.Address>();
            if (organization.Addresses != null)
            {
                retVal.Addresses = new ObservableCollection<foundationModel.Address>(organization.Addresses.Select(x => x.ToFoundation()));
                foreach (var address in retVal.Addresses)
                {
                    address.MemberId = organization.Id;
                }
            }

            retVal.Notes = new NullCollection<foundationModel.Note>();
            if (organization.Notes != null)
            {
                retVal.Notes = new ObservableCollection<foundationModel.Note>(organization.Notes.Select(x => x.ToFoundation()));
                foreach (var note in retVal.Notes)
                {
                    note.MemberId = organization.Id;
                }
            }

            retVal.MemberRelations = new NullCollection<foundationModel.MemberRelation>();
            if (organization.ParentId != null)
            {
                retVal.MemberRelations = new ObservableCollection<foundationModel.MemberRelation>();
                var memberRelation = new foundationModel.MemberRelation
                {
                    AncestorId = organization.ParentId,
                    DescendantId = organization.Id,
                    AncestorSequence = 1

                };
                retVal.MemberRelations.Add(memberRelation);
            }
            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this foundationModel.Organization source, foundationModel.Organization target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            var patchInjection = new PatchInjection<foundationModel.Organization>(x => x.Name, x => x.Description,
                                                                           x => x.OwnerId, x => x.OrgType,
                                                                           x => x.BusinessCategory);
            target.InjectFrom(patchInjection, source);

            if (!source.Phones.IsNullCollection())
            {
                var phoneComparer = AnonymousComparer.Create((foundationModel.Phone x) => x.Number);
                source.Phones.Patch(target.Phones, phoneComparer, (sourcePhone, targetPhone) => targetPhone.Number = sourcePhone.Number);
            }
            if (!source.Emails.IsNullCollection())
            {
                var addressComparer = AnonymousComparer.Create((foundationModel.Email x) => x.Address);
                source.Emails.Patch(target.Emails, addressComparer, (sourceEmail, targetEmail) => targetEmail.Address = sourceEmail.Address);
            }
            if (!source.Addresses.IsNullCollection())
            {
                source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
            }
            if (!source.Notes.IsNullCollection())
            {
                var noteComparer = AnonymousComparer.Create((foundationModel.Note x) => x.Id);
                source.Notes.Patch(target.Notes, noteComparer, (sourceNote, targetNote) => sourceNote.Patch(targetNote));
            }
        }


    }
}
