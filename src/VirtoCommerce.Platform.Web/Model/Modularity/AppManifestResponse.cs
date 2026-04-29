using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Model.Modularity
{
    /// <summary>
    /// Response shape for <c>GET /api/apps/{appId}/manifest</c>.
    /// Returns the host app metadata plus a topologically ordered, permission-filtered
    /// list of plugins the host should load.
    /// </summary>
    public class AppManifestResponse
    {
        /// <summary>
        /// Echo of the requested app id (e.g. <c>vc-shell-marketplace</c>, <c>system-operations</c>, <c>platform</c>).
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Version of the host app — taken from the running platform when
        /// <see cref="AppId"/> is the reserved <c>platform</c> id, otherwise
        /// from the module that declares the <c>&lt;app&gt;</c> in its
        /// <c>module.manifest</c>. Useful for clients that want to surface the
        /// host's version (e.g. footer banner, support diagnostics) without
        /// a second round-trip.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Display title of the host app, taken from the owning module's
        /// <c>&lt;app&gt;</c> declaration in <c>module.manifest</c>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Plugins to load, in topological dependency order of their owning modules.
        /// Already filtered by current user permissions and (for Module Federation
        /// hosts) by host-app discovery folder convention.
        /// </summary>
        public IList<PluginEntry> Plugins { get; set; } = new List<PluginEntry>();
    }
}
