using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Search.Model
{
    public class ResultDocument : IDocument
    {
        private readonly List<IDocumentField> _fields = new List<IDocumentField>();

        public IEnumerable<IDocumentField> Fields { get { return _fields; } }

        public int FieldCount
        {
            get { return _fields.Count; }
        }

        public void Add(IDocumentField field)
        {
            var existingField = this[field.Name];

            if (existingField != null)
            {
                existingField.AddValue(field.Value);
            }
            else
            {
                _fields.Add(field);
            }
        }

        public bool ContainsKey(string name)
        {
            return this[name] != null;
        }


        public void RemoveField(string name)
        {
            var field = this[name];

            if (field != null)
            {
                _fields.Remove(field);
            }
        }

        public IDocumentField this[int index]
        {
            get
            {
                if (_fields.Count < index)
                    throw new IndexOutOfRangeException();

                if (_fields != null && _fields.Count > index)
                    return _fields[index];

                return null;
            }
        }

        public IDocumentField this[string name]
        {
            get
            {
                return _fields.FirstOrDefault(field => field.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
