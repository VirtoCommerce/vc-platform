using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    //
    // Summary:
    //     Specifies the direction in which to sort a list of items.
    public enum SortDirection
    {
        //
        // Summary:
        //     Sort from smallest to largest. For example, from A to Z.
        Ascending = 0,
        //
        // Summary:
        //     Sort from largest to smallest. For example, from Z to A.
        Descending = 1
    }

    public sealed class SortInfo : IEquatable<SortInfo>
    {
        public override string ToString()
        {
            return SortColumn + ":" + (SortDirection == SortDirection.Descending ? "desc" : "asc");
        }
        public static string ToString(IEnumerable<SortInfo> sortInfos)
        {
            return string.Join(";", sortInfos);
        }
        public static IEnumerable<SortInfo> Parse(string sortExpr)
        {
            var retVal = new List<SortInfo>();
            if (String.IsNullOrEmpty(sortExpr))
                return retVal;

            var sortInfoStrings = sortExpr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sortInfoString in sortInfoStrings)
            {
                var parts = sortInfoString.Split(new[] { ':', '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Any())
                {
                    var sortInfo = new SortInfo
                    {
                        SortColumn = parts[0],
                        SortDirection = SortDirection.Ascending
                    };
                    if (parts.Count() > 1)
                    {
                        sortInfo.SortDirection = parts[1].StartsWith("desc", StringComparison.InvariantCultureIgnoreCase) ? SortDirection.Descending : SortDirection.Ascending;
                    }
                    retVal.Add(sortInfo);
                }
            }
            return retVal;
        }

        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; }

        public bool Equals(SortInfo other)
        {
            return other != null
                   && String.Equals(SortColumn, other.SortColumn, StringComparison.OrdinalIgnoreCase)
                   && SortDirection == other.SortDirection;
        }

        public override bool Equals(object obj)
        {
            SortInfo sortInfo = obj as SortInfo;
            if (sortInfo != null)
            {
                return Equals(sortInfo);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return SortColumn.GetHashCode();
        }
    }
}
