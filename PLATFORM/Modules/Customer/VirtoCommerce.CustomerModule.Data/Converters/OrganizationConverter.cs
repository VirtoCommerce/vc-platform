using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using dataModel = VirtoCommerce.CustomerModule.Data.Model;
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
        public static coreModel.Organization ToCoreModel(this dataModel.Organization dbEntity)
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


        public static dataModel.Organization ToDataModel(this coreModel.Organization organization)
        {
            if (organization == null)
                throw new ArgumentNullException("organization");

            var retVal = new dataModel.Organization();

            retVal.InjectFrom(organization);

            if (organization.Phones != null)
            {
                retVal.Phones = new ObservableCollection<dataModel.Phone>(organization.Phones.Select(x => new dataModel.Phone
                {
                    Number = x,
                    MemberId = organization.Id
                }));
            }

            if (organization.Emails != null)
            {
                retVal.Emails = new ObservableCollection<dataModel.Email>(organization.Emails.Select(x => new dataModel.Email
                {
                    Address = x,
                    MemberId = organization.Id
                }));
            }

            if (organization.Addresses != null)
            {
                retVal.Addresses = new ObservableCollection<dataModel.Address>(organization.Addresses.Select(x => x.ToDataModel()));
                foreach (var address in retVal.Addresses)
                {
                    address.MemberId = organization.Id;
                }
            }

            if (organization.Notes != null)
            {
                retVal.Notes = new ObservableCollection<dataModel.Note>(organization.Notes.Select(x => x.ToDataModel()));
                foreach (var note in retVal.Notes)
                {
                    note.MemberId = organization.Id;
                }
            }

            if (organization.ParentId != null)
            {
                retVal.MemberRelations = new ObservableCollection<dataModel.MemberRelation>();
                var memberRelation = new dataModel.MemberRelation
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
        public static void Patch(this dataModel.Organization source, dataModel.Organization target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            var patchInjection = new PatchInjection<dataModel.Organization>(x => x.Name, x => x.Description,
                                                                           x => x.OwnerId, x => x.OrgType,
                                                                           x => x.BusinessCategory);
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
                var noteComparer = AnonymousComparer.Create((dataModel.Note x) => x.Id);
                source.Notes.Patch(target.Notes, noteComparer, (sourceNote, targetNote) => sourceNote.Patch(targetNote));
            }
        }


    }
}
