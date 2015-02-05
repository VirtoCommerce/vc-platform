using System.Collections.Generic;
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
                //EntityBefore.Invoke(sender, args);
                EntityBefore(sender, args);
            }
        }

        public void RaiseAfterEvent(object sender, EntityEventArgs args)
        {
            Initialize();

            if (EntityAfter != null)
            {
                //EntityAfter.Invoke(sender, args);
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

                IEnumerable<IEntityEventListener> listeners = null;
                try
                {
                    listeners = container.ResolveAll<IEntityEventListener>();
                }
                catch
                {
                    //asu: This happens if container is prepared using PerRequestLifeTimeManager and HttpContext not availabe
                }

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
