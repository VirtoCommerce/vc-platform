using System;
using System.Text.Json;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Testing
{
    public static class DeepCloneCheckerExtensions
    {
        /// <summary>
        /// Check object clone immutability and independency:
        /// Ensures the data in object clone equial to original.
        /// Ensures no shared references between original and cloned objects (each object is a fully independent).
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static async Task AssertCloneIndependency(this ICloneable original)
        {
            await Task.Run(() =>
            {
                var clone = original.Clone();
                var sOriginal = JsonSerializer.Serialize((object)original, new JsonSerializerOptions() { WriteIndented = true });
                var sClone = JsonSerializer.Serialize(clone, new JsonSerializerOptions() { WriteIndented = true });
                if (!sOriginal.Equals(sClone)) // Ensure data in objects is equal
                {
                    throw new PlatformException(@$"Clone check failed: object and clone not equal.");
                }
                original.AssertNoSharedRefsWith(clone); // Ensure no shared references between objects (each object is a fully independent)
            });
        }

        /// <summary>
        /// Ensures no shared references with expected object (original and expected objects are fully independent from each other).
        /// </summary>
        /// <param name="original"></param>
        /// <param name="expected"></param>
        public static void AssertNoSharedRefsWith(this object original, object expected)
        {
            if (expected == null) throw new NullReferenceException("Expected object should not be null.");

            var originalEnumerator = original.TraverseObjectGraph().GetEnumerator();
            var expectedEnumerator = expected.TraverseObjectGraph().GetEnumerator();

            while (originalEnumerator.MoveNext() && expectedEnumerator.MoveNext())
            {
                if (originalEnumerator.Current.Key != expectedEnumerator.Current.Key)
                {
                    throw new MemberAccessException(@$"Original value resides at path ""{originalEnumerator.Current.Key}"", but current path in expected object is ""{expectedEnumerator.Current.Key}""");
                }

                if (!(originalEnumerator.Current.Value.IsPrimitive()) && ReferenceEquals(originalEnumerator.Current.Value, expectedEnumerator.Current.Value))
                {
                    throw new MemberAccessException(@$"Deep clone check failed: objects at path ""{originalEnumerator.Current.Key}"" are reference equal.");
                }
            }
        }
    }
}
