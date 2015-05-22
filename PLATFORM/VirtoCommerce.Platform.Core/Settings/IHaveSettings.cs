using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Settings
{
	public interface IHaveSettings
	{
		ICollection<SettingEntry> Settings { get; set; }
	}
}
