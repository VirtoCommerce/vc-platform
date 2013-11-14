using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Search
{
    public interface IDocument
    {
        int FieldCount { get; }
        void Add(IDocumentField field);
        void RemoveField(string name);
        bool ContainsKey(string name);
        IDocumentField this[int index] { get; }
        IDocumentField this[string name] { get; }
    }
}
