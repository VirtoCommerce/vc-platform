using Microsoft.Practices.Unity;

namespace VirtoCommerce.Foundation.Frameworks.Events
{
    public class EntityEventContext : IEntityEventContext
    {
        public delegate void EntityEventHandler(object sender, EntityEventArgs e);

        public event EntityEventHandler EntityBefore;
        public event EntityEventHandler EntityAfter;

        public void RaiseBeforeEvent(object sender, EntityEventArgs args)
        {
            Initialize();

            if (EntityBefore != null)
            {
                EntityBefore(sender, args);
            }
        }

        public void RaiseAfterEvent(object sender, EntityEventArgs args)
        {
            Initialize();

            if (EntityAfter != null)
            {
                EntityAfter(sender, args);
            }
        }

        private IUnityContainer container;

        //rp: container added to constructor to avoid using MVC service locator from background process (Scheduler)
        public EntityEventContext(IUnityContainer container)
        {
            this.container = container;
        }

        public void Subscribe(IEntityEventListener listener)
        {
            EntityBefore += listener.EntityBeforeSaved;
            EntityAfter += listener.EntityAfterSaved;
        }

        private object _lockObject = new object();
        private bool _isInitialized = false;
        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            lock (_lockObject)
            {
                if (_isInitialized)
                {
                    return;
                }


                // rp: this doesn't work for Scheduler (ServiceLocator depends on Http context)  
                // var listeners = ServiceLocator.Current.GetAllInstances<IEntityEventListener>();
                var listeners = container.ResolveAll<IEntityEventListener>();
                if (listeners != null)
                {
                    foreach (var listener in listeners)
                    {
                        Subscribe(listener);
                    }
                }

                

                _isInitialized = true;
            }
        }
    }
}
