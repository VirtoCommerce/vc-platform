using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
    public class PolymorphismRegistrar : IPolymorphismRegistrar
    {
        private readonly Dictionary<string, HashSet<IPolymorphicBaseTypeInfo>> _modulePolymorphicTypes = new Dictionary<string, HashSet<IPolymorphicBaseTypeInfo>>(StringComparer.InvariantCultureIgnoreCase);

        public void RegisterPolymorphicBaseType(string moduleName, IPolymorphicBaseTypeInfo polymorphismBaseTypeInfo)
        {
            if (moduleName == null)
            {
                throw new ArgumentNullException(nameof(moduleName));
            }

            if (polymorphismBaseTypeInfo == null)
            {
                throw new ArgumentNullException(nameof(polymorphismBaseTypeInfo));
            }

            if (!_modulePolymorphicTypes.ContainsKey(moduleName))
            {
                _modulePolymorphicTypes.Add(moduleName, new HashSet<IPolymorphicBaseTypeInfo>(AnonymousComparer.Create<IPolymorphicBaseTypeInfo, string>(x => x.Type.FullName)));
            }

            if (_modulePolymorphicTypes[moduleName].Contains(polymorphismBaseTypeInfo))
            {
                throw new ArgumentException($"\"{polymorphismBaseTypeInfo.Type.FullName}\" polymorphic base type already registered for \"{moduleName}\" module.");
            }

            _modulePolymorphicTypes[moduleName].Add(polymorphismBaseTypeInfo);
        }

        public IPolymorphicBaseTypeInfo[] GetPolymorphicBaseTypes(string moduleName)
        {
            var result = Array.Empty<IPolymorphicBaseTypeInfo>();

            var needAllTypes = string.IsNullOrEmpty(moduleName);

            if (needAllTypes)
            {
                result = _modulePolymorphicTypes.Values.SelectMany(x => x).Distinct().ToArray();
            }

            if (!needAllTypes && _modulePolymorphicTypes.ContainsKey(moduleName))
            {
                result = _modulePolymorphicTypes[moduleName].ToArray();
            }

            return result;
        }
    }
}
