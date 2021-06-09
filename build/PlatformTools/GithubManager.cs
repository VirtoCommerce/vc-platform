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
        private static GitHubClient Client = new GitHubClient(new ProductHeaderValue("vc-build"));
        public static async Task<Release> GetPlatformRelease(string releaseTag)
        {
            var release = string.IsNullOrEmpty(releaseTag)
                ? await GetLatestReleaseAsync(GithubUser, PlatformRepo)
                : await Client.Repository.Release.Get(GithubUser, PlatformRepo, releaseTag);
            return release;
        }
        public static async Task<Release> GetPlatformRelease(string token, string releaseTag)
        {
            SetAuthToken(token);
            return await GetPlatformRelease(releaseTag);
        }
        public static void SetAuthToken(string token)
        {
            if(!string.IsNullOrEmpty(token))
                Client.Credentials = new Credentials(token);
        }

        private static async Task<Release> GetLatestReleaseAsync(string repoUser, string repoName)
        {
            var releases = await Client.Repository.Release.GetAll(repoUser, repoName, new ApiOptions() { PageSize = 5, PageCount = 1});
            var release = releases.OrderByDescending(r => r.TagName.Trim()).FirstOrDefault();
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
        public static async Task<Release> GetModuleRelease(string token, string moduleRepo, string releaseTag)
        {
            GithubManager.SetAuthToken(token);
            return await GithubManager.GetModuleRelease(moduleRepo, releaseTag);
        }
        public static async Task<Release> GetModuleRelease(string moduleRepo, string releaseTag)
        {
            Release release;
            if (string.IsNullOrEmpty(releaseTag))
            {
                release = await Client.Repository.Release.GetLatest(GithubUser, moduleRepo);
            }
            else
            {
                release = await Client.Repository.Release.Get(GithubUser, moduleRepo, releaseTag);
            }
            return release;
        }
    }
}
