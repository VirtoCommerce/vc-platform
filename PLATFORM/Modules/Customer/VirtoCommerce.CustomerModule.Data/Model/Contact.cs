using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using Omu.ValueInjecter;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class Contact : Member
    {
        public Contact()
        {
            BirthDate = DateTime.Now;
        }

        #region UserProfile members
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

        [StringLength(64)]
        public string TaxpayerId { get; set; }

        [StringLength(64)]
        public string PreferredDelivery { get; set; }

        [StringLength(64)]
        public string PreferredCommunication { get; set; }

        public byte[] Photo { get; set; }

        [StringLength(256)]
        public string Salutation { get; set; }

        #endregion

        public override void Patch(Member target)
        {
            var patchInjection = new PatchInjection<Contact>(x => x.FirstName, x => x.MiddleName, x => x.LastName, x => x.BirthDate, x => x.DefaultLanguage,
                                                                        x => x.FullName, x => x.Salutation,
                                                                        x => x.TimeZone);
            target.InjectFrom(patchInjection, this);

            base.Patch(target);
        }
    }
}
