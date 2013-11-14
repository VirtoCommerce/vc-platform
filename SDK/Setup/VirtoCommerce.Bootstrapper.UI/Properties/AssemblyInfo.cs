using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.Bootstrapper.UI;

[assembly: AssemblyTitle("VirtoCommerce SDK UI")]
[assembly: AssemblyDescription("UI for SDK setup")]

[assembly: ComVisible(false)]

[assembly: BootstrapperApplication(typeof(Installer))]
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

