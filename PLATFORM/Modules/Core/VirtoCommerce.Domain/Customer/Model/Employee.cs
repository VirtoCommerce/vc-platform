using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Domain.Customer.Model
{
    public class Employee : Member, IHasSecurityAccounts
    {
        public Employee()
        {
            SecurityAccounts = new List<ApplicationUserExtended>();
        }
        public string PhotoUrl { get; set; }
        /// <summary>
        /// Employee type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Employee activity flag
        /// </summary>
        public bool IsActive { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }
        public string TimeZone { get; set; }
        public string DefaultLanguage { get; set; }
        public DateTime? BirthDate { get; set; }

        public ICollection<string> Organizations { get; set; }
        #region IHasSecurityAccounts Members
        /// <summary>
        /// All security accounts associated with this employee
        /// </summary>
        public ICollection<ApplicationUserExtended> SecurityAccounts { get; private set; } 
        #endregion
    }
}
