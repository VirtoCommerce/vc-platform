using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
	//Used for mark that collection is null (ot initialized) used in pacth operation
	public class NullCollection<T> : ObservableCollection<T>
	{
	}
}
