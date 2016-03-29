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
        public static void ToCoreModel(this dataModel.Organization dbOrg, coreModel.Organization org)
        {
            if (dbOrg == null)
                throw new ArgumentNullException("dbOrg");
            if (org == null)
                throw new ArgumentNullException("org");

            if (dbOrg.MemberRelations.Any())
            {
                org.ParentId = dbOrg.MemberRelations.FirstOrDefault().AncestorId;
            }
        }


        public static void ToDataModel(this coreModel.Organization org, dataModel.Organization dbOrg, PrimaryKeyResolvingMap pkMap)
        {
            if (org == null)
                throw new ArgumentNullException("org");

            pkMap.AddPair(org, dbOrg);
         
            if (org.ParentId != null)
            {
                dbOrg.MemberRelations = new ObservableCollection<dataModel.MemberRelation>();
                var memberRelation = new dataModel.MemberRelation
                {
                    AncestorId = org.ParentId,
                    DescendantId = org.Id,
                    AncestorSequence = 1
                };
                dbOrg.MemberRelations.Add(memberRelation);
            }
        }

    }
}
