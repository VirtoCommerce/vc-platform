using System.IO;

namespace VirtoCommerce.ConfigurationUtility.Main.Infrastructure
{
    internal static class DirectoryExtensions
    {
        internal static void Copy(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (var directory in source.EnumerateDirectories())
            {
                Copy(directory, destination.CreateSubdirectory(directory.Name));
            }
            foreach (var file in source.EnumerateFiles())
            {
                file.CopyTo(Path.Combine(destination.FullName, file.Name));
            }
        }
    }
}