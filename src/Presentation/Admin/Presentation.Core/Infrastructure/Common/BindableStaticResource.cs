using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Data;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public class BindableStaticResource2 : MarkupExtension
    {
        /// <summary>
        /// Flag to know if the data context change handler has been setted
        /// </summary>
        private bool dataContextChangeHandlerSet = false;
        /// <summary>
        /// Reference to the target object
        /// </summary>
        private WeakReference targetObjectRef;
        /// <summary>
        /// Target property
        /// </summary>
        private object targetProperty;
        /// <summary>
        /// Hack to trick the WPF platform in thining that the binding is not sealed yet and then change the binding
        /// </summary>
        private static FieldInfo isSealedFieldInfo;
        /// <summary>
        /// Dummy property used to simulate the binding
        /// </summary>
        private static readonly DependencyProperty dummyProperty = DependencyProperty.Register("Dummy", typeof(object), typeof(DependencyObject));

        /// <summary>
        /// Static constructor to get the FieldInfo meta data for the _isSealed field of the BindingBase class
        /// See http://marlongrech.wordpress.com/2008/08/03/
        /// </summary>
        static BindableStaticResource2()
        {
            // Initialize the field info once
            isSealedFieldInfo = typeof(BindingBase).GetField("_isSealed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (isSealedFieldInfo == null)
                throw new InvalidOperationException("Oops, we have a problem, it seems like the WPF team decided to change the name of the _isSealed field of the BindingBase class.");

        }

        /// <summary>
        /// Create a bindable static resource
        /// </summary>
        public BindableStaticResource2()
            : this(new Binding())
        {
        }

        /// <summary>
        /// Create a bindable static resource
        /// </summary>
        /// <param name="binding">Binding used to retrive the resource key</param>
        public BindableStaticResource2(Binding binding)
            : base()
        {
            Binding = binding;
        }

        /// <summary>
        /// Provide a value for the target property
        /// </summary>
        /// <param name="serviceProvider">Service provider for the markup extension</param>
        /// <returns>Value for the target property</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Get the service
            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                // If the target is defined
                if (provideValueTarget.TargetObject != null && provideValueTarget.TargetProperty != null)
                {
                    // Save the target
                    this.targetObjectRef = new WeakReference(provideValueTarget.TargetObject);
                    this.targetProperty = provideValueTarget.TargetProperty;
                }

                // Get the target element
                FrameworkElement targetElement = provideValueTarget.TargetObject as FrameworkElement;
                if (targetElement != null)
                {
                    // If the binding has none source
                    if (String.IsNullOrEmpty(Binding.ElementName) && Binding.RelativeSource == null && Binding.Source == null)
                    {
                        // Use the datacontext of the target
                        Binding.Source = targetElement.DataContext;

                        // We must know if the data context change, so if the handler has not been setted
                        if (!dataContextChangeHandlerSet)
                        {
                            // Add an handler
                            targetElement.DataContextChanged += new DependencyPropertyChangedEventHandler(targetElement_DataContextChanged);
                            dataContextChangeHandlerSet = true;
                        }
                    }
                }
            }

            // Return the resource
            return GetResource(GetResourceKey());
        }

        /// <summary>
        /// Occurs when the datacontext of the target object has changed
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void targetElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Update the binding
            UpdateBinding(e.NewValue);
            // Refresh the target
            RefreshTarget();
        }

        /// <summary>
        /// Update the binding
        /// </summary>
        /// <param name="source">New binding source</param>
        private void UpdateBinding(object source)
        {
            if (this.Binding != null)
            {
                // This is a hack to trick the WPF platform in thining that the binding is not sealed yet and then change the binding
                bool isSealed = (bool)isSealedFieldInfo.GetValue(this.Binding);

                if (isSealed)
                    // Change the is sealed value
                    isSealedFieldInfo.SetValue(this.Binding, false);

                // If the binding use the datacontext as source
                if (String.IsNullOrEmpty(this.Binding.ElementName) && this.Binding.RelativeSource == null)
                    // Use the source
                    this.Binding.Source = source;

                if (isSealed)
                    // Put the is sealed value back as it was...
                    isSealedFieldInfo.SetValue(this.Binding, true);
            }
        }

        /// <summary>
        /// Get the resource key provided by the binding
        /// </summary>
        /// <returns>Resource key</returns>
        private object GetResourceKey()
        {
            // Create a dummy object to simulate the binding
            DependencyObject dummyObject = new DependencyObject();
            // Set the dummy binding
            BindingOperations.SetBinding(dummyObject, dummyProperty, Binding);
            // Get the binded value and return the resource key
            return dummyObject.GetValue(dummyProperty);
        }

        /// <summary>
        /// Get a resource
        /// </summary>
        /// <param name="resourceKey">Resource key</param>
        /// <returns>Resource</returns>
        private object GetResource(object resourceKey)
        {
            // If the target is not available, skip
            if (!targetObjectRef.IsAlive)
                return null;

            object resource = null;
            // Get the resource
            FrameworkElement targetElement = targetObjectRef.Target as FrameworkElement;
            if (targetElement != null && resourceKey != null)
                resource = targetElement.FindResource(resourceKey);

            // Return the resource
            return resource;
        }

        /// <summary>
        /// Refresh the target
        /// </summary>
        private void RefreshTarget()
        {
            // If the target is not available, skip
            if (!targetObjectRef.IsAlive)
                return;

            // Get the resource
            object resource = GetResource(GetResourceKey());

            // Apply the resource to the target
            if (targetProperty is DependencyProperty)
            {
                DependencyObject obj = targetObjectRef.Target as DependencyObject;
                DependencyProperty property = targetProperty as DependencyProperty;
                obj.SetValue(property, resource);

            }
            else
            {
                object obj = targetObjectRef.Target;
                PropertyInfo property = targetProperty as PropertyInfo;
                property.SetValue(obj, resource, null);
            }
        }

        /// <summary>
        /// Get or set the binding used to retrieve the resource key
        /// </summary>
        public Binding Binding { get; set; }
    }


    public class BindableStaticResource : StaticResourceExtension
    {
        /// <summary>
        /// Flag to know if the data context change handler has been setted
        /// </summary>
        private bool dataContextChangeHandlerSet = false;
        /// <summary>
        /// Reference to the target object
        /// </summary>
        private WeakReference targetObjectRef;
        /// <summary>
        /// Target property
        /// </summary>
        private object targetProperty;

        private static readonly DependencyProperty dummyProperty = DependencyProperty.Register("Dummy", typeof(object), typeof(DependencyObject));

        /// <summary>
        /// Hack to trick the WPF platform in thinking that the binding is not sealed yet and then change the binding
        /// </summary>
        private static FieldInfo isSealedFieldInfo;

        static BindableStaticResource()
        {
            /*
            dummyProperty = DependencyProperty.RegisterAttached("Dummy",
                                                                typeof(Object),
                                                                typeof(DependencyObject),
                                                                new UIPropertyMetadata(null));
             * */

            // Initialize the field info once
            isSealedFieldInfo = typeof(BindingBase).GetField("_isSealed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (isSealedFieldInfo == null)
                throw new InvalidOperationException("Oops, we have a problem, it seems like the WPF team decided to change the name of the _isSealed field of the BindingBase class.");

        }

        public Binding Binding { get; set; }

        public BindableStaticResource()
        {
        }

        public BindableStaticResource(Binding binding)
        {
            Binding = binding;
        }

        /*
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetObject = (FrameworkElement)target.TargetObject;

            MyBinding.Source = targetObject.DataContext;
            var DummyDO = new DependencyObject();
            BindingOperations.SetBinding(DummyDO, DummyProperty, MyBinding);

            ResourceKey = DummyDO.GetValue(DummyProperty);

            return ResourceKey != null ? base.ProvideValue(serviceProvider) : null;
        }

        public new object ResourceKey
        {
            get
            {
                return base.ResourceKey;
            }
            set
            {
                if (value != null)
                    base.ResourceKey = value;
            }
        }
         * */

        /// <summary>
        /// Provide a value for the target property
        /// </summary>
        /// <param name="serviceProvider">Service provider for the markup extension</param>
        /// <returns>Value for the target property</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Get the service
            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                // If the target is defined
                if (provideValueTarget.TargetObject != null && provideValueTarget.TargetProperty != null)
                {
                    // Save the target
                    this.targetObjectRef = new WeakReference(provideValueTarget.TargetObject);
                    this.targetProperty = provideValueTarget.TargetProperty;
                }

                // Get the target element
                FrameworkElement targetElement = provideValueTarget.TargetObject as FrameworkElement;
                if (targetElement != null)
                {
                    // If the binding has none source
                    if (String.IsNullOrEmpty(Binding.ElementName) && Binding.RelativeSource == null && Binding.Source == null)
                    {
                        // Use the datacontext of the target
                        Binding.Source = targetElement.DataContext;

                        // We must know if the data context change, so if the handler has not been setted
                        if (!dataContextChangeHandlerSet)
                        {
                            // Add an handler
                            targetElement.DataContextChanged += new DependencyPropertyChangedEventHandler(targetElement_DataContextChanged);
                            dataContextChangeHandlerSet = true;
                        }
                    }
                }
            }

            // Return the resource
            return GetResource(GetResourceKey());
        }

        /// <summary>
        /// Get the resource key provided by the binding
        /// </summary>
        /// <returns>Resource key</returns>
        private object GetResourceKey()
        {
            // Create a dummy object to simulate the binding
            DependencyObject dummyObject = new DependencyObject();
            // Set the dummy binding
            BindingOperations.SetBinding(dummyObject, dummyProperty, Binding);
            // Get the binded value and return the resource key
            return dummyObject.GetValue(dummyProperty);
        }

        /// <summary>
        /// Get a resource
        /// </summary>
        /// <param name="resourceKey">Resource key</param>
        /// <returns>Resource</returns>
        private object GetResource(object resourceKey)
        {
            // If the target is not available, skip
            if (!targetObjectRef.IsAlive)
                return null;

            object resource = null;
            // Get the resource
            FrameworkElement targetElement = targetObjectRef.Target as FrameworkElement;
            if (targetElement != null && resourceKey != null)
                resource = targetElement.FindResource(resourceKey);

            // Return the resource
            return resource;
        }

        /// <summary>
        /// Refresh the target
        /// </summary>
        private void RefreshTarget()
        {
            // If the target is not available, skip
            if (!targetObjectRef.IsAlive)
                return;

            // Get the resource
            object resource = GetResource(GetResourceKey());

            // Apply the resource to the target
            if (targetProperty is DependencyProperty)
            {
                DependencyObject obj = targetObjectRef.Target as DependencyObject;
                DependencyProperty property = targetProperty as DependencyProperty;
                obj.SetValue(property, resource);

            }
            else
            {
                object obj = targetObjectRef.Target;
                PropertyInfo property = targetProperty as PropertyInfo;
                property.SetValue(obj, resource, null);
            }
        }

        /// <summary>
        /// Occurs when the datacontext of the target object has changed
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void targetElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Update the binding
            UpdateBinding(e.NewValue);
            // Refresh the target
            RefreshTarget();
        }

        /// <summary>
        /// Update the binding
        /// </summary>
        /// <param name="source">New binding source</param>
        private void UpdateBinding(object source)
        {
            if (this.Binding != null)
            {
                // This is a hack to trick the WPF platform in thining that the binding is not sealed yet and then change the binding
                bool isSealed = (bool)isSealedFieldInfo.GetValue(this.Binding);

                if (isSealed)
                    // Change the is sealed value
                    isSealedFieldInfo.SetValue(this.Binding, false);

                // If the binding use the datacontext as source
                if (String.IsNullOrEmpty(this.Binding.ElementName) && this.Binding.RelativeSource == null)
                    // Use the source
                    this.Binding.Source = source;

                if (isSealed)
                    // Put the is sealed value back as it was...
                    isSealedFieldInfo.SetValue(this.Binding, true);
            }
        }
    }

}
