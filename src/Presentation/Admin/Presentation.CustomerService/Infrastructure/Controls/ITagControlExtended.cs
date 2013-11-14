using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls
{
    public interface ITagControlExtended<T>
    {
        ObservableCollection<T> SearchedItems { get; }

        ObservableCollection<TagControlItemViewModel> CollectionFromTagControl { get; } 
            
        DelegateCommand<string> SearchItemsForTagControlCommand { get; }

    }
}
