using System.Collections.ObjectModel;

namespace VirtoCommerce.Platform.Core.Common
{
    //Used for mark that collection is null (ot initialized) used in pacth operation
    public class NullCollection<T> : ObservableCollection<T>
    {
    }
}
