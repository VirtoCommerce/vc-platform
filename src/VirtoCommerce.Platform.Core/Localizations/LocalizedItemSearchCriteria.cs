using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Localizations;

public class LocalizedItemSearchCriteria : SearchCriteriaBase
{
    public IList<string> Names { get; set; }
    public IList<string> Aliases { get; set; }
}
