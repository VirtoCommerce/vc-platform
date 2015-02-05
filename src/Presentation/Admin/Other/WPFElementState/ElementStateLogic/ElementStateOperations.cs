using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Virtoway.WPF.State
{
    public static class ElementStateOperations
    {
        #region Fields

        private static readonly Persistency _persistency = new Persistency();

        /// <stringValue>Holds the state of each registered element</stringValue>
        private static readonly Dictionary<string, ElementState> _states =
            new Dictionary<string, ElementState>();

        #endregion

        #region Properties

        internal static Persistency Persistency
        {
            get { return ElementStateOperations._persistency; }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Load persist elements state from XML stream
        /// </summary>
        public static void Load(Stream stream)
        {
            // Read persist elements state from XML stream
            _persistency.Load(stream);
        }

        /// <summary>
        /// Save persist elements state into XML stream
        /// </summary>
        /// <param name="stream"></param>
        public static void Save(Stream stream)
        {
            // Save persist elements state into XML stream
            _persistency.Save(stream);
        }

        public static object GetPropertyValue(DependencyObject element, DependencyProperty property)
        {
            string uid = GetUId(element);
            if (!_states.ContainsKey(uid))
            {
                throw new ArgumentException(string.Format("The property {0}.{1} is not in state", element, property.Name));
            }
            ElementState state = _states[uid];
            return state.GetValue(property);
        }

        public static bool HasPropertyValue(DependencyObject element, DependencyProperty property)
        {
            string uid = GetUId(element);
            if (!_states.ContainsKey(uid))
            {
                return false;
            }
            ElementState state = _states[uid];
            return state.HasValue(property);
        }

        public static object AddPropertyValue(DependencyObject element, DependencyProperty property, object value)
        {
            string uid = GetUId(element);
            ElementState state = null;
            if (_states.ContainsKey(uid))
            {
                state = _states[uid];
            }
            else
            {
                state = new ElementState(element);
                _states.Add(uid, state);
            }
            return state.AddValue(property, value);
        }

        public static List<Property> GetProperties(string uid)
        {
            return _persistency.GetProperties(uid);
        }

        public static void SetPropertyValue(string uid, string property, string value)
        {
            _persistency.Update(uid, property, value);
        }

        public static BindingBase CreateBinding(DependencyObject element, DependencyProperty property)
        {
            string uid = GetUId(element);
            ElementState state = _states[uid];

            Binding binding = new Binding();
            binding.Mode = BindingMode.TwoWay;
            binding.Converter = new PropertyValueConverter()
            {
                State = _states[uid],
                Target = element,
                Property = property
            };
            binding.Source = _states;
            binding.Path = new PropertyPath(string.Format("[{0}]", uid));
            return binding;
        }

        #endregion

        #region Internal Methods

        private static string GetUId(DependencyObject element)
        {
            string uid = ElementState.GetUId(element);
            if (uid == string.Empty)
            {
                throw new ArgumentException(string.Format("You must set the ElementState.UId attached property for element {0}", element));
            }
            return uid;
        }

        #endregion
    }
}
