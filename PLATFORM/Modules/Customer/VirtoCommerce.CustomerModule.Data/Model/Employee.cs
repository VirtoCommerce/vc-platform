using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class Employee : Member
    {
        [StringLength(64)]
        public string Type { get; set; }
   
        public bool IsActive { get; set; }

        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string MiddleName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(254)]
        [Required]
        public string FullName { get; set; }

        [StringLength(32)]
        public string TimeZone { get; set; }

        [StringLength(32)]
        public string DefaultLanguage { get; set; }

        public DateTime? BirthDate { get; set; }


        public override void Patch(Member target)
        {
            var patchInjection = new PatchInjection<Employee>(x => x.FirstName, x => x.MiddleName, x => x.LastName, x => x.BirthDate, x => x.DefaultLanguage,
                                                                   x => x.FullName, x => x.IsActive, x => x.Type, x => x.TimeZone);
            target.InjectFrom(patchInjection, this);

            base.Patch(target);
        }
    }
}
