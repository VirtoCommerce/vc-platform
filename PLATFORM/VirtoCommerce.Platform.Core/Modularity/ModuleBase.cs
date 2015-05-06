namespace VirtoCommerce.Platform.Core.Modularity
{
    public abstract class ModuleBase : IModule
    {
        public virtual void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual void PostInitialize()
        {
        }
    }
}
