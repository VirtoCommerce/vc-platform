using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VirtoCommerce.Platform.Modules.External
{
    public interface IExternalModulesClient
    {
        Stream OpenRead(Uri address);
    }
}
