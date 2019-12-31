using System;
using System.IO;

namespace VirtoCommerce.Platform.Modules.External
{
    public interface IExternalModulesClient
    {
        Stream OpenRead(Uri address);
    }
}
