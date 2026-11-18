using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace VirtoCommerce.Platform.Security.Extensions;

public static class IPAddressExtensions
{
    private const int IPv4AddressLength = 4;

    // 2002:AABB:CCDD::/48 carries AA.BB.CC.DD
    private const int SixToFourIPv4Offset = 2;

    private static readonly IPNetwork[] _nonPublicNetworks =
    [
        IPNetwork.Parse("0.0.0.0/8"),
        IPNetwork.Parse("10.0.0.0/8"),
        IPNetwork.Parse("100.64.0.0/10"),
        IPNetwork.Parse("127.0.0.0/8"),
        IPNetwork.Parse("169.254.0.0/16"),
        IPNetwork.Parse("172.16.0.0/12"),
        IPNetwork.Parse("192.0.0.0/24"),
        IPNetwork.Parse("192.0.2.0/24"),
        IPNetwork.Parse("192.88.99.0/24"),
        IPNetwork.Parse("192.168.0.0/16"),
        IPNetwork.Parse("198.18.0.0/15"),
        IPNetwork.Parse("198.51.100.0/24"),
        IPNetwork.Parse("203.0.113.0/24"),
        IPNetwork.Parse("224.0.0.0/4"),
        IPNetwork.Parse("240.0.0.0/4"),
        IPNetwork.Parse("::/96"),
        IPNetwork.Parse("64:ff9b:1::/48"),
        IPNetwork.Parse("100::/64"),
        IPNetwork.Parse("2001::/32"),
        IPNetwork.Parse("2001:2::/48"),
        IPNetwork.Parse("2001:10::/28"),
        IPNetwork.Parse("2001:db8::/32"),
        IPNetwork.Parse("3fff::/20"),
        IPNetwork.Parse("5f00::/16"),
        IPNetwork.Parse("fc00::/7"),
        IPNetwork.Parse("fe80::/10"),
        IPNetwork.Parse("fec0::/10"),
        IPNetwork.Parse("ff00::/8"),
    ];

    // IPv6 networks that carry an IPv4 address, which must be public too
    private static readonly IPNetwork _nat64Network = IPNetwork.Parse("64:ff9b::/96");
    private static readonly IPNetwork _6to4Network = IPNetwork.Parse("2002::/16");

    public static bool IsPublic(this IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.IsIPv4MappedToIPv6)
        {
            return address.MapToIPv4().IsPublic();
        }

        if (_nat64Network.Contains(address))
        {
            return new IPAddress(address.GetAddressBytes()[^IPv4AddressLength..]).IsPublic();
        }

        if (_6to4Network.Contains(address))
        {
            return new IPAddress(address.GetAddressBytes()[SixToFourIPv4Offset..(SixToFourIPv4Offset + IPv4AddressLength)]).IsPublic();
        }

        return address.AddressFamily is AddressFamily.InterNetwork or AddressFamily.InterNetworkV6 &&
            !_nonPublicNetworks.Any(x => x.Contains(address));
    }
}
