using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoSoftware.CommerceFoundation.Asset.Model
{
	[Serializable]
	public class ProviderInfo 
	{
		public string ProviderKey
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public long ContentLength
		{
			get;
			set;
		}

	}
}
