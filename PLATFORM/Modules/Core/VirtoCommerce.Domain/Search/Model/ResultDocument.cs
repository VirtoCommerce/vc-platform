using System;
using System.Collections.Generic;

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
            _fields.Add(field);
        }

        public bool ContainsKey(string name)
        {
            System.Collections.IEnumerator it = _fields.GetEnumerator();
            while (it.MoveNext())
            {
                IDocumentField field = (IDocumentField)it.Current;
                if (field.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }


        public void RemoveField(string name)
        {
            System.Collections.IEnumerator it = _fields.GetEnumerator();
            while (it.MoveNext())
            {
                IDocumentField field = (IDocumentField)it.Current;
                if (field.Name.Equals(name))
                {
                    _fields.Remove(field);
                    return;
                }
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
                foreach (IDocumentField field in _fields)
                {
                    if (field.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return field;
                }

                return null;
            }
            private set
            {
                int index = 0;
                foreach (IDocumentField field in _fields)
                {
                    if (field.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        _fields[index] = value;
                        return;
                    }

                    index++;
                }
            }

        }
    }
}
