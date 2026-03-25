# Settings V2 API and Unified Settings UI

## Overview

Settings V2 introduces a modernized settings experience inspired by Visual Studio 2026 Options dialog:

- **Unified Settings Blade** -- single wide blade with left tree navigation + right properties panel
- **Clean REST API** -- separates schema from values, returns only modified values
- **Global / Tenant scoping** -- URL path-based scope (`/global/` or `/tenant/{type}/{id}/`)
- **JSON import/export** -- save and load settings as JSON document files
- **Inline JSON editor** -- edit all settings as raw JSON with CodeMirror (syntax highlighting, folding, validation)
- **Reusable blade** -- same controller works for global settings, store settings, payment settings, and custom entities
- **Filter & search** -- filter popup (modified only, by module) + keyword search across names, values, and groups
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

### Client-Side Group Tree

The schema response is a **flat array** of property descriptors. The client builds the navigation tree by splitting each setting's `GroupName` on `|`.

Example: `GroupName: "Catalog|General"` produces:
```
Catalog
  General
```

The tree is rendered as a flat list with indentation (no recursive `ng-include`), using `padding-left` per depth level. An "All Settings" root node is prepended. When filters are active, only tree nodes containing matching settings (and their ancestors) are shown.

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
    "restartRequired": false
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
    "restartRequired": true
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
    "restartRequired": false
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

**Value Type Conversion:** The service automatically converts incoming JSON values to the correct .NET type based on the setting's `ValueType`. Newtonsoft `JToken` and System.Text.Json `JsonElement` are both handled.

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
- `SaveValuesAsync` -- resolves each dictionary key to a registered `SettingDescriptor` (throws on unknown names). Converts values via `ConvertValue()` which handles Newtonsoft `JToken` and System.Text.Json `JsonElement` unwrapping. Creates `ObjectSettingEntry` objects and calls `SaveObjectSettingsAsync`.

**`SettingsV2Controller`** (`VirtoCommerce.Platform.Web.Controllers.Api`)

REST controller at `api/platform/settings/v2` with 6 endpoints (3 global + 3 tenant). Uses a custom `JsonSerializerSettings` with `DefaultNamingStrategy` for value responses to preserve dictionary key casing.

### Relation to Existing API

The existing `SettingController` at `api/platform/settings` is **completely unchanged**. Both APIs coexist and share the same `ISettingsManager` data layer.

---

## Frontend Architecture

### Unified Settings Blade

Controller: `platformWebApp.settingsUnifiedController`

The blade loads data in two parallel API calls (`getSchema` + `getValues`), then merges schema and values client-side into `ObjectSettingEntry`-compatible objects that the existing `va-generic-value-input` directive can render without changes.

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

### Filter UI

The filter follows the notification journal filter pattern (`.settings-filter-*` CSS classes):

```
[Filter button] [Search keyword input]
```

- **Filter button** opens a popup panel with:
  - **Modified Only** toggle (checkbox with switch style) -- shows count of modified settings
  - **Module** dropdown -- filter by owning module
  - **Clear filters** link
  - Active filter badge indicator (blue dot) on button
- **Search input** filters by keyword across: setting Name, translated display name, GroupName, and **current value** (including stringified JSON values)
- When filters are active, tree nodes are automatically filtered to show only groups containing matching settings (plus their ancestors), and matching groups are auto-expanded

### JSON Editor Blade

Controller: `platformWebApp.settingsJsonEditorController`

Opens as a child blade showing all modified settings in the Settings Document format. Uses CodeMirror (same as the platform's Json value type editor) with:

- Syntax highlighting, line numbers, code folding
- **Format JSON** button (top-right, also `Ctrl+Alt+F`)
- **JSON validation indicator** (red "Invalid JSON" / hidden when valid)
- Save/Reset toolbar commands with dirty checking

### Toolbar Commands

| Command | Icon | Permission | Description |
|---------|------|-----------|-------------|
| Save | `fas fa-save` | `platform:setting:update` | POST only dirty entries as `{ name: value }` |
| Reset | `fa fa-undo` | -- | Revert to loaded values (re-fetches from API) |
| Export JSON | `fa fa-download` | -- | Download modified settings as document file |
| Import JSON | `fa fa-upload` | `platform:setting:update` | Upload JSON file, confirm, apply via POST |
| Edit as JSON | `fa fa-code` | -- | Open CodeMirror JSON editor child blade |
| *delimiter* | | | |
| Restart | `fa fa-bolt` | `platform:module:manage` | Restart application |
| Reset cache | `fa fa-eraser` | `cache:reset` | Clear platform cache |

### Dirty Tracking & Navigation Guard

- `isDirty()` compares each setting's current `values[0].value` against a snapshot taken at load time
- Save button is disabled when not dirty or form is invalid
- `blade.onClose` uses `showConfirmationIfNeeded` for standard blade close
- `$transitions.onBefore` hook intercepts UI-Router state changes away from `workspace.settingsUnified` -- shows save/discard dialog. On discard, resets the dirty snapshot so re-opening doesn't trigger a stale dialog.

### Blade Reusability

The unified blade accepts parameters for different contexts:

| Parameter | Type | Purpose |
|-----------|------|---------|
| `blade.tenantType` | string | Tenant scope type (e.g., `"Store"`) |
| `blade.tenantId` | string | Tenant instance ID |
| `blade.settingNames` | string[] | Limit visible settings to specific names |
| `blade.isSavingToParentObject` | bool | Show OK/Cancel footer, save to parent entity |

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
var blade = {
    id: 'entitySettingList',
    tenantType: 'Store',
    tenantId: storeId,
    settingNames: _.pluck(entity.settings, 'name'),
    isSavingToParentObject: true,
    controller: 'platformWebApp.settingsUnifiedController',
    template: '$(Platform)/Scripts/app/settings/blades/settings-unified.tpl.html'
};
```

---

## CSS Architecture

Styles are in `_settings-unified.sass` (compiled into the platform's main theme bundle):

| Class prefix | Purpose |
|-------------|---------|
| `.settings-filter-*` | Filter button, badge, popup panel (mirrors `.journal-filter-*` from notification module) |
| `.settings-unified-layout` | Flexbox two-panel container |
| `.settings-tree-panel` | Left tree panel (300px, scrollable) |
| `.settings-properties-panel` | Right properties panel (flex, 600px min, 18px legend font) |
| `.stree-*` | Tree node, arrow, text, modified dot |
| `.settings-property-modified` | Blue dot indicator on modified property labels |
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
- Unified settings blade (controller + template + JSON editor)
- `_settings-unified.sass` styles
- `settingsV2.js` Angular resource
- New `workspace.settingsUnified` UI-Router state

### Migration path

1. V2 API and UI ship alongside V1 -- zero breaking changes
2. Main menu "Settings" points to unified blade; old route (`workspace.modulesSettings`) still accessible
3. Entity settings widgets can opt-in to new blade by changing controller/template reference
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
| `settings.js` | Add `workspace.settingsUnified` route, register scripts, toolbar commands, update main menu |
| `_module.sass` | Import `_settings-unified` |
