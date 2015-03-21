using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks
{
	//Used for mark that collection is null (ot initialized) used in pacth operation
	public class NullCollection<T> : ObservableCollection<T>
	{
	}
}
