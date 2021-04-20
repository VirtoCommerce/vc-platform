using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octokit;

namespace PlatformTools
{
    class GithubManager
    {
        private static string GithubUser = "virtocommerce";
        private static string PlatformRepo = "vc-platform";
        private static GitHubClient client = new GitHubClient(new ProductHeaderValue("vc-build"));
        public static async Task<Release> GetPlatformRelease(string releaseTag)
        {
            var release = string.IsNullOrEmpty(releaseTag)
                ? await client.Repository.Release.GetLatest(GithubUser, PlatformRepo)
                : await client.Repository.Release.Get(GithubUser, PlatformRepo, releaseTag);
            return release;
        }

        /// <summary>
        /// Gets a repo owner and a repo name from packageUrl
        /// </summary>
        /// <param name="url"></param>
        /// <returns>The First Value is Owner, The Second is Repo Name</returns>
        public static Tuple<string, string> GetRepoFromUrl(string url)
        {
            var regex = new Regex(@"http[s]{0,1}:\/\/github.com\/([A-z0-9]*)\/([A-z0-9\-]*)\/", RegexOptions.IgnoreCase);
            var match = regex.Match(url);
            var groups = match.Groups;
            return new Tuple<string, string>(groups[1].Value, groups[2].Value);
        }

        public static async Task<Release> GetModuleRelease(string moduleRepo, string releaseTag)
        {
            Release release;
            if (string.IsNullOrEmpty(releaseTag))
            {
                release = await client.Repository.Release.GetLatest(GithubUser, moduleRepo);
            }
            else
            {
                release = await client.Repository.Release.Get(GithubUser, moduleRepo, releaseTag);
            }
            return release;
        }
    }
}
