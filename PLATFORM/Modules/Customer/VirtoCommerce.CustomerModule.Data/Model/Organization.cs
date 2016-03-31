using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class Organization : Member
	{
		public int OrgType { get; set; }

 		[StringLength(256)]
		public string Description { get; set; }

 		[StringLength(64)]
		public string BusinessCategory { get; set; }

  		[StringLength(128)]
		public string OwnerId { get; set; }
	}
}
