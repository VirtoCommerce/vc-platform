using System;
using Hangfire;
using Microsoft.Practices.Unity;

namespace VirtoCommerce.Platform.Web.Hangfire
{
    public class UnityJobActivator : JobActivator
    {
        private readonly IUnityContainer _container;

        public UnityJobActivator(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
        }

        public override object ActivateJob(Type jobType)
        {
            return _container.Resolve(jobType);
        }
    }
}
