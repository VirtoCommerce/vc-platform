using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class HostingEnviromentExtension
    {
        public static string GetRelativePath(this IHostingEnvironment hostEnv, string basePath, string path)
        {
            var basePathuri = new Uri(hostEnv.MapPath(basePath));
            var pathUri = new Uri(path);
            return "/" + basePathuri.MakeRelativeUri(pathUri).ToString();
        }

        public static string MapPath(this IHostingEnvironment hostEnv, string path)
        {
            var result = hostEnv.WebRootPath;

            if (path.StartsWith("~/"))
            {
                result = Path.Combine(result, path.Replace("~/", string.Empty).Replace('/', Path.DirectorySeparatorChar));
            }
            else if (Path.IsPathRooted(path))
            {
                result = path;
            }

            return result;
        }
    }
}
