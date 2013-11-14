using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace  VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
    public interface IVirtualListLoader<T>
    {
        bool CanSort { get; }
        IList<T> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount);
    }
}
