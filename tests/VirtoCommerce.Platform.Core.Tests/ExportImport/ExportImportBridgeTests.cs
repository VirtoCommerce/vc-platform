using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.ExportImport;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.ExportImport;

[Trait("Category", "Unit")]
public class ExportImportBridgeTests
{
    private sealed class ModernExporter : IExportSupport
    {
        public CancellationToken ReceivedToken { get; private set; }

        public Task ExportAsync(Stream outStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            ReceivedToken = cancellationToken;
            return Task.CompletedTask;
        }
    }

    private sealed class ModernImporter : IImportSupport
    {
        public CancellationToken ReceivedToken { get; private set; }

        public Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            ReceivedToken = cancellationToken;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task IExportSupport_ModernOverload_ReceivesCancellationToken()
    {
        var impl = new ModernExporter();
        IExportSupport exporter = impl;
        using var cts = new CancellationTokenSource();

        await exporter.ExportAsync(Stream.Null, new ExportImportOptions(), _ => { }, cts.Token);

        Assert.Equal(cts.Token, impl.ReceivedToken);
    }

    [Fact]
#pragma warning disable VC0014
    public async Task IExportSupport_LegacyOverload_ThrowsNotImplementedException()
#pragma warning restore VC0014
    {
        //Arrange — class that only implements the modern overload
        var impl = new ModernExporter();
        IExportSupport exporter = impl;

        //Act & Assert — legacy DIM throws NotImplementedException
#pragma warning disable VC0014
        await Assert.ThrowsAsync<NotImplementedException>(
            () => exporter.ExportAsync(Stream.Null, new ExportImportOptions(), _ => { }, null));
#pragma warning restore VC0014
    }

    [Fact]
    public async Task IImportSupport_ModernOverload_ReceivesCancellationToken()
    {
        var impl = new ModernImporter();
        IImportSupport importer = impl;
        using var cts = new CancellationTokenSource();

        await importer.ImportAsync(Stream.Null, new ExportImportOptions(), _ => { }, cts.Token);

        Assert.Equal(cts.Token, impl.ReceivedToken);
    }

    [Fact]
#pragma warning disable VC0014
    public async Task IImportSupport_LegacyOverload_ThrowsNotImplementedException()
#pragma warning restore VC0014
    {
        //Arrange — class that only implements the modern overload
        var impl = new ModernImporter();
        IImportSupport importer = impl;

        //Act & Assert — legacy DIM throws NotImplementedException
#pragma warning disable VC0014
        await Assert.ThrowsAsync<NotImplementedException>(
            () => importer.ImportAsync(Stream.Null, new ExportImportOptions(), _ => { }, null));
#pragma warning restore VC0014
    }
}
