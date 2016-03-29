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
        public static void ToCoreModel(this dataModel.Member dbEntity, coreModel.Member member)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");
            if (member == null)
                throw new ArgumentNullException("member");
            //preserve member type 
            var memberType = member.MemberType;
            member.InjectFrom(dbEntity);
            member.MemberType = memberType;

            member.Addresses = dbEntity.Addresses.OrderBy(x=>x.Id).Select(x => x.ToCoreModel()).ToList();
            member.Emails = dbEntity.Emails.OrderBy(x => x.Id).Select(x => x.Address).ToList();
            member.Notes = dbEntity.Notes.OrderBy(x => x.Id).Select(x => x.ToCoreModel()).ToList();
            member.Phones = dbEntity.Phones.OrderBy(x => x.Id).Select(x => x.Number).ToList();
        }


        public static void ToDataModel(this coreModel.Member member, dataModel.Member dbMember)
        {
            if (member == null)
                throw new ArgumentNullException("member");
            if (dbMember == null)
                throw new ArgumentNullException("dbMember");

            dbMember.InjectFrom(member);

            if (member.Phones != null)
            {
                dbMember.Phones = new ObservableCollection<dataModel.Phone>(member.Phones.Select(x => new dataModel.Phone
                {
                    Number = x,
                    MemberId = member.Id
                }));
            }

            if (member.Emails != null)
            {
                dbMember.Emails = new ObservableCollection<dataModel.Email>(member.Emails.Select(x => new dataModel.Email
                {
                    Address = x,
                    MemberId = member.Id
                }));
            }

            if (member.Addresses != null)
            {
                dbMember.Addresses = new ObservableCollection<dataModel.Address>(member.Addresses.Select(x => x.ToDataModel()));
                foreach (var address in dbMember.Addresses)
                {
                    address.MemberId = member.Id;
                }
            }

            if (member.Notes != null)
            {
                dbMember.Notes = new ObservableCollection<dataModel.Note>(member.Notes.Select(x => x.ToDataModel()));
                foreach (var note in dbMember.Notes)
                {
                    note.MemberId = member.Id;
                }
            }
        }        
    }
}
