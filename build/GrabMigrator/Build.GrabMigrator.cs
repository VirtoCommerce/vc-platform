using Nuke.Common;

partial class Build : NukeBuild
{
    [Parameter("Grab-migrator config file")] readonly string GrabMigratorConfig;

    Target GrabMigrator => _ => _
     .Requires(() => GrabMigratorConfig)
     .Executes(() =>
     {
         new GrabMigrator.GrabMigrator().Do(GrabMigratorConfig);
     });

}
