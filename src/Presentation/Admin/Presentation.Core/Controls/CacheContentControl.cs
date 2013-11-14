using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    public class CacheContentControl : ContentControl
    {
        #region Static Members

        static private Dictionary<object, object> cachedItems = new Dictionary<object, object>();
        static private Dictionary<Type, List<WeakReference>> unusedItems = new Dictionary<Type, List<WeakReference>>();

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(Type), typeof(CacheContentControl), new PropertyMetadata(TypePropertyChangedCallback));

        #endregion

        #region Private Fields

        private object viewModel;
        private Type type;

        #endregion

        #region Constructors

        public CacheContentControl()
        {
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(ViewModelManager_DataContextChanged);
        }

        #endregion

        #region Handlers

        static void TypePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CacheContentControl).UpdateContent();
        }

        void ViewModelManager_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && this.viewModel != e.NewValue)
            {
                this.viewModel = e.NewValue;
                UpdateContent();
            }
        }

		private static void GetBindings(FrameworkElement root, List<Tuple<FrameworkElement,DependencyProperty, Binding>> value)
    	{
    		foreach (FrameworkElement element in LogicalTreeHelper.GetChildren(root).OfType<FrameworkElement>())
    		{
    			
    			FieldInfo[] properties = element.GetType().GetFields(BindingFlags.Public | BindingFlags.GetProperty |
    			                                                     BindingFlags.Static | BindingFlags.FlattenHierarchy);
    			foreach (FieldInfo field in properties)
    			{
    				if (field.FieldType == typeof (DependencyProperty))
    				{
    					DependencyProperty dp = (DependencyProperty) field.GetValue(null);

    					BindingExpression bindingExpression = BindingOperations.GetBindingExpression(element, dp);
    					//bindingExpression.
    					if (BindingOperations.IsDataBound(element, dp))
    					{
    						var b = BindingOperations.GetBinding(element, dp);
    						if (b != null)
    						{
								value.Add(new Tuple<FrameworkElement, DependencyProperty, Binding>(element,
								dp,
								b
								));
    						}
    					}
    				}
    			}
				GetBindings(element,value);
    		}
    	}

        void closable_CloseViewRequestedEvent(object sender, EventArgs e)
        {
			// rp: try/catch were commented out as nobody knows does this code works proprerly or not.
			// at the same time I can observe that the assignement element.DataContext = null 
			// usully genereates exception (of course it should generate since there should be a lot 
			// of binded functionality in with data in unknown state).
            // therefore I'm going to disable bindig before DataContext changes and 
			// enable it after!
            //{
                if (cachedItems.ContainsKey(this.viewModel))
                {
                    var element = cachedItems[this.viewModel] as FrameworkElement;
                    if (element != null)
                    {
						// rp: disable binding
	                    var list = new List<Tuple<FrameworkElement,DependencyProperty, Binding>>();
						GetBindings(element,list);
						list.ForEach(i => BindingOperations.ClearBinding(i.Item1, i.Item2));
	                    
                        element.DataContext = null;
						
						// rp: enable binding
						list.ForEach(i => BindingOperations.SetBinding(i.Item1, i.Item2, i.Item3));

                        if (!unusedItems.Keys.Contains(this.type))
                            unusedItems.Add(this.type, new List<WeakReference>());

                        if (unusedItems[this.type].FirstOrDefault(x => x.Target == element) == null)
                            unusedItems[this.type].Add(new WeakReference(element));
                    }
                    this.Content = null;
                    cachedItems.Remove(this.viewModel);
                }

                var closable = this.viewModel as VirtoCommerce.ManagementClient.Core.Infrastructure.IClosable;
                if (closable != null)
                    closable.CloseViewRequestedEvent -= closable_CloseViewRequestedEvent;
            //}
            //catch { }
        }

        #endregion

        #region Help Methods

        public void UpdateContent()
        {
            if (this.Type != null)
            {
                this.type = this.Type;

                if (this.type != null && this.viewModel != null)
                {
                    object content = null;
                    if (cachedItems.ContainsKey(this.viewModel))
                    {
                        content = cachedItems[this.viewModel];
                    }
                    else
                    {
                        content = CreateObject(this.type, this.viewModel);
                        cachedItems.Add(this.viewModel, content);
                    }

                    if (content != null)
                    {
                        if (this.viewModel is VirtoCommerce.ManagementClient.Core.Infrastructure.IClosable)
                        {
                            var closable = this.viewModel as VirtoCommerce.ManagementClient.Core.Infrastructure.IClosable;
                            closable.CloseViewRequestedEvent -= closable_CloseViewRequestedEvent;
                            closable.CloseViewRequestedEvent += closable_CloseViewRequestedEvent;
                        }
                        this.Content = null;
                        this.Content = content;
                    }
                }
            }
        }

        private object CreateObject(System.Type type, object viewModel)
        {
            ClearWeakReferences();

            object res = null;

            if (unusedItems.ContainsKey(type))
            {
                foreach (var obj in unusedItems[type])
                {
                    if (obj.IsAlive)
                    {
                        res = obj.Target;
                        break;
                    }
                }
            }

            if (res != null)
                unusedItems[type] = unusedItems[type].Where(x => x.Target != res).ToList();
            else
                res = Activator.CreateInstance(type);

            if (res is FrameworkElement)
                (res as FrameworkElement).DataContext = viewModel;
            return res;
        }

        private void ClearWeakReferences()
        {
            List<Type> keys = new List<Type>(unusedItems.Keys);
            foreach (var key in keys)
                unusedItems[key] = unusedItems[key].Where(x => x.IsAlive).ToList();
        }

        #endregion

        #region Public Properties

        public Type Type
        {
            get
            {
                return this.GetValue(TypeProperty) as Type;
            }
            set
            {
                if (value != null)
                    this.SetValue(TypeProperty, value);
            }
        }

        #endregion
    }
}
