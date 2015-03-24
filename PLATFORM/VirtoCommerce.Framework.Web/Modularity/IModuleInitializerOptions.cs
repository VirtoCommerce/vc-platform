using System.Collections.Generic;

namespace VirtoCommerce.Framework.Web.Modularity
{
    public interface IModuleInitializerOptions
    {
        SampleDataLevel SampleDataLevel { get; }
        string GetModuleDirectoryPath(string moduleId);
    }
}
