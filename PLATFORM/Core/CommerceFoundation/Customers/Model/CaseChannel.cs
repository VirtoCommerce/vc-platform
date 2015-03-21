using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public enum CaseChannel
	{
        /// <summary>
        /// The email
        /// </summary>
		[Description("By E-Mail")]
		Email,

        /// <summary>
        /// The phone
        /// </summary>
        [Description("By phone")]
        Phone,

        /// <summary>
        /// The commerce manager
        /// </summary>
        [Description("By commerce manager")]
        CommerceManager
	}
}
