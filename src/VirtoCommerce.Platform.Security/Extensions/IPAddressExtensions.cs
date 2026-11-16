using System;
using System.Net;
using System.Net.Sockets;

namespace VirtoCommerce.Platform.Security.Extensions;

public static class IPAddressExtensions
{
    private static readonly (uint Network, int PrefixLength)[] _nonPublicIPv4Networks =
    [
        (0x00000000, 8),    // 0.0.0.0/8
        (0x0A000000, 8),    // 10.0.0.0/8
        (0x64400000, 10),   // 100.64.0.0/10
        (0x7F000000, 8),    // 127.0.0.0/8
        (0xA9FE0000, 16),   // 169.254.0.0/16
        (0xAC100000, 12),   // 172.16.0.0/12
        (0xC0000000, 24),   // 192.0.0.0/24
        (0xC0000200, 24),   // 192.0.2.0/24
        (0xC0586300, 24),   // 192.88.99.0/24
        (0xC0A80000, 16),   // 192.168.0.0/16
        (0xC6120000, 15),   // 198.18.0.0/15
        (0xC6336400, 24),   // 198.51.100.0/24
        (0xCB007100, 24),   // 203.0.113.0/24
        (0xE0000000, 4),    // 224.0.0.0/4
        (0xF0000000, 4),    // 240.0.0.0/4, 255.255.255.255 included
    ];

    public static bool IsPublic(this IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.IsIPv4MappedToIPv6)
        {
            address = address.MapToIPv4();
        }

        return address.AddressFamily switch
        {
            AddressFamily.InterNetwork => IsPublicIPv4(address),
            AddressFamily.InterNetworkV6 => IsPublicIPv6(address),
            _ => false,
        };
    }

    private static bool IsPublicIPv4(IPAddress address)
    {
        var bytes = address.GetAddressBytes();
        var value = (uint)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);

        foreach (var (network, prefixLength) in _nonPublicIPv4Networks)
        {
            var mask = uint.MaxValue << (32 - prefixLength);
            if ((value & mask) == network)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsPublicIPv6(IPAddress address)
    {
        if (address.Equals(IPAddress.IPv6Any) || address.Equals(IPAddress.IPv6Loopback) ||
            address.IsIPv6LinkLocal || address.IsIPv6SiteLocal || address.IsIPv6Multicast || address.IsIPv6UniqueLocal ||
            address.IsIPv6Teredo)
        {
            return false;
        }

        var bytes = address.GetAddressBytes();

        // IPv4-compatible addresses (::a.b.c.d, deprecated) and the documentation prefix 2001:db8::/32
        if (IsZero(bytes, 0, 12) || (bytes[0] == 0x20 && bytes[1] == 0x01 && bytes[2] == 0x0D && bytes[3] == 0xB8))
        {
            return false;
        }

        // An IPv4 address carried in IPv6: NAT64 (64:ff9b::/96) and 6to4 (2002::/16) must be public as IPv4 too
        if (bytes[0] == 0x00 && bytes[1] == 0x64 && bytes[2] == 0xFF && bytes[3] == 0x9B && IsZero(bytes, 4, 8))
        {
            return IsPublicIPv4(new IPAddress(bytes[12..16]));
        }

        if (bytes[0] == 0x20 && bytes[1] == 0x02)
        {
            return IsPublicIPv4(new IPAddress(bytes[2..6]));
        }

        return true;
    }

    private static bool IsZero(byte[] bytes, int offset, int count)
    {
        for (var i = offset; i < offset + count; i++)
        {
            if (bytes[i] != 0)
            {
                return false;
            }
        }

        return true;
    }
}
