# Settings V2 API and Unified Settings UI

## Overview

Settings V2 introduces a modernized settings experience inspired by Visual Studio 2026 Options dialog:

- **Unified Settings Blade** -- single wide blade with left tree navigation + right properties panel
- **Clean REST API** -- separates schema from values, returns only modified values
- **Global / Tenant scoping** -- URL path-based scope (`/global/` or `/tenant/{type}/{id}/`)
- **Data Source abstraction** -- clean isolation between API mode (global/tenant) and Entity mode (in-memory parent entity)
- **JSON import/export** -- save and load settings as JSON document files
- **Inline JSON editor** -- edit all settings as raw JSON with CodeMirror (syntax highlighting, folding, validation)
- **Reusable blade** -- same controller works for global settings, store settings, payment settings, and custom entities
- **Filter & search** -- filter popup (modified only, by module) + keyword search across names, values, and groups
- **Tenant property labels** -- in global view, settings assigned to tenants (e.g., Store) show a gray badge and can be hidden via filter
- **Deep linking** -- shareable URLs that scroll to a specific section or property (`?group=...&setting=...`)
- **Copy link** -- hover over a section legend to copy a direct link to clipboard
- **Dirty tracking** -- unsaved changes prompt on navigation, save button disabled until changes detected
- **Backward compatible** -- existing v1 API and UI remain fully functional

## Core Concepts

### Setting Name as Property Key

Each setting has a unique dot-notation `Name` (e.g., `"Catalog.ImageCategories"`, `"VirtoCommerce.Search.IndexingJobs.Enable"`). The V2 API uses this `Name` directly as the dictionary key in all value request/response payloads. Case-sensitive matching is used -- the API preserves the original `Name` casing from `SettingDescriptor` registration.

### Schema vs Values Separation

The API splits settings into two concerns:
- **Schema** (`GET .../schema`) -- metadata about each setting: type, default value, allowed values, group path, flags. No current values.
- **Values** (`GET .../values`) -- current values as a flat `{ "Name": value }` dictionary. Supports `modifiedOnly` filter.

This separation enables:
- Efficient "modified only" queries (compare against schema defaults server-side)
- Lightweight save payloads (send only changed values)
- Client-side tree building from schema `GroupName` without server overhead

### Scope: Global vs Tenant

Settings scope is expressed in the URL path:

| Scope | URL Pattern | Maps To |
|-------|-------------|---------|
| Global | `/api/platform/settings/v2/global/...` | `objectType=null, objectId=null` |
| Tenant | `/api/platform/settings/v2/tenant/{tenantType}/{tenantId}/...` | `objectType=tenantType, objectId=tenantId` |

Internally, `tenantType`/`tenantId` map to the existing `objectType`/`objectId` parameters in `ISettingsManager`. For tenant scope, the schema endpoint returns only settings registered for that type via `ISettingsRegistrar.RegisterSettingsForType()`.

The `tenantType` is the **short type name** (e.g., `"Store"`), extracted from the fully-qualified .NET type name (e.g., `"VirtoCommerce.StoreModule.Core.Model.Store"`) by splitting on `.` and taking the last segment.

### Client-Side Group Tree

The schema response is a **flat array** of property descriptors. The client builds the navigation tree by splitting each setting's `GroupName` on `|`.

Example: `GroupName: "Catalog|General"` produces:
```
Catalog
  General
```

The tree is rendered as a flat list with indentation (no recursive `ng-include`), using `padding-left` per depth level. An "All Settings" root node is prepended. When filters are active, only tree nodes containing matching settings (and their ancestors) are shown.

### Data Source Pattern

The controller uses a **Data Source abstraction** to cleanly isolate load/save operations. Two modes are supported:

| Mode | `isEntityMode` | Schema source | Values source | Save target |
|------|---------------|---------------|---------------|-------------|
| **API mode** | `false` | REST API | REST API | REST API |
| **Entity mode** | `true` | REST API | Parent entity in-memory | Parent entity in-memory |

Each data source implements three methods:
```javascript
{
    loadSchema: function() -> Promise<schemas[]>,
    loadValues: function() -> Promise<{ name: value }>,
    saveValues: function(changedValues) -> Promise
}
```

The rest of the controller (merge, tree building, filtering, dirty checking) is completely storage-agnostic.

---

## REST API Reference

### Base URL

```
api/platform/settings/v2
```

### Authentication & Permissions

| Action | Permission |
|--------|-----------|
| Read schema/values | `platform:setting:query` |
| Update values | `platform:setting:update` |

---

### GET .../schema

Returns property schema (metadata only, no values).

**Endpoints:**
```
GET /api/platform/settings/v2/global/schema
GET /api/platform/settings/v2/tenant/{tenantType}/{tenantId}/schema
```

**Query Parameters:**

| Param | Type | Required | Description |
|-------|------|----------|-------------|
| `moduleId` | string | No | Filter by module ID |
| `keyword` | string | No | Text search in Name, DisplayName, and GroupName |

**Response `200 OK`:**

```json
[
  {
    "name": "Catalog.ImageCategories",
    "displayName": "Image Categories",
    "groupName": "Catalog|General",
    "moduleId": "VirtoCommerce.Catalog",
    "valueType": "ShortText",
    "defaultValue": null,
    "allowedValues": ["Images"],
    "isRequired": false,
    "isReadOnly": false,
    "isDictionary": true,
    "isLocalizable": false,
    "restartRequired": false,
    "assignedToTenants": []
  },
  {
    "name": "VirtoCommerce.Search.IndexingJobs.Enable",
    "displayName": "Enable indexing jobs",
    "groupName": "VirtoCommerce.Search|Indexing",
    "moduleId": "VirtoCommerce.Search",
    "valueType": "Boolean",
    "defaultValue": true,
    "allowedValues": null,
    "isRequired": false,
    "isReadOnly": false,
    "isDictionary": false,
    "isLocalizable": false,
    "restartRequired": true,
    "assignedToTenants": []
  },
  {
    "name": "VirtoCommerce.Store.EnablePriceRounding",
    "displayName": "Enable price rounding",
    "groupName": "VirtoCommerce.Store|General",
    "moduleId": "VirtoCommerce.Store",
    "valueType": "Boolean",
    "defaultValue": false,
    "allowedValues": null,
    "isRequired": false,
    "isReadOnly": false,
    "isDictionary": false,
    "isLocalizable": false,
    "restartRequired": false,
    "assignedToTenants": ["Store"]
  }
]
```

**Schema Field Reference:**

| Field | Type | Description |
|-------|------|-------------|
| `name` | string | Unique setting identifier (dot-notation), used as dictionary key in values API |
| `displayName` | string | Human-readable label (falls back to `name` if not set) |
| `groupName` | string | Pipe-delimited group path for tree building (e.g., `"Catalog\|General"`) |
| `moduleId` | string | Owning module ID |
| `valueType` | string | One of: `ShortText`, `LongText`, `Integer`, `PositiveInteger`, `Decimal`, `DateTime`, `Boolean`, `SecureString`, `Json` |
| `defaultValue` | any | Default value when not explicitly set |
| `allowedValues` | array\|null | Enumerated allowed values (rendered as dropdown), or `null` for free-form |
| `isRequired` | bool | Whether a value is mandatory |
| `isReadOnly` | bool | `true` when value is forced by configuration override (`appsettings.json`) |
| `isDictionary` | bool | Setting is an editable dictionary (list of values) |
| `isLocalizable` | bool | Values can be localized |
| `restartRequired` | bool | Application restart needed after change |
| `assignedToTenants` | string[] | Tenant types this setting is registered for via `RegisterSettingsForType` (e.g., `["Store"]`). Empty array if global-only. |

---

### GET .../values

Returns current property values as a flat `{ "Name": value }` dictionary.

**Endpoints:**
```
GET /api/platform/settings/v2/global/values
GET /api/platform/settings/v2/tenant/{tenantType}/{tenantId}/values
```

**Query Parameters:**

| Param | Type | Required | Default | Description |
|-------|------|----------|---------|-------------|
| `modifiedOnly` | bool | No | `false` | When `true`, only return values that differ from `defaultValue` |

**Response `200 OK` -- all values (`modifiedOnly=false`):**

```json
{
  "Catalog.ImageCategories": null,
  "Catalog.EditorialReviewTypes": "QuickReview",
  "VirtoCommerce.Search.IndexingJobs.Enable": false,
  "VirtoCommerce.Search.IndexingJobs.CronExpression": "0/5 * * * *",
  "VirtoCommerce.Store.EnablePriceRounding": true
}
```

**Response `200 OK` -- modified only (`modifiedOnly=true`):**

```json
{
  "VirtoCommerce.Search.IndexingJobs.Enable": false,
  "VirtoCommerce.Store.EnablePriceRounding": true
}
```

Only settings whose current value differs from their schema `defaultValue` are returned.

> **Note:** The values endpoint uses a custom JSON serializer that preserves dictionary key casing (the platform's default `CamelCasePropertyNamesContractResolver` would lowercase keys, breaking Name matching).

---

### POST .../values

Update property values. Send only the settings you want to change.

**Endpoints:**
```
POST /api/platform/settings/v2/global/values
POST /api/platform/settings/v2/tenant/{tenantType}/{tenantId}/values
```

**Request Body:**

```json
{
  "VirtoCommerce.Search.IndexingJobs.Enable": true,
  "VirtoCommerce.Store.EnablePriceRounding": false,
  "VirtoCommerce.Search.IndexingJobs.CronExpression": "0/10 * * * *"
}
```

**Response `204 No Content`** on success.

**Response `400/500`** on validation error (e.g., unknown setting name).

**Value Type Conversion:** The service automatically converts incoming JSON values to the correct .NET type based on the setting's `ValueType`. Newtonsoft `JToken` and System.Text.Json `JsonElement` are both handled. Integer values use the correct target width (`int` for Integer/PositiveInteger, `long` otherwise) to prevent overflow.

---

## Settings Document File Format

For export/import/maintenance, settings values are wrapped in a JSON document envelope **client-side**:

### Global Settings File

```json
{
  "version": "1.0",
  "exportedAt": "2026-03-25T10:30:00Z",
  "scope": "global",
  "settings": {
    "VirtoCommerce.Search.IndexingJobs.Enable": false,
    "VirtoCommerce.Search.IndexingJobs.CronExpression": "0/15 * * * *",
    "VirtoCommerce.Store.EnablePriceRounding": true
  }
}
```

### Tenant-Scoped Settings File

```json
{
  "version": "1.0",
  "exportedAt": "2026-03-25T11:00:00Z",
  "scope": "tenant/Store/Electronics",
  "settings": {
    "VirtoCommerce.Store.EnablePriceRounding": true,
    "VirtoCommerce.Store.RoundingPolicy": "AwayFromZero"
  }
}
```

| Field | Type | Description |
|-------|------|-------------|
| `version` | string | Document format version (`"1.0"`) |
| `exportedAt` | string | ISO 8601 timestamp |
| `scope` | string | `"global"` or `"tenant/{tenantType}/{tenantId}"` |
| `settings` | object | Flat `{ "Name": value }` dictionary (modified values only by default) |

**Export flow:** `GET /values?modifiedOnly=true` -> client wraps in envelope -> browser file download.

**Import flow:** User uploads `.json` file -> client extracts `settings` dict -> confirmation dialog -> `POST /values`.

---

## Declaring Settings (Module Developers)

Settings are declared exactly as before -- no changes to module registration:

```csharp
public static SettingDescriptor ImageCategories { get; } = new SettingDescriptor
{
    Name = "Catalog.ImageCategories",
    GroupName = "Catalog|General",
    ValueType = SettingValueType.ShortText,
    IsDictionary = true,
    AllowedValues = ["Images"]
};
```

Registration in `Module.cs`:

```csharp
public void PostInitialize(IApplicationBuilder appBuilder)
{
    var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
    settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
    settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.StoreSettings, "Store");
}
```

The V2 API automatically picks up all registered settings. `GroupName` controls tree placement. `Name` is the property key. `DisplayName` provides the UI label (with i18n translation fallback via `settings.{Name}.title` key).

---

## Backend Architecture

### New Types

**`SettingPropertySchema`** (`VirtoCommerce.Platform.Core.Settings`)

Schema-only DTO projected from `SettingDescriptor`. The `IsReadOnly` flag is computed by checking if the setting has a configuration override via `ISettingsOverrideProvider`.

**`SettingsPropertySearchCriteria`** (`VirtoCommerce.Platform.Core.Settings`)

Extends `SearchCriteriaBase` with `ModuleId` and `Keyword` filters.

**`ISettingsPropertyService`** (`VirtoCommerce.Platform.Core.Settings`)

```csharp
public interface ISettingsPropertyService
{
    Task<IReadOnlyList<SettingPropertySchema>> GetSchemaAsync(
        SettingsPropertySearchCriteria criteria, string tenantType = null);

    Task<Dictionary<string, object>> GetValuesAsync(
        string tenantType = null, string tenantId = null, bool modifiedOnly = false);

    Task SaveValuesAsync(Dictionary<string, object> values,
        string tenantType = null, string tenantId = null);
}
```

**`SettingsPropertyService`** (`VirtoCommerce.Platform.Data.Settings`)

Implementation that delegates to existing `ISettingsManager` and `ISettingsRegistrar`:

- `GetSchemaAsync` -- iterates `AllRegisteredSettings`, maps to `SettingPropertySchema`, filters by criteria (hidden settings excluded, keyword search on Name/DisplayName/GroupName). For tenant scope, intersects with `GetSettingsForType(tenantType)`.
- `GetValuesAsync` -- loads all setting values via `GetObjectSettingsAsync(names, tenantType, tenantId)`. Returns `Dictionary<Name, Value>` with case-insensitive key matching. When `modifiedOnly=true`, compares each value against `DefaultValue` using string comparison.
- `SaveValuesAsync` -- resolves each dictionary key to a registered `SettingDescriptor` (throws on unknown names). Converts values via `ConvertValue()` which handles Newtonsoft `JToken` and System.Text.Json `JsonElement` unwrapping with correct integer width. Creates `ObjectSettingEntry` objects and calls `SaveObjectSettingsAsync`.

**`SettingsV2Controller`** (`VirtoCommerce.Platform.Web.Controllers.Api`)

REST controller at `api/platform/settings/v2` with 6 endpoints (3 global + 3 tenant). Uses a custom `JsonSerializerSettings` with `DefaultNamingStrategy` for value responses to preserve dictionary key casing.

### Relation to Existing API

The existing `SettingController` at `api/platform/settings` is **completely unchanged**. Both APIs coexist and share the same `ISettingsManager` data layer.

---

## Frontend Architecture

### Data Source Abstraction

The controller creates a data source object at initialization based on `blade.isEntityMode`:

**API Data Source** (`isEntityMode = false`):
- `loadSchema()` -- calls `GET .../schema`
- `loadValues()` -- calls `GET .../values`
- `saveValues(changed)` -- calls `POST .../values`

**Entity Data Source** (`isEntityMode = true`):
- `loadSchema()` -- calls `GET .../tenant/{type}/{id}/schema` (schema always from API)
- `loadValues()` -- reads `blade.parentBlade.currentEntity.settings` array, converts to `{ name: value }` dict (no API call)
- `saveValues(changed)` -- writes values back to parent entity's in-memory settings array (no API call)

All other controller logic (merge, tree, filters, dirty checking) is storage-agnostic and works identically in both modes.

### Unified Settings Blade

Controller: `platformWebApp.settingsUnifiedController`

The blade calls `dataSource.loadSchema()` and `dataSource.loadValues()` in parallel, then merges schema and values client-side into `ObjectSettingEntry`-compatible objects that `va-generic-value-input` can render without changes.

**Layout:**

```
+-----------------------------------------------------------------------+
| [Filter] [Search keyword...................................] [x]      |
+-----------------------------------------------------------------------+
| TREE (300px)           | PROPERTIES (600px+, scrollable)               |
|                        |                                               |
| All Settings           | Catalog > General                             |
|   Application Insights | ────────────────────                          |
|   v Back In Stock      | * Catalog.ImageCategories  [?] [reset]       |
|     General            |   [Images v]                                  |
|   v CMS Content        |                                               |
|     General            | * Catalog.EditorialReview  [?] [reset]        |
|     Migration          |   [QuickReview v]                             |
|   v Cart               |                                               |
|     General            | VirtoCommerce.Search > Indexing                |
|   v Catalog            | ────────────────────                          |
|     Brands             |   Indexing cron expression  [?] [reset]       |
|     General            |   [0/5 * * * *]                               |
|     Search             |                                               |
+-----------------------------------------------------------------------+
```

- **Left panel (300px):** Flat tree rendered with `ng-repeat` + `padding-left` indentation per level. Expand/collapse arrows on parent nodes. "All Settings" root shows everything. Blue dot indicator on nodes with modified descendants.
- **Right panel (600px+ flex):** Property list grouped by full `GroupName` path (with ` > ` separator). Uses existing `va-generic-value-input`, `ui-select`, and dictionary editor. Blue dot indicator next to modified property labels. Reset-to-default button on modified properties.
- **Blade sizing:** Default width 960px. Maximizable up to 1300px.

### Filter UI — Reusable `<va-filter-panel>` Directive

The filter bar uses the platform's reusable `<va-filter-panel>` directive (`wwwroot/js/common/directives/filterPanel.js`). The directive renders the common shell — button with active badge, popup panel container, "Clear filters" link, and search input. Module-specific filter controls are injected via **AngularJS transclusion**.

```html
<va-filter-panel
    has-active-filters="filter.hasActiveFilters()"
    on-clear-filters="filter.clearFilters()"
    search-text="blade.searchText">
    <!-- Transcluded: settings-specific rows -->
    <div class="va-filter-panel__row">...</div>
</va-filter-panel>
```

**Directive scope bindings:**

| Binding | Type | Description |
|---------|------|-------------|
| `has-active-filters` | `&` (expression) | Consumer returns `true` when filters are active (drives badge + clear link) |
| `on-clear-filters` | `&` (callback) | Consumer resets its filter model |
| `search-text` | `=?` (two-way) | Keyword search binding |
| `hide-search` | `=?` (bool) | Hide the search input |
| `filter-title` | `@?` (string) | Tooltip for the filter button |

**Settings blade uses** these transcluded rows:
- **Modified Only** toggle (checkbox with switch style) — shows count of modified settings
- **Module** dropdown (sorted alphabetically) — filter by owning module

**Search input** filters by keyword across: setting Name, translated display name, GroupName, and **current value** (including stringified JSON values). When filters are active, tree nodes are automatically filtered to show only groups containing matching settings (plus their ancestors), and matching groups are auto-expanded.

**Other modules** (e.g., Notifications) can use the same directive with their own transcluded rows (status dropdown, date range, etc.) — no CSS or JS duplication needed.

### JSON Editor Blade

Controller: `platformWebApp.settingsJsonEditorController`

Opens as a child blade showing all modified settings in the Settings Document format. Uses CodeMirror (same as the platform's Json value type editor) with:

- Syntax highlighting, line numbers, code folding
- **Format JSON** button (top-right, also `Ctrl+Alt+F`)
- **JSON validation indicator** (red "Invalid JSON" / hidden when valid)
- Save/Reset toolbar commands with dirty checking

### Toolbar Commands

**API mode** (global settings):

| Command | Icon | Permission | Description |
|---------|------|-----------|-------------|
| Save | `fas fa-save` | `platform:setting:update` | POST only dirty entries as `{ name: value }` |
| Reset | `fa fa-undo` | -- | Revert to loaded values (re-fetches from API) |
| Export JSON | `fa fa-download` | -- | Download modified settings as document file |
| Import JSON | `fa fa-upload` | `platform:setting:update` | Upload JSON file, confirm, apply via POST |
| Edit as JSON | `fa fa-code` | -- | Open CodeMirror JSON editor child blade |
| *separator* | | | |
| Reset cache | `fa fa-eraser` | `cache:reset` | Clear platform cache |
| Restart | `fa fa-bolt` | `platform:module:manage` | Restart application |

**Entity mode** (store/entity settings):

| Command | Icon | Description |
|---------|------|-------------|
| Reset | `fa fa-undo` | Revert to loaded values |
| Export JSON | `fa fa-download` | Download modified settings |
| Import JSON | `fa fa-upload` | Upload and apply settings |
| Edit as JSON | `fa fa-code` | Open JSON editor |

In entity mode, **Save**, **Reset cache**, and **Restart** are hidden. The blade shows an **OK/Cancel** footer instead -- OK writes changes back to the parent entity's in-memory settings, Cancel closes without changes.

### Deep Linking & Copy Link

The settings blade supports URL-based deep linking via query parameters on the `workspace.settings` state:

```
#!/workspace/settings?group=VirtoCommerce.Search%7CIndexing
#!/workspace/settings?setting=VirtoCommerce.Search.IndexingJobs.CronExpression
#!/workspace/settings?group=Catalog%7CGeneral&setting=Catalog.ImageCategories
```

| Parameter | Purpose |
|-----------|---------|
| `group` | Pipe-delimited GroupName — scrolls to section legend |
| `setting` | Setting Name — scrolls to specific property (hidden/programmatic, for use by external modules) |

On load, the controller reads `$stateParams`, waits for DOM render, then calls `scrollIntoView()` on the matching `data-group-name` or `data-setting-name` element with a 2-second yellow highlight animation.

**Copy link icon:** Each section legend shows a link icon on hover. Clicking it copies the absolute URL (with `?group=...`) to the clipboard via `navigator.clipboard.writeText()`. The icon uses `$state.href('workspace.settings', { group: groupName }, { absolute: true })` to generate the URL.

### Tenant Property Labels (Global Mode)

In global settings view, properties registered for tenant types via `RegisterSettingsForType()` display a gray pill badge (e.g., `Store`) next to the property label. The schema API returns `assignedToTenants` (e.g., `["Store"]`) for each property.

By default, tenant-assigned properties are **hidden** in global view. A "Show tenant properties" toggle in the filter popup reveals them. This prevents clutter from store-specific settings in the global settings list.

In entity mode, the toggle and badges are hidden (all properties are relevant to that entity).

### Dirty Tracking & Navigation Guard

- `isDirty()` compares each setting's current `values[0].value` against a snapshot taken at load time via `getSettingCurrentValue()`
- Save button is disabled when not dirty or form is invalid
- **API mode:** `blade.onClose` uses `showConfirmationIfNeeded` for standard blade close. `$transitions.onBefore` hook intercepts UI-Router state changes away from `workspace.settingsUnified` -- shows save/discard dialog. On discard, resets the dirty snapshot via `updateOrigSnapshot()` so re-opening doesn't trigger a stale dialog.
- **Entity mode:** `blade.onClose` skips confirmation (parent entity handles its own save flow). No navigation guard hook.

### Blade Reusability

The unified blade accepts parameters for different contexts:

| Parameter | Type | Purpose |
|-----------|------|---------|
| `blade.tenantType` | string | Short tenant type name (e.g., `"Store"`) |
| `blade.tenantId` | string | Tenant instance ID |
| `blade.settingNames` | string[] | Limit visible settings to specific names |
| `blade.isEntityMode` | bool | Use entity data source (in-memory parent entity) instead of API |

**Global settings** (main menu route):
```javascript
var blade = {
    id: 'settingsUnified',
    isClosingDisabled: true,
    controller: 'platformWebApp.settingsUnifiedController',
    template: '$(Platform)/Scripts/app/settings/blades/settings-unified.tpl.html'
};
```

**Store settings** (entity widget):
```javascript
// entitySettingsWidget.js extracts tenantType from the entity's
// fully-qualified objectType (or typeName as fallback):
// "VirtoCommerce.StoreModule.Core.Model.Store" -> "Store"
var blade = {
    id: 'entitySettingList',
    tenantType: getTenantType(),   // "Store"
    tenantId: getTenantId(),       // entity.id
    settingNames: getSettingNames(), // _.pluck(entity.settings, 'name')
    isEntityMode: true,
    isExpandable: true,
    controller: 'platformWebApp.settingsUnifiedController',
    template: '$(Platform)/Scripts/app/settings/blades/settings-unified.tpl.html'
};
```

The entity widget uses helper functions to safely extract blade parameters:

```javascript
function getEntity() {
    return blade.currentEntity || {};
}

function getTenantType() {
    var fullType = getEntity().objectType || getEntity().typeName;
    if (!fullType) { return undefined; }
    var parts = fullType.split('.');
    return parts[parts.length - 1];
}

function getTenantId() {
    return getEntity().id;
}

function getSettingNames() {
    var settings = getEntity().settings;
    return settings ? _.pluck(settings, 'name') : undefined;
}
```

---

## CSS Architecture

Styles are split across two SASS modules (compiled into the platform's main theme bundle):

**`_va-filter-panel.sass`** — Reusable filter directive styles (shared by all blades):

| Class prefix | Purpose |
|-------------|---------|
| `.va-filter-bar` | Flex container for filter + search |
| `.va-filter-wrap` | Relative positioning wrapper for button + panel |
| `.btn.__va-filter` | Filter toggle button with active state |
| `.va-filter-badge` | Blue dot indicator on button |
| `.va-filter-panel` | Popup panel (absolute, shadow, z-index) |
| `.va-filter-panel__row` | Flex row (label + control, border separator) |
| `.va-filter-panel__label` | Row label text |
| `.va-filter-panel__select` | Dropdown control |
| `.va-filter-panel__date-input` | Date input control |

**`_settings-unified.sass`** — Settings blade-specific styles:

| Class prefix | Purpose |
|-------------|---------|
| `.settings-unified-layout` | Flexbox two-panel container |
| `.settings-tree-panel` | Left tree panel (300px, scrollable) |
| `.settings-properties-panel` | Right properties panel (flex, 600px min, 18px legend font) |
| `.stree-*` | Tree node (14px font), arrow, text, modified dot |
| `.settings-property-modified` | Blue dot indicator on modified property labels |
| `.settings-property-tenant-badge` | Gray pill badge showing tenant type (e.g., "Store") |
| `.settings-copy-link` | Link icon on section legends (visible on hover) |
| `.__highlight` | Yellow fade animation for deep link scroll targets |
| `.__settings-unified` | Blade content width override (960px default, 1300px max when maximized) |

---

## Migration and Backward Compatibility

### What stays the same

- All existing `api/platform/settings` endpoints unchanged
- `settingGroup-list` and `settings-detail` blades remain functional
- `entitySettingListController` remains functional
- `ISettingsManager`, `SettingsManager`, and data layer unchanged
- Module setting declarations (`SettingDescriptor`) unchanged
- No database schema changes
- No breaking changes to any existing API or UI

### What is new (additive only)

- `api/platform/settings/v2` endpoints (6 new endpoints)
- `ISettingsPropertyService` / `SettingsPropertyService`
- `SettingPropertySchema` DTO
- `SettingsPropertySearchCriteria`
- Unified settings blade with Data Source abstraction (controller + template + JSON editor)
- Reusable `<va-filter-panel>` directive (`filterPanel.js` + `.tpl.html` + `_va-filter-panel.sass`)
- `_settings-unified.sass` styles (layout/tree/property only — filter styles in shared directive)
- `settingsV2.js` Angular resource
- New `workspace.settings` UI-Router state with `?group&setting` deep link params
- Updated `entitySettingsWidget.js` with tenant type extraction helpers
- `ISettingsRegistrar.GetSettingTenants()` for tenant assignment lookup
- `SettingPropertySchema.AssignedToTenants` field

### Migration path

1. V2 API and UI ship alongside V1 -- zero breaking changes
2. Main menu "Settings" points to unified blade; old route (`workspace.modulesSettings`) still accessible
3. Entity settings widget now opens the unified blade with `isEntityMode: true`
4. In a future major version, V1 blade and old endpoints can be deprecated

---

## Files Reference

### New Backend Files

| File | Location |
|------|----------|
| `SettingPropertySchema.cs` | `src/VirtoCommerce.Platform.Core/Settings/` |
| `SettingsPropertySearchCriteria.cs` | `src/VirtoCommerce.Platform.Core/Settings/` |
| `ISettingsPropertyService.cs` | `src/VirtoCommerce.Platform.Core/Settings/` |
| `SettingsPropertyService.cs` | `src/VirtoCommerce.Platform.Data/Settings/` |
| `SettingsV2Controller.cs` | `src/VirtoCommerce.Platform.Web/Controllers/Api/` |

### New Frontend Files

| File | Location |
|------|----------|
| `filterPanel.js` | `wwwroot/js/common/directives/` (reusable directive) |
| `filterPanel.tpl.html` | `wwwroot/js/common/directives/` (directive template) |
| `_va-filter-panel.sass` | `wwwroot/css/themes/main/sass/modules/` (shared filter styles) |
| `settingsV2.js` | `wwwroot/js/app/settings/resources/` |
| `settings-unified.js` | `wwwroot/js/app/settings/blades/` |
| `settings-unified.tpl.html` | `wwwroot/js/app/settings/blades/` |
| `settings-json-editor.js` | `wwwroot/js/app/settings/blades/` |
| `settings-json-editor.tpl.html` | `wwwroot/js/app/settings/blades/` |
| `_settings-unified.sass` | `wwwroot/css/themes/main/sass/modules/` |

### Modified Files

| File | Change |
|------|--------|
| `ServiceCollectionExtensions.cs` | Register `ISettingsPropertyService` -> `SettingsPropertyService` |
| `ISettingsRegistrar.cs` | Add `GetSettingTenants()` method |
| `SettingsManager.cs` | Implement `GetSettingTenants()` |
| `settings.js` | Add `workspace.settings` route with `?group&setting` params, register scripts, toolbar commands, update main menu |
| `entitySettingsWidget.js` | Use unified blade with `isEntityMode`, tenant type extraction helpers |
| `main.sass` | Import `_va-filter-panel` before `_settings-unified` |
