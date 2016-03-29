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
    public static class EmployeeConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public static void ToCoreModel(this dataModel.Employee dbEmployee, coreModel.Employee employee)
        {
            if (dbEmployee == null)
                throw new ArgumentNullException("dbEmployee");
            if (employee == null)
                throw new ArgumentNullException("employee");

            employee.Organizations = dbEmployee.MemberRelations.Select(x => x.Ancestor).OfType<dataModel.Organization>().Select(x => x.Id).ToList();
        }

        public static void ToDataModel(this coreModel.Employee employee, dataModel.Employee dbEmployee, PrimaryKeyResolvingMap pkMap)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            if (dbEmployee.Name == null)
            {
                dbEmployee.Name = dbEmployee.FullName;
            }

            pkMap.AddPair(employee, dbEmployee);

            if (employee.Organizations != null)
            {
                dbEmployee.MemberRelations = new ObservableCollection<dataModel.MemberRelation>();
                foreach (var organization in employee.Organizations)
                {
                    var memberRelation = new dataModel.MemberRelation
                    {
                        AncestorId = organization,
                        AncestorSequence = 1,
                        DescendantId = dbEmployee.Id,
                    };
                    dbEmployee.MemberRelations.Add(memberRelation);
                }
            }
        }

    }
}
