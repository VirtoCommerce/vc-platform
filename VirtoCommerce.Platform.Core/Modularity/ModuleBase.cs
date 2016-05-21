namespace VirtoCommerce.Platform.Core.Modularity
{
    public abstract class ModuleBase : IModule
    {
        public virtual void SetupDatabase()
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual void PostInitialize()
        {
        }

        public void Uninstall()
        {
        }
    }
}
