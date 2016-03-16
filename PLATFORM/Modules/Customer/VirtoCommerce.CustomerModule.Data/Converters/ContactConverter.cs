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
            dbEntity.ToCoreModel(retVal);
            retVal.Organizations = dbEntity.MemberRelations.Select(x => x.Ancestor).OfType<dataModel.Organization>().Select(x => x.Id).ToList();
            return retVal;
        }

        public static dataModel.Contact ToDataModel(this coreModel.Contact contact, PrimaryKeyResolvingMap pkMap)
        {
            if (contact == null)
                throw new ArgumentNullException("contact");

            var retVal = new dataModel.Contact();
           
            contact.ToDataModel(retVal);

            if (retVal.Name == null)
            {
                retVal.Name = retVal.FullName;
            }

            pkMap.AddPair(contact, retVal);

            if (contact.Organizations != null)
            {
                retVal.MemberRelations = new ObservableCollection<dataModel.MemberRelation>();
                foreach (var organization in contact.Organizations)
                {
                    var memberRelation = new dataModel.MemberRelation
                    {
                        AncestorId = organization,
                        AncestorSequence = 1,
                        DescendantId = retVal.Id,
                    };
                    retVal.MemberRelations.Add(memberRelation);
                }
            }
            return (dataModel.Contact)retVal;
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

            var patchInjection = new PatchInjection<dataModel.Contact>(x => x.FirstName, x => x.MiddleName, x => x.LastName, x=> x.BirthDate, x => x.DefaultLanguage,
                                                                           x => x.FullName, x => x.Salutation,
                                                                           x => x.TimeZone);
            target.InjectFrom(patchInjection, source);
            //Path base type properties
            ((dataModel.Member)source).Patch(target);
        }
    }
}
