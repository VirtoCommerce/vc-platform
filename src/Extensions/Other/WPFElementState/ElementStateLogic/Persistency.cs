using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Virtoway.WPF.State
{
    internal class Persistency
    {
        #region Fields

        private readonly Dictionary<string, Element> _elementLookupTable = new Dictionary<string, Element>();
        private State _state;

        #endregion

        #region Initializers

        internal Persistency()
        {
            this._state = new State();
        }

        #endregion

        #region Operations

        internal void Load(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(State));
            try
            {
                _state = (State)serializer.Deserialize(stream);
            }
            catch
            {
                // Failed to deserialize the stream. Using an empty state
            }
            InitializeLookupTables();
        }

        internal void Save(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(State));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(stream, _state, namespaces);
        }

        internal void Update(string uid, string propertyName, string value)
        {
            if (!_elementLookupTable.ContainsKey(uid))
            {
                // Add new element
                Element element = new Element();
                element.UId = uid;
                Property property = new Property();
                property.Name = propertyName;
                property.Value = value;
                element.Properties.Add(property);
                // Update the state
                _state.Elements.Add(element);
                // Update the lookup table
                _elementLookupTable.Add(element.UId, element);
                return;
            }

            Element foundElement = _elementLookupTable[uid];
            if (!Contains(foundElement, propertyName))
            {
                // Add new property
                Property property = new Property();
                property.Name = propertyName;
                property.Value = value;
                foundElement.Properties.Add(property);
                return;
            }

            // Update or remove existing property.
            Property foundProperty = GetProperty(foundElement, propertyName);
            if (value == null)
                foundElement.Properties.Remove(foundProperty);
            else
                foundProperty.Value = value;
        }

        internal bool Contains(string uid, string propertyName)
        {
            return _elementLookupTable.ContainsKey(uid) &&
                Contains(_elementLookupTable[uid], propertyName);
        }

        internal string GetValue(string uid, string propertyName)
        {
            Element element = _elementLookupTable[uid];
            return GetValue(element, propertyName);
        }

        internal List<Property> GetProperties(string uid)
        {
            return _elementLookupTable.ContainsKey(uid) ? _elementLookupTable[uid].Properties : null;
        }
        #endregion

        #region Internal Operations

        private void InitializeLookupTables()
        {
            foreach (Element element in _state.Elements)
            {
                _elementLookupTable.Add(element.UId, element);
            }
        }

        private bool Contains(Element element, string propertyName)
        {
            foreach (Property property in element.Properties)
            {
                if (property.Name == propertyName)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetValue(Element element, string propertyName)
        {
            foreach (Property property in element.Properties)
            {
                if (property.Name == propertyName)
                {
                    return property.Value;
                }
            }
            return string.Empty;
        }

        private Property GetProperty(Element element, string propertyName)
        {
            foreach (Property property in element.Properties)
            {
                if (property.Name == propertyName)
                {
                    return property;
                }
            }
            return null;
        }

        #endregion
    }
}
