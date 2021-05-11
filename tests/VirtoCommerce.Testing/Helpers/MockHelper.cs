using System;
using AutoFixture;

namespace VirtoCommerce.Testing.Helpers
{
    public class MockHelper
    {
        protected const int DEFAULT_PAGE_SIZE = 50;
        protected readonly Fixture _fixture = new();

        /// <summary>
        /// Helper for testing
        /// </summary>
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"

        public class TestComparableClass : IComparable
#pragma warning restore S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
        {
            public int Value { get; set; }

            public int CompareTo(object obj)
            {
                return Value.CompareTo((obj as TestComparableClass).Value);
            }
        }
    }
}
