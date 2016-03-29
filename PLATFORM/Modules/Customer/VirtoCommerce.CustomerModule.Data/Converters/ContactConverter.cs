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
        public static void ToCoreModel(this dataModel.Contact dbEntity, coreModel.Contact contact)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");
            contact.Organizations = dbEntity.MemberRelations.Select(x => x.Ancestor).OfType<dataModel.Organization>().Select(x => x.Id).ToList();
        }

        public static void ToDataModel(this coreModel.Contact contact, dataModel.Contact dbContact, PrimaryKeyResolvingMap pkMap)
        {
            if (contact == null)
                throw new ArgumentNullException("contact");
            if (dbContact == null)
                throw new ArgumentNullException("dbContact");


            if (dbContact.Name == null)
            {
                dbContact.Name = dbContact.FullName;
            }

            pkMap.AddPair(contact, dbContact);

            if (contact.Organizations != null)
            {
                dbContact.MemberRelations = new ObservableCollection<dataModel.MemberRelation>();
                foreach (var organization in contact.Organizations)
                {
                    var memberRelation = new dataModel.MemberRelation
                    {
                        AncestorId = organization,
                        AncestorSequence = 1,
                        DescendantId = dbContact.Id,
                    };
                    dbContact.MemberRelations.Add(memberRelation);
                }
            }
        }
     
    }
}
