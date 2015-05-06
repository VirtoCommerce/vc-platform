using DotLiquid;
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Models
{
    public class SubmitFormErrors : Drop, IEnumerable<string>
    {
        public SubmitFormErrors(string field, string errorMessage)
        {
            if (_fields == null)
            {
                _fields = new List<string>();
                Messages = new Dictionary<string, string>();
            }

            _fields.Add(field);
            Messages.Add(field, errorMessage);
        }

        public SubmitFormErrors(IDictionary<string, string> errors)
        {
            _fields = errors.Keys;
            Messages = errors;
        }

        private ICollection<string> _fields { get; set; }

        public IDictionary<string, string> Messages { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (string field in _fields)
            {
                yield return field;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (string field in _fields)
            {
                yield return field;
            }
        }
    }
}