using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InProcess;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

internal static class TestLocks
{
    public static InProcessDistributedLock CreateInProcess(DistributedLockOptions options = null)
    {
        return new InProcessDistributedLock(
            Microsoft.Extensions.Options.Options.Create(options ?? new DistributedLockOptions()),
            NullLogger<InProcessDistributedLock>.Instance);
    }
}
