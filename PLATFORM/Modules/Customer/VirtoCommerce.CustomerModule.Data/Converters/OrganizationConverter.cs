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
            dbEntity.ToCoreModel(retVal);

            if (dbEntity.MemberRelations.Any())
            {
                retVal.ParentId = dbEntity.MemberRelations.FirstOrDefault().AncestorId;
            }
            return retVal;
        }


        public static dataModel.Organization ToDataModel(this coreModel.Organization organization, PrimaryKeyResolvingMap pkMap)
        {
            if (organization == null)
                throw new ArgumentNullException("organization");

            var retVal = new dataModel.Organization();

            organization.ToDataModel(retVal);

            pkMap.AddPair(organization, retVal);
         
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
            //Path base type properties
            ((dataModel.Member)source).Patch(target);
        }


    }
}
