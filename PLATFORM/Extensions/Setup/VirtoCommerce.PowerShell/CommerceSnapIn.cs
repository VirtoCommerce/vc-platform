using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.PowerShell
{
	[CLSCompliant(false)]
	[RunInstaller(true)]
	public class CommerceSnapIn : PSSnapIn
	{
		public CommerceSnapIn()
			: base()
		{
		}

		public override string Name
		{
			get { return "VirtoCommerceCmdlets"; }
		}

		public override string Vendor
		{
			get { return "(c) by Alexandre Siniouguine - mailto:sasha@virtoway.com"; }
		}

		public override string Description
		{
			get { return "Virto Commerce Cmdlets"; }
		}
	}
}
