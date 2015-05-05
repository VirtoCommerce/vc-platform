
namespace VirtoCommerce.Platform.Core.Modularity
{
    public abstract class AbstractModule: IModule
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
