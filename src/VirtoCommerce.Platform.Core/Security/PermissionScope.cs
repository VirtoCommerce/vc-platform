using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class PermissionScope : ValueObject
    {
        public PermissionScope()
        {
            Type = GetType().Name;
        }

        /// <summary>
        /// Scope type name
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Display label for particular scope value used only for  representation 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Represent string scope value
        /// </summary>
        public string Scope { get; set; }

        //public virtual string ToJsonString()
        //{
        //    return $"{{Type: {Type}, Scope: {Scope}, Label: {Label}}}";
        //}

        ////we don't use Json serialization/deserialization due to difficult inject json options on this level
        //public static PermissionScope TryParse(string input)
        //{
        //    if (input == null)
        //    {
        //        throw new ArgumentNullException(nameof(input));
        //    }

        //    PermissionScope result = null;

        //    const string pattern = @"{Type: (?<type>\w+), Scope: (?<scope>[\w\{\}]+), Label: (?<label>\w+)}";
        //    var match = Regex.Match(input, pattern);
        //    if (match.Success)
        //    {
        //        var type = match.Groups["type"].Value;
        //        result = AbstractTypeFactory<PermissionScope>.TryCreateInstance(type);
        //        result.Type = type;
        //        result.Scope = match.Groups["scope"].Value;
        //        result.Label = match.Groups["label"].Value;
        //    }
        //    return result;
        //}

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Scope;
        }

        public virtual void Patch(PermissionScope target)
        {
            target.Label = Label;
            target.Scope = Scope;
        }

    }
}
