using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Settings.Converters
{
	public static class ModuleConverter
	{

		public static ModuleDescriptor ToModel(this ModuleManifest module)
		{
			return new ModuleDescriptor
			{
				Id = module.Id,
				Title = module.Title,
			};
		}



	}
}
