using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

/// <summary>
/// A thin read-only adapter implementing ILocalModuleCatalog that wraps preloaded modules.
/// Provides backward compatibility for code that resolves ILocalModuleCatalog from DI
/// (e.g., UseModulesAndAppsFiles, ModulesHealthChecker, etc.).
/// No InnerLoad — modules are already loaded by the static module system.
/// Mutation methods (AddModule, Reload) throw NotSupportedException.
/// </summary>
[Obsolete("Use ModuleRegistry instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class LocalModuleCatalogAdapter : ModuleCatalog, ILocalModuleCatalog
{
    public LocalModuleCatalogAdapter(IEnumerable<ManifestModuleInfo> modules, ModuleSequenceBoostOptions boostOptions = null)
        : base(modules, Options.Create(boostOptions ?? new ModuleSequenceBoostOptions()))
    {
    }

    protected override void InnerLoad()
    {
        // No-op: modules were already loaded by ModuleManifestReader + ModuleLoader
    }

    public override void AddModule(ModuleInfo moduleInfo)
    {
        throw new NotSupportedException("Cannot add modules to a read-only catalog. Modules are pre-loaded at startup by the static module pipeline.");
    }

    public override void Reload()
    {
        throw new NotSupportedException("Cannot reload a read-only catalog. Modules are pre-loaded at startup by the static module pipeline.");
    }
}
