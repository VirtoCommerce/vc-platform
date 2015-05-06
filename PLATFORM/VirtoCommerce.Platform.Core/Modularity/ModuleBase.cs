
namespace VirtoCommerce.Platform.Core.Modularity
{
    public abstract class ModuleBase: IModule
    {
        public virtual void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        public abstract void Initialize();

        public virtual void PostInitialize()
        {
        }
    }
}
