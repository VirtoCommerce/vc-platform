using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.ExportImport;

[Trait("Category", "Unit")]
public class ExportImportBridgeTests
{
#pragma warning disable VC0014 // Tests intentionally implement the legacy obsolete overload.
    private sealed class LegacyOnlyExporter : IExportSupport
    {
        public ICancellationToken ReceivedToken { get; private set; }

        public Task ExportAsync(Stream outStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            ReceivedToken = cancellationToken;
            return Task.CompletedTask;
        }
    }

    private sealed class LegacyOnlyImporter : IImportSupport
    {
        public ICancellationToken ReceivedToken { get; private set; }

        public Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            ReceivedToken = cancellationToken;
            return Task.CompletedTask;
        }
    }
#pragma warning restore VC0014

    [Fact]
    public async Task IExportSupport_ModernOverload_BridgesToLegacyImpl()
    {
        var impl = new LegacyOnlyExporter();
        // Default-interface-method overloads are only resolvable through the interface type.
        IExportSupport exporter = impl;
        using var cts = new CancellationTokenSource();

        await exporter.ExportAsync(Stream.Null, new ExportImportOptions(), _ => { }, cts.Token);

        Assert.NotNull(impl.ReceivedToken);
        Assert.IsType<CancellationTokenWrapper>(impl.ReceivedToken);
    }

    [Fact]
    public async Task IExportSupport_ModernOverloadCancellation_FlowsThroughBridge()
    {
        var impl = new LegacyOnlyExporter();
        IExportSupport exporter = impl;
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await exporter.ExportAsync(Stream.Null, new ExportImportOptions(), _ => { }, cts.Token);

        Assert.Throws<OperationCanceledException>(() => impl.ReceivedToken.ThrowIfCancellationRequested());
    }

    [Fact]
    public async Task IImportSupport_ModernOverload_BridgesToLegacyImpl()
    {
        var impl = new LegacyOnlyImporter();
        IImportSupport importer = impl;
        using var cts = new CancellationTokenSource();

        await importer.ImportAsync(Stream.Null, new ExportImportOptions(), _ => { }, cts.Token);

        Assert.NotNull(impl.ReceivedToken);
        Assert.IsType<CancellationTokenWrapper>(impl.ReceivedToken);
    }

    [Fact]
    public async Task IImportSupport_ModernOverloadCancellation_FlowsThroughBridge()
    {
        var impl = new LegacyOnlyImporter();
        IImportSupport importer = impl;
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await importer.ImportAsync(Stream.Null, new ExportImportOptions(), _ => { }, cts.Token);

        Assert.Throws<OperationCanceledException>(() => impl.ReceivedToken.ThrowIfCancellationRequested());
    }
}
