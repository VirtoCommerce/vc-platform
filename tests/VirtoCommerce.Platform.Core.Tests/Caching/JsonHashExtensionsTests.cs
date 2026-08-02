using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Caching
{
    public class JsonHashExtensionsTests
    {
        // Mirrors the writer's internal buffer; the tests below derive drain boundaries from it rather than
        // hard-coding payload lengths, because the JSON around the payload shifts where the seam lands.
        private const int BufferSize = 4096;

        [Fact]
        public void GetJsonSha256Hex_EqualGraphsBuiltSeparately_ProduceEqualHashes()
        {
            // Arrange — two graphs with no shared instance
            var first = BuildTree(new AndNode(), "alpha", "beta");
            var second = BuildTree(new AndNode(), "alpha", "beta");

            // Act & Assert
            first.GetJsonSha256Hex().Should().Be(second.GetJsonSha256Hex());
        }

        [Fact]
        public void GetJsonSha256Hex_GraphsDifferingOnlyInLeafValue_ProduceDifferentHashes()
        {
            var first = BuildTree(new AndNode(), "alpha", "beta");
            var second = BuildTree(new AndNode(), "alpha", "gamma");

            first.GetJsonSha256Hex().Should().NotBe(second.GetJsonSha256Hex());
        }

        [Fact]
        public void GetJsonSha256Hex_GraphsDifferingOnlyInPolymorphicRuntimeType_ProduceDifferentHashes()
        {
            // THIS is the test that pins TypeNameHandling in the shipped settings: drop the discriminator
            // from CreateCacheKeySettings and this fails. AndNode and OrNode are structurally identical —
            // each declares exactly one member of the same type — so nothing else can tell them apart.
            var and = BuildTree(new AndNode(), "alpha", "beta");
            var or = BuildTree(new OrNode(), "alpha", "beta");

            and.GetJsonSha256Hex().Should().NotBe(or.GetJsonSha256Hex());
        }

        [Fact]
        public void GetJsonSha256Hex_WithoutTypeNameHandling_CollidesOnPolymorphicRuntimeType()
        {
            // Validates the FIXTURE of the test above rather than the shipped settings: it proves AndNode
            // and OrNode really are indistinguishable without a discriminator, so that test's pass is
            // evidence about the discriminator and not about some incidental difference between the types.
            var withoutDiscriminator = JsonHashExtensions.CreateCacheKeySettings();
            withoutDiscriminator.TypeNameHandling = TypeNameHandling.None;

            var and = BuildTree(new AndNode(), "alpha", "beta");
            var or = BuildTree(new OrNode(), "alpha", "beta");

            and.GetJsonSha256Hex(withoutDiscriminator).Should().Be(or.GetJsonSha256Hex(withoutDiscriminator));
        }

        [Fact]
        public void GetJsonSha256Hex_NullSettings_Throws()
        {
            // Newtonsoft treats null settings as "defaults", which emits no $type — i.e. silently produces
            // the collision this class exists to prevent, and returns a plausible digest while doing it.
            var act = () => BuildTree(new AndNode(), "alpha").GetJsonSha256Hex(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetJsonSha256Hex_NullValue_HashesTheJsonNullLiteral()
        {
            object value = null;

            value.GetJsonSha256Hex().Should().Be(NonStreamingHash(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(16)]
        [InlineData(BufferSize * 3)]
        public void GetJsonSha256Hex_PayloadOfVariousSizes_MatchesNonStreamingHash(int payloadLength)
        {
            var value = LeafOf(new string('x', payloadLength));

            value.GetJsonSha256Hex().Should().Be(NonStreamingHash(value));
        }

        [Fact]
        public void GetJsonSha256Hex_PayloadEndingOnADrainBoundary_MatchesNonStreamingHash()
        {
            // The seam is not at a round payload length: the JSON ahead of the payload — property names and
            // the assembly-qualified $type of every node — pushes it. Measure the overhead and derive the
            // exact padding, so this keeps testing the boundary after any rename to the fixture types.
            var overhead = SerializedLength(LeafOf(string.Empty));
            var atSeam = BufferSize - overhead;

            atSeam.Should().BeGreaterThan(0, "the fixture's JSON overhead must be smaller than one buffer");

            foreach (var padding in new[] { atSeam - 1, atSeam, atSeam + 1 })
            {
                var value = LeafOf(new string('x', padding));

                value.GetJsonSha256Hex().Should().Be(NonStreamingHash(value),
                    "the digest must not depend on where the buffer happens to drain (padding {0})", padding);
            }
        }

        [Fact]
        public void GetJsonSha256Hex_SurrogatePairStraddlingADrainBoundary_MatchesNonStreamingHash()
        {
            // A surrogate pair is two chars, and the writer drains every BufferSize chars. When the pair is
            // split across a drain, only a STATEFUL encoder carries the high half over; a stateless one
            // emits a replacement character for each half and the digest diverges.
            //
            // Sweep a whole buffer's worth of alignments rather than picking offsets: an earlier version of
            // this test hand-picked five and passed against a deliberately broken encoder, i.e. proved
            // nothing. The same mistake is why the boundary test above computes its seam.
            for (var padding = BufferSize; padding < BufferSize * 2 + 8; padding++)
            {
                var value = LeafOf(new string('x', padding) + "\U0001F600tail");

                value.GetJsonSha256Hex().Should().Be(NonStreamingHash(value),
                    "the surrogate pair must survive a drain boundary (padding {0})", padding);
            }
        }

        [Fact]
        public void GetJsonSha256Hex_ConcurrentCalls_AllMatchTheReferenceHash()
        {
            // Concurrency is the only way a pooled-buffer bug shows itself: a double return, or an array
            // handed out while another writer still holds it, aliases two callers onto one buffer. A
            // sequential loop cannot surface that. Comparing against the reference — not merely against the
            // first result — is what makes a uniformly wrong digest fail rather than pass.
            var value = LeafOf(new string('y', BufferSize * 2 + 137));
            var expected = NonStreamingHash(value);
            var results = new ConcurrentBag<string>();

            Parallel.For(0, 64, _ => results.Add(value.GetJsonSha256Hex()));

            results.Should().HaveCount(64).And.OnlyContain(x => x == expected);
        }

        // Reference implementation: serialize to a string, then hash its UTF8 bytes. The streaming writer
        // exists to avoid materializing that string; it must produce the same digest as if it had not.
        // Uses the shipped settings factory so the two sides cannot drift apart silently.
        private static string NonStreamingHash(object value)
        {
            var json = JsonConvert.SerializeObject(value, JsonHashExtensions.CreateCacheKeySettings());

            return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(json)));
        }

        private static int SerializedLength(object value)
        {
            return JsonConvert.SerializeObject(value, JsonHashExtensions.CreateCacheKeySettings()).Length;
        }

        private static Holder LeafOf(string value)
        {
            return new Holder { Root = new AndNode { Children = [new Leaf { Value = value }] } };
        }

        private static Holder BuildTree(NodeBase root, params string[] leafValues)
        {
            root.Children = [];

            foreach (var leafValue in leafValues)
            {
                root.Children.Add(new Leaf { Value = leafValue });
            }

            return new Holder { Root = root };
        }

        private sealed class Holder
        {
            public INode Root { get; set; }
        }

        private interface INode
        {
        }

        private abstract class NodeBase : INode
        {
            public IList<INode> Children { get; set; }
        }

        // Structurally identical siblings — the shape a content hash cannot tell apart without a type name.
        private sealed class AndNode : NodeBase
        {
        }

        private sealed class OrNode : NodeBase
        {
        }

        private sealed class Leaf : INode
        {
            public string Value { get; set; }
        }
    }
}
