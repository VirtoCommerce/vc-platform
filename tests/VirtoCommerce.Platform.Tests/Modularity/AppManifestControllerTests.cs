using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Model.Modularity;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class AppManifestControllerTests
{
    [Fact]
    public void GetManifest_WritesContributionsAsTheObjectTheyAre_NotAsAString()
    {
        var controller = NewController(new PluginDescriptor
        {
            Id = "sales-rep",
            Contributions = """{"format":1,"when":{"setting":"SalesRep.Enabled"}}""",
        });

        var json = Serialize(controller.GetManifest("vc-frontend"));

        var contributions = json["Plugins"]![0]!["Contributions"];
        Assert.Equal(JTokenType.Object, contributions!.Type);
        Assert.Equal(1, contributions["format"]!.Value<int>());
        Assert.Equal("SalesRep.Enabled", contributions["when"]!["setting"]!.Value<string>());
    }

    [Fact]
    public void GetManifest_WritesNullContributions_WhenThePluginDeclaresNone()
    {
        var controller = NewController(new PluginDescriptor { Id = "sales-rep" });

        var json = Serialize(controller.GetManifest("vc-frontend"));

        Assert.Equal(JTokenType.Null, json["Plugins"]![0]!["Contributions"]!.Type);
    }

    private static AppManifestController NewController(PluginDescriptor plugin)
    {
        var service = new Mock<IAppManifestService>();
        service.Setup(s => s.GetManifest("vc-frontend")).Returns(new AppManifestDescriptor
        {
            AppId = "vc-frontend",
            Hash = "ABC",
            Plugins = [plugin],
        });

        var hostEnvironment = new Mock<IHostEnvironment>();
        hostEnvironment.SetupGet(x => x.EnvironmentName).Returns(Environments.Production);

        return new AppManifestController(service.Object, hostEnvironment.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
    }

    private static JObject Serialize(ActionResult<AppManifestResponse> result)
    {
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        return JObject.Parse(JsonConvert.SerializeObject(ok.Value));
    }
}
