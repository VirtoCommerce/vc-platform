using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Omu.ValueInjecter;

namespace VirtoCommerce.Platform.Data.Common.ConventionInjections
{
    /// <summary>
    /// Allows to inject only specified properties not contains null value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PatchInjection<T> : ConventionInjection
    {
        private bool _injectNullValue = false;
        private List<string> _propertyNames = new List<string>();
        public PatchInjection(params Expression<Func<T, object>>[] propertyNames)
            :this(injectNullValues: false , propertyNames: propertyNames)
        {
        }
        public PatchInjection(bool injectNullValues = false, params Expression<Func<T, object>>[] propertyNames)
        {
            _injectNullValue = injectNullValues;
            if (propertyNames != null)
            {
                foreach (var lambda in propertyNames.Select(x => (LambdaExpression)x))
                {
                    MemberExpression memberExpression;
                    if (lambda.Body is UnaryExpression)
                    {
                        var unaryExpression = (UnaryExpression)lambda.Body;
                        memberExpression = (MemberExpression)unaryExpression.Operand;
                    }
                    else
                    {
                        memberExpression = (MemberExpression)lambda.Body;
                    }
                    _propertyNames.Add(memberExpression.Member.Name);
                }
            }
        }

        protected override bool Match(ConventionInfo c)
        {
            var retVal = c.SourceProp.Name == c.TargetProp.Name &&
                       c.SourceProp.Type == c.TargetProp.Type && (_injectNullValue ? true : c.SourceProp.Value != null)
                       && _propertyNames.Contains(c.SourceProp.Name);
            return retVal;
        }
    }
}
