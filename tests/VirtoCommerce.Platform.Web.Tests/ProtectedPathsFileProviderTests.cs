using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Moq;
using VirtoCommerce.Platform.Web.Infrastructure;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests;

[Trait("Category", "Unit")]
public class ProtectedPathsFileProviderTests
{
    private readonly Mock<IFileProvider> _innerProviderMock = new();

    [Theory]
    [InlineData("assets/sales-rep-documents", "/assets/sales-rep-documents/file.pdf")]
    [InlineData("assets/sales-rep-documents", "/assets/sales-rep-documents/nested/file.pdf")]
    [InlineData("assets/sales-rep-documents", "/assets/sales-rep-documents")]
    [InlineData("assets/sales-rep-documents", "/Assets/Sales-Rep-Documents/File.pdf")]
    [InlineData("/assets/sales-rep-documents/", "/assets/sales-rep-documents/file.pdf")]
    [InlineData("assets\\sales-rep-documents", "/assets/sales-rep-documents/file.pdf")]
    [InlineData("https://localhost:5001/assets/sales-rep-documents/", "/assets/sales-rep-documents/file.pdf")]
    public void GetFileInfo_ProtectedPath_ReturnsNotFound(string protectedPath, string subpath)
    {
        var provider = CreateProvider(protectedPath);

        var fileInfo = provider.GetFileInfo(subpath);

        Assert.IsType<NotFoundFileInfo>(fileInfo);
        _innerProviderMock.Verify(x => x.GetFileInfo(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("assets/sales-rep-documents", "/assets/sales-rep-documents-other/file.pdf")]
    [InlineData("assets/sales-rep-documents", "/assets/other/file.pdf")]
    [InlineData("assets/sales-rep-documents", "/assets/sales-rep")]
    [InlineData("assets/sales-rep-documents", "/index.html")]
    public void GetFileInfo_UnprotectedPath_DelegatesToInnerProvider(string protectedPath, string subpath)
    {
        var innerFileInfo = Mock.Of<IFileInfo>();
        _innerProviderMock.Setup(x => x.GetFileInfo(subpath)).Returns(innerFileInfo);
        var provider = CreateProvider(protectedPath);

        var fileInfo = provider.GetFileInfo(subpath);

        Assert.Same(innerFileInfo, fileInfo);
    }

    [Fact]
    public void GetDirectoryContents_ProtectedPath_ReturnsNotFound()
    {
        var provider = CreateProvider("assets/sales-rep-documents");

        var contents = provider.GetDirectoryContents("/assets/sales-rep-documents");

        Assert.Same(NotFoundDirectoryContents.Singleton, contents);
        _innerProviderMock.Verify(x => x.GetDirectoryContents(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void GetDirectoryContents_UnprotectedPath_DelegatesToInnerProvider()
    {
        var innerContents = Mock.Of<IDirectoryContents>();
        _innerProviderMock.Setup(x => x.GetDirectoryContents("/assets")).Returns(innerContents);
        var provider = CreateProvider("assets/sales-rep-documents");

        var contents = provider.GetDirectoryContents("/assets");

        Assert.Same(innerContents, contents);
    }

    [Fact]
    public void Watch_AnyFilter_DelegatesToInnerProvider()
    {
        var innerToken = Mock.Of<IChangeToken>();
        _innerProviderMock.Setup(x => x.Watch("assets/sales-rep-documents/**")).Returns(innerToken);
        var provider = CreateProvider("assets/sales-rep-documents");

        var token = provider.Watch("assets/sales-rep-documents/**");

        Assert.Same(innerToken, token);
    }

    [Fact]
    public void EmptyProtectedPaths_AllCallsDelegateToInnerProvider()
    {
        var innerFileInfo = Mock.Of<IFileInfo>();
        var innerContents = Mock.Of<IDirectoryContents>();
        _innerProviderMock.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(innerFileInfo);
        _innerProviderMock.Setup(x => x.GetDirectoryContents(It.IsAny<string>())).Returns(innerContents);
        var provider = CreateProvider();

        Assert.Same(innerFileInfo, provider.GetFileInfo("/assets/sales-rep-documents/file.pdf"));
        Assert.Same(innerContents, provider.GetDirectoryContents("/assets/sales-rep-documents"));
    }

    [Fact]
    public void Constructor_NullArguments_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ProtectedPathsFileProvider(null, new[] { "assets" }));
        Assert.Throws<ArgumentNullException>(() => new ProtectedPathsFileProvider(_innerProviderMock.Object, null));
    }

    [Theory]
    [InlineData("assets/sales-rep-documents", "assets/sales-rep-documents")]
    [InlineData("/assets/sales-rep-documents/", "assets/sales-rep-documents")]
    [InlineData("assets\\sales-rep-documents", "assets/sales-rep-documents")]
    [InlineData("https://localhost:5001/assets/sales-rep-documents", "assets/sales-rep-documents")]
    [InlineData("http://cdn.example.com/assets/sales-rep-documents/", "assets/sales-rep-documents")]
    public void NormalizePaths_ValidPath_ReturnsPathPrefix(string path, string expected)
    {
        var result = ProtectedPathsFileProvider.NormalizePaths([path]);

        Assert.Equal(expected, Assert.Single(result));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("/")]
    [InlineData("https://localhost:5001")]
    public void NormalizePaths_EmptyPath_IsDropped(string path)
    {
        var result = ProtectedPathsFileProvider.NormalizePaths([path]);

        Assert.Empty(result);
    }

    [Fact]
    public void NormalizePaths_NullArgument_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ProtectedPathsFileProvider.NormalizePaths(null));
    }

    private ProtectedPathsFileProvider CreateProvider(params string[] protectedPaths)
    {
        return new ProtectedPathsFileProvider(_innerProviderMock.Object, protectedPaths);
    }
}
