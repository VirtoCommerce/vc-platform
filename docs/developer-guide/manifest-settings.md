# Declarative platform settings in `module.manifest`

> **Audience:** module authors (.NET and frontend-only), platform team.
>
> **Companion docs:**
> [Backoffice Modularity overview](backoffice-modularity.md) · [Framework reference](backoffice-modularity-framework.md)

---

## 1. Why

Until now, the platform's settings system required every module to register
its settings programmatically from `IModule.Initialize(IServiceCollection)`:

```csharp
public void Initialize(IServiceCollection services)
{
    var settingsRegistrar = services.BuildServiceProvider()
                                    .GetRequiredService<ISettingsRegistrar>();
    settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
}
```

That works for modules shipping a .NET assembly. **It does not work for
frontend-only modules** — the modularity framework explicitly supports
modules with no `<assemblyFile>` and no `<moduleType>`:

- [`vc-module-system-operations`](https://github.com/VirtoCommerce/vc-module-system-operations) — Vue 3 standalone SPA.
- [`samples/VirtoCommerce.SystemOperations.SampleExtension/`](https://github.com/VirtoCommerce/vc-module-system-operations/tree/main/samples/VirtoCommerce.SystemOperations.SampleExtension) — MF plugin.
- Any custom `<app>` shipped via the platform `<apps>` mechanism without a `.dll`.

The `<settings>` element on `<module>` lets any module — assembly or not —
ship settings as XML. The platform parses the element at startup and feeds
the descriptors into the **existing** `ISettingsRegistrar`, so:

- The same v2 settings API serves them.
- The same admin settings UI renders them.
- The same value store persists them.
- Programmatic registration keeps working — the two paths are purely additive.

---

## 2. Element schema

`<settings>` appears at the top level of `<module>` and contains zero or
more `<setting>` children. Each `<setting>` mirrors the C# type
[`SettingDescriptor`](../../src/VirtoCommerce.Platform.Core/Settings/SettingDescriptor.cs)
field-for-field.

```xml
<module>
  <id>VirtoCommerce.SystemOperations</id>
  <version>3.1001.0</version>
  …

  <settings>

    <!-- Minimal: name + groupName + valueType. -->
    <setting>
      <name>VirtoCommerce.SystemOperations.EnableExperimentalUi</name>
      <groupName>System Operations|UI</groupName>
      <valueType>Boolean</valueType>
      <defaultValue>false</defaultValue>
    </setting>

    <!-- Integer with display label. -->
    <setting>
      <name>VirtoCommerce.SystemOperations.MaxBackupRetentionDays</name>
      <groupName>System Operations|Backup</groupName>
      <displayName>Max backup retention (days)</displayName>
      <valueType>PositiveInteger</valueType>
      <defaultValue>30</defaultValue>
      <restartRequired>false</restartRequired>
    </setting>

    <!-- Enum-style: ShortText + AllowedValues. -->
    <setting>
      <name>VirtoCommerce.SystemOperations.PreferredAccountType</name>
      <groupName>System Operations|Auth</groupName>
      <displayName>Preferred account type</displayName>
      <valueType>ShortText</valueType>
      <defaultValue>EmailAndPassword</defaultValue>
      <allowedValues>
        <value>EmailAndPassword</value>
        <value>EmailWithLink</value>
        <value>Sso</value>
      </allowedValues>
    </setting>

    <!-- Secret value. -->
    <setting>
      <name>VirtoCommerce.SystemOperations.WebhookSecret</name>
      <groupName>System Operations|Integrations</groupName>
      <valueType>SecureString</valueType>
    </setting>

    <!-- Public setting (exposed via XAPI to storefront / external clients). -->
    <setting>
      <name>VirtoCommerce.SystemOperations.PublicMessage</name>
      <groupName>System Operations|UI</groupName>
      <valueType>LongText</valueType>
      <isPublic>true</isPublic>
    </setting>

    <!-- Free-form structured value. -->
    <setting>
      <name>VirtoCommerce.SystemOperations.RetryPolicy</name>
      <groupName>System Operations|Reliability</groupName>
      <valueType>Json</valueType>
      <defaultValue>{ "maxRetries": 3, "backoffSeconds": 5 }</defaultValue>
    </setting>

  </settings>
</module>
```

### 2.1 Element reference

| Child element | Type | Required | Maps to `SettingDescriptor` | Notes |
|---|---|---|---|---|
| `<name>` | string | **yes** | `Name` | Globally unique. Convention: `<ModuleId>.<Group>.<Name>` |
| `<groupName>` | string | yes | `GroupName` | Pipe-delimited tree, e.g. `System Operations\|Backup` |
| `<displayName>` | string | no | `DisplayName` | Raw English label. Localization is a frontend concern (see §5) |
| `<valueType>` | enum | **yes** | `ValueType` | One of: `ShortText`, `LongText`, `Integer`, `PositiveInteger`, `Decimal`, `DateTime`, `Boolean`, `SecureString`, `Json` |
| `<defaultValue>` | string | no | `DefaultValue` | Always XML-string; the platform coerces to `ValueType` at registration time |
| `<allowedValues>` | array of `<value>` | no | `AllowedValues` | Same coercion rules as `<defaultValue>`. Useful for enum-style settings |
| `<isRequired>` | bool | no | `IsRequired` | Default `false` |
| `<isHidden>` | bool | no | `IsHidden` | When `true`, omitted from settings UI |
| `<isPublic>` | bool | no | `IsPublic` | When `true`, exposed via XAPI to non-admin clients |
| `<isDictionary>` | bool | no | `IsDictionary` | Editable lookup list, no concrete value |
| `<isLocalizable>` | bool | no | `IsLocalizable` | The setting **value** itself is localizable (per-locale storage) |
| `<restartRequired>` | bool | no | `RestartRequired` | Surfaces a "restart required" banner in the settings UI |

### 2.2 Type coercion

XML carries everything as strings. At registration time, the platform
parses `<defaultValue>` and each `<value>` under `<allowedValues>` based
on the declared `<valueType>`:

| `valueType` | Coercion |
|---|---|
| `ShortText` / `LongText` / `SecureString` / `Json` | string verbatim |
| `Integer` / `PositiveInteger` | `int.Parse(invariant)` |
| `Decimal` | `decimal.Parse(invariant)` |
| `Boolean` | `bool.Parse` (`"true"` / `"false"`, case-insensitive) |
| `DateTime` | `DateTime.Parse(invariant, RoundtripKind)` |

A coercion failure does **not** fail the module. The offending setting is
skipped and an entry is added to `ManifestModuleInfo.Errors`, which
surfaces under the **Modules** admin UI alongside any other module-load
errors — the rest of the module's settings (and the rest of the module
itself) load normally.

---

## 3. Where it lives in the platform code

The implementation is small and additive. Reference for platform
maintainers; module authors do not need to touch any of these:

| File | Role |
|---|---|
| [`Core/Modularity/ManifestSetting.cs`](../../src/VirtoCommerce.Platform.Core/Modularity/ManifestSetting.cs) | XML POCO. `ToSettingDescriptor(moduleId)` instance method does the type coercion and stamps the module id |
| [`Core/Modularity/ModuleManifest.cs`](../../src/VirtoCommerce.Platform.Core/Modularity/ModuleManifest.cs) | Adds `[XmlArray("settings"), XmlArrayItem("setting")] ManifestSetting[] Settings` |
| [`Core/Modularity/ManifestModuleInfo.cs`](../../src/VirtoCommerce.Platform.Core/Modularity/ManifestModuleInfo.cs) | Adds `ICollection<SettingDescriptor> Settings`. Populated in `LoadFromManifest` with try/catch around each setting; failures land in `Errors` |
| [`Web/Extensions/ApplicationBuilderExtensions.cs`](../../src/VirtoCommerce.Platform.Web/Extensions/ApplicationBuilderExtensions.cs) | `UsePlatformSettings()` iterates installed modules and calls `ISettingsRegistrar.RegisterSettings(module.Settings, module.Id)` after registering Platform settings |

### 3.1 Registration order

Inside `UsePlatformSettings()`:

1. `Platform` settings register first (the legacy programmatic path).
2. Each installed module's manifest-declared settings register next.
3. Each module's `IModule.Initialize` runs *later* in the platform pipeline
   and may register additional settings programmatically.

`ISettingsRegistrar` is last-writer-wins, so:

- A `.NET` module that declares `<settings>` in its manifest **and** also
  calls `RegisterSettings(...)` from `IModule.Initialize` ends up with the
  programmatic value — useful during a gradual XML migration.
- A frontend-only module's manifest settings are final; nothing later
  can override them (because there's no `IModule.Initialize` to run).

### 3.2 Tests

| File | Coverage |
|---|---|
| [`tests/.../Modularity/ManifestSettingTests.cs`](../../tests/VirtoCommerce.Platform.Tests/Modularity/ManifestSettingTests.cs) | Coercion across all 9 `SettingValueType` values, `AllowedValues` per-entry coercion, error paths, flag preservation, module-id stamping |
| [`tests/.../Modularity/ManifestModuleInfoSettingsTests.cs`](../../tests/VirtoCommerce.Platform.Tests/Modularity/ManifestModuleInfoSettingsTests.cs) | End-to-end manifest-XML → registered descriptor flow, including error isolation (one bad setting doesn't break the others) |

---

## 4. Reading manifest-declared settings via the v2 API

There is **no new API** for declared settings. Once registered, they
appear in the v2 controllers exactly like programmatic ones.

### 4.1 Schema (metadata only)

```http
GET /api/platform/settings/v2/global/schema?moduleId=VirtoCommerce.SystemOperations
```

Response:

```json
[
  {
    "name": "VirtoCommerce.SystemOperations.RestartTimeoutSeconds",
    "groupName": "System Operations|Restart",
    "displayName": "Restart polling timeout (seconds)",
    "valueType": "PositiveInteger",
    "defaultValue": 120,
    "moduleId": "VirtoCommerce.SystemOperations",
    "restartRequired": false
  },
  {
    "name": "VirtoCommerce.SystemOperations.DefaultTheme",
    "groupName": "System Operations|UI",
    "displayName": "Default theme",
    "valueType": "ShortText",
    "defaultValue": "system",
    "allowedValues": ["system", "light", "dark"],
    "moduleId": "VirtoCommerce.SystemOperations"
  },
  {
    "name": "VirtoCommerce.SystemOperations.AllowDestructiveOperations",
    "groupName": "System Operations|Safety",
    "displayName": "Allow destructive operations",
    "valueType": "Boolean",
    "defaultValue": true,
    "moduleId": "VirtoCommerce.SystemOperations"
  }
]
```

The `moduleId` query filter is implemented by the existing
`SettingsPropertyService.GetSchemaAsync`
([`SettingsV2Controller.cs:37`](../../src/VirtoCommerce.Platform.Web/Controllers/Api/SettingsV2Controller.cs)),
so no controller change was needed.

### 4.2 Current values

```http
GET /api/platform/settings/v2/global/values?modifiedOnly=false
```

Response (flat dictionary keyed by setting name):

```json
{
  "VirtoCommerce.SystemOperations.RestartTimeoutSeconds": 120,
  "VirtoCommerce.SystemOperations.DefaultTheme": "system",
  "VirtoCommerce.SystemOperations.AllowDestructiveOperations": true,
  …
}
```

`modifiedOnly=true` returns only those whose persisted value differs from
the registered default — useful for diff-style admin views.

### 4.3 Save updates

```http
POST /api/platform/settings/v2/global/values
Content-Type: application/json

{
  "VirtoCommerce.SystemOperations.RestartTimeoutSeconds": 300,
  "VirtoCommerce.SystemOperations.AllowDestructiveOperations": false
}
```

Returns `204 No Content`. Add `?replaceAll=true` to reset every other
setting to its default in the same call.

### 4.4 Curl recipe (smoke test)

```bash
# 1. List your module's settings
curl -s -b session.cookies \
  'https://platform/api/platform/settings/v2/global/schema?moduleId=VirtoCommerce.SystemOperations' \
  | jq

# 2. Read current value for one setting
curl -s -b session.cookies \
  'https://platform/api/platform/settings/v2/global/values' \
  | jq '."VirtoCommerce.SystemOperations.RestartTimeoutSeconds"'

# 3. Update it
curl -s -b session.cookies -X POST \
  -H 'Content-Type: application/json' \
  -d '{"VirtoCommerce.SystemOperations.RestartTimeoutSeconds": 300}' \
  'https://platform/api/platform/settings/v2/global/values'
```

---

## 5. Reading from a frontend SPA — the `useModuleSettings` composable

`vc-module-system-operations` ships a reactive Vue 3 composable that
encapsulates all three v2 endpoints. Other custom SPAs are encouraged to
copy it verbatim — the contract is intentionally tiny.

**Source:**
[`vc-module-system-operations/src/.../app/composables/useModuleSettings.ts`](https://github.com/VirtoCommerce/vc-module-system-operations/blob/main/src/VirtoCommerce.SystemOperations.Web/app/composables/useModuleSettings.ts)

**API:**

```ts
export function useModuleSettings(moduleId: string): {
  schema: Ref<SettingSchema[]>;             // /schema?moduleId=…
  values: Ref<Record<string, unknown>>;     // /values, filtered to this module
  loaded: Ref<boolean>;                     // load() succeeded at least once
  error:  Ref<ApiError | null>;             // last fetch/save error
  load(): Promise<void>;                    // refetch schema + values
  save(updates: Record<string, unknown>): Promise<void>;
  get<T>(name: string, fallback?: T): T;    // persisted → default → fallback
};
```

**Typical use site:**

```vue
<script setup lang="ts">
import { onMounted, computed } from 'vue';
import { useModuleSettings } from './composables/useModuleSettings';

const moduleSettings = useModuleSettings('VirtoCommerce.SystemOperations');

// `get(name, fallback)` resolves: persisted value → schema default → fallback
const restartTimeout = computed(() =>
  moduleSettings.get<number>('VirtoCommerce.SystemOperations.RestartTimeoutSeconds', 120),
);
const allowDestructive = computed(() =>
  moduleSettings.get<boolean>('VirtoCommerce.SystemOperations.AllowDestructiveOperations', true),
);

onMounted(async () => {
  // Don't block the UI on settings — the page should render with fallbacks
  // first, then refine once settings arrive.
  try {
    await moduleSettings.load();
  } catch {
    // 401/403/network — fall back to defaults silently.
  }
});

async function setTimeoutSeconds(seconds: number) {
  await moduleSettings.save({
    'VirtoCommerce.SystemOperations.RestartTimeoutSeconds': seconds,
  });
  // values is reactive — bound `:value` updates automatically
}
</script>

<template>
  <div v-if="allowDestructive">
    <button @click="restartPlatform">Restart (timeout: {{ restartTimeout }}s)</button>
  </div>
</template>
```

**Notes:**

- `load()` is best-effort — failures are caught and surfaced via `error`.
  The page should render with fallbacks first and refine after.
- `values` is filtered to settings *owned by* `moduleId`. Even if the
  global `/values` endpoint returns settings from other modules, this
  composable only exposes its own — preventing accidental writes into a
  neighbour's namespace.
- `save(updates)` optimistically merges into `values` after a successful
  POST, so the UI doesn't need a second round-trip.

### 5.1 Wiring settings into existing composables without coupling

A common pattern is to thread a setting into a composable that already
exists, without making the composable depend on the settings system.
The `useOperations` composable in System Operations does this:

```ts
// useOperations.ts
export interface UseOperationsOptions {
  /** Returns the current platform-restart polling timeout in seconds. */
  restartTimeoutSeconds?: () => number;
}

export function useOperations(dialog, t, options: UseOperationsOptions = {}) {
  // …
  const timeoutSeconds = options.restartTimeoutSeconds?.() ?? 120;
  // …
}
```

```ts
// App.vue
const { restartPlatform } = useOperations(dialog, t, {
  restartTimeoutSeconds: () =>
    moduleSettings.get<number>('VirtoCommerce.SystemOperations.RestartTimeoutSeconds', 120),
});
```

The composable stays settings-agnostic; the *getter* (not the value) is
passed in so a setting change mid-session takes effect on the next call
without remounting.

---

## 6. Localization

Settings register their `<displayName>` as **raw text** (no resource
keys) — matching today's C#-registered settings. Per-locale labels are
the frontend's responsibility:

- The platform admin's existing `Localizations/{lang}.{moduleId}.json`
  loads as a translation namespace; the AngularJS settings UI looks up
  setting display names against it by setting `Name`.
- Custom SPAs (e.g. System Operations) follow the same pattern via their
  `app/locales/*.json` files.

So a manifest declaring `<displayName>Max backup retention (days)</displayName>`
is the **English fallback**. Put translations in
`Localizations/de.VirtoCommerce.SystemOperations.json`, etc., keyed by the
setting's `Name`:

```json
{
  "VirtoCommerce.SystemOperations.MaxBackupRetentionDays": "Maximale Backup-Aufbewahrung (Tage)"
}
```

This keeps the manifest XML free of localization plumbing and reuses the
JSON-translation pattern modules already maintain.

---

## 7. Coexistence with programmatic registration

| Module shape | What changes |
|---|---|
| Has `IModule.Initialize` calling `RegisterSettings(...)` programmatically | **Nothing.** Programmatic path keeps working unchanged |
| Has `IModule.Initialize` and wants to migrate | Move setting definitions to `<settings>` in `module.manifest`, delete the `RegisterSettings` call. Manifest registers first; programmatic registers second; last-writer-wins lets you migrate gradually |
| Frontend-only module (no `.NET` assembly) | Add `<settings>` to `module.manifest`, no other code change. Read via v2 API |
| Module with `<startupType>` only (no full `IModule`) | Manifest path works fine; `<startupType>` lifecycle is independent |

---

## 8. Reference example — System Operations

`vc-module-system-operations` ships **three real settings** that exercise
the breadth of the schema. Use them as a copy-paste starting point.

**Manifest fragment**
([`vc-module-system-operations/.../module.manifest`](https://github.com/VirtoCommerce/vc-module-system-operations/blob/main/src/VirtoCommerce.SystemOperations.Web/module.manifest)):

```xml
<settings>
  <!-- Tunable replacement for a previously-hardcoded restart polling
       timeout. Cold-start time on customer infra varies; ops can tune
       without a rebuild. -->
  <setting>
    <name>VirtoCommerce.SystemOperations.RestartTimeoutSeconds</name>
    <groupName>System Operations|Restart</groupName>
    <displayName>Restart polling timeout (seconds)</displayName>
    <valueType>PositiveInteger</valueType>
    <defaultValue>120</defaultValue>
  </setting>

  <!-- Platform-wide default theme; user override (localStorage) wins
       on a per-browser basis. -->
  <setting>
    <name>VirtoCommerce.SystemOperations.DefaultTheme</name>
    <groupName>System Operations|UI</groupName>
    <displayName>Default theme</displayName>
    <valueType>ShortText</valueType>
    <defaultValue>system</defaultValue>
    <allowedValues>
      <value>system</value>
      <value>light</value>
      <value>dark</value>
    </allowedValues>
  </setting>

  <!-- Production safety: when false, hides Reset Cache and Restart
       Platform cards. Page still renders for diagnostics. -->
  <setting>
    <name>VirtoCommerce.SystemOperations.AllowDestructiveOperations</name>
    <groupName>System Operations|Safety</groupName>
    <displayName>Allow destructive operations</displayName>
    <valueType>Boolean</valueType>
    <defaultValue>true</defaultValue>
  </setting>
</settings>
```

| Setting | Type demonstrated | Replaces |
|---|---|---|
| `RestartTimeoutSeconds` | `PositiveInteger` + `<defaultValue>` | A previously-hardcoded `60 * 2000ms` polling loop |
| `DefaultTheme` | `ShortText` + `<allowedValues>` enum-style | A previously-hardcoded `'system'` fallback |
| `AllowDestructiveOperations` | `Boolean` | New capability — production safety mode |

**Wiring in `App.vue`** (excerpts):

```ts
const moduleSettings = useModuleSettings('VirtoCommerce.SystemOperations');
const allowDestructiveOperations = computed(() =>
  moduleSettings.get<boolean>(
    'VirtoCommerce.SystemOperations.AllowDestructiveOperations', true),
);

// useOperations consults this getter on each restart click — not at
// composable-construction time — so admin updates take effect without
// remounting.
const { restartPlatform } = useOperations(dialog, t, {
  restartTimeoutSeconds: () =>
    moduleSettings.get<number>(
      'VirtoCommerce.SystemOperations.RestartTimeoutSeconds', 120),
});
```

```vue
<!-- Reset Cache and Restart Platform cards: hidden when admin opts out. -->
<OperationCard v-if="allowDestructiveOperations" … />
<OperationCard v-if="allowDestructiveOperations" … />
```

---

## 9. Out of scope (today)

- **Tenant-scoped settings.** The C# API exposes `RegisterSettingsForType`
  for per-store / per-organisation settings. `<settings>` only handles
  globals. Tenant scope can be added later as a `<settings forType="Store">`
  attribute or a `<tenantSettings>` sibling element.
- **Runtime mutation of the schema.** Settings are registered once at
  startup. Adding a new `<setting>` element to a deployed module's manifest
  requires a platform restart — same as the programmatic path.
- **Changing a registered setting's `<valueType>`.** Changing the type of an
  already-persisted setting can corrupt stored values. The platform doesn't
  guard against this — bump the setting's `<name>` if you need to replace it.
- **JSON-schema-style validation** beyond `AllowedValues` and required-ness.
  Use `valueType=Json` for free-form structured settings; richer validation
  remains an admin-UI concern.

---

## 10. Reference

- `SettingDescriptor` ([Core/Settings/SettingDescriptor.cs](../../src/VirtoCommerce.Platform.Core/Settings/SettingDescriptor.cs))
- `SettingValueType` ([Core/Settings/SettingValueType.cs](../../src/VirtoCommerce.Platform.Core/Settings/SettingValueType.cs))
- `ISettingsRegistrar` ([Core/Settings/ISettingsRegistrar.cs](../../src/VirtoCommerce.Platform.Core/Settings/ISettingsRegistrar.cs))
- `SettingsV2Controller` ([Web/Controllers/Api/SettingsV2Controller.cs](../../src/VirtoCommerce.Platform.Web/Controllers/Api/SettingsV2Controller.cs))
- `ManifestSetting` ([Core/Modularity/ManifestSetting.cs](../../src/VirtoCommerce.Platform.Core/Modularity/ManifestSetting.cs))
- `useModuleSettings` composable ([vc-module-system-operations/.../app/composables/useModuleSettings.ts](https://github.com/VirtoCommerce/vc-module-system-operations/blob/main/src/VirtoCommerce.SystemOperations.Web/app/composables/useModuleSettings.ts))
- Backoffice Modularity overview: [`backoffice-modularity.md`](backoffice-modularity.md)
- Modularity framework reference: [`backoffice-modularity-framework.md`](backoffice-modularity-framework.md)
