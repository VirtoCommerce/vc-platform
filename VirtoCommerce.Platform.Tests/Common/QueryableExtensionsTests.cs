using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Common
{
    [CLSCompliant(false)]
    [Trait("Category", "CI")]
    public class QueryableExtensionsTests
    {
        class A { }

        class B : A
        {
            public int Prop { get; set; }
        }

        class C
        {
            public int Prop { get; set; }
        }

        readonly List<A> _bInsideListA = new List<A>
        {
            new B{Prop = 8},
            new B{Prop = 3},
            new B{Prop = 10},
            new B{Prop = 5}
        };

        readonly List<C> _simpleList = new List<C>
        {
            new C{Prop = 8},
            new C{Prop = 3},
            new C{Prop = 10},
            new C{Prop = 5}
        };

        public QueryableExtensionsTests()
        {
            if (!AbstractTypeFactory<A>.AllTypeInfos.Any())
            {
                AbstractTypeFactory<A>.RegisterType<B>();
            }
        }

        [Theory]
        [InlineData(SortDirection.Ascending, 3)]
        [InlineData(SortDirection.Descending, 10)]
        public void OrderByMustOrderDerrivedClassProperties(SortDirection direction, int expected)
        {
            var sortInfo = new[]
            {
                new SortInfo
                {
                    SortColumn = nameof(B.Prop),
                    SortDirection = direction
                }
            };

            var orderedList = _bInsideListA.AsQueryable().OrderBySortInfos(sortInfo).ToList();
            var firstB = (B)orderedList.First();
            Assert.Equal(expected, firstB.Prop);
        }

        [Theory]
        [InlineData(SortDirection.Ascending, 3)]
        [InlineData(SortDirection.Descending, 10)]
        public void OrderByMustOrderSimpleList(SortDirection direction, int expected)
        {
            var sortInfo = new[]
            {
                new SortInfo
                {
                    SortColumn = nameof(B.Prop),
                    SortDirection = direction
                }
            };

            var orderedList = _simpleList.AsQueryable().OrderBySortInfos(sortInfo).ToList();
            Assert.Equal(expected, orderedList.First().Prop);
        }
    }
}
