using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common.Events
{
    public interface IAsyncObserver<in T>
    {
        //
        // Summary:
        //     Provides the observer with new data.
        //
        // Parameters:
        //   value:
        //     The current notification information.
        Task OnNextAsync(T value);
    }
}
