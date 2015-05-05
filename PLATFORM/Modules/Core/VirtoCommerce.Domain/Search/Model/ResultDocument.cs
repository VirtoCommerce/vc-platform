using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Search
{
    public class ResultDocument : IDocument
    {
        private List<IDocumentField> _Fields = new List<IDocumentField>();
        public int FieldCount
        {
            get { return _Fields.Count; }
        }

        public void Add(IDocumentField field)
        {
            _Fields.Add(field);
        }

        public bool ContainsKey(string name)
        {
            System.Collections.IEnumerator it = _Fields.GetEnumerator();
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
            System.Collections.IEnumerator it = _Fields.GetEnumerator();
            while (it.MoveNext())
            {
                IDocumentField field = (IDocumentField)it.Current;
                if (field.Name.Equals(name))
                {
                    _Fields.Remove(field);
                    return;
                }
            }
        }

        public IDocumentField this[int index]
        {
            get
            {
                if (_Fields.Count < index)
                    throw new IndexOutOfRangeException();

                if (_Fields != null && _Fields.Count > index)
                    return _Fields[index];

                return null;
            }
        }

        public IDocumentField this[string name]
        {
            get
            {
                foreach (IDocumentField field in _Fields)
                {
                    if (field.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return field;
                }

                return null;
            }
            private set
            {
                int index = 0;
                foreach (IDocumentField field in _Fields)
                {
                    if (field.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        _Fields[index] = value;
                        return;
                    }

                    index++;
                }
            }

        }
    }
}
