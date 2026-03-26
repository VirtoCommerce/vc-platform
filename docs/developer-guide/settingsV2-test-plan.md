# Settings V2 — Test Plan

## 1. REST API

### Schema API
- [ ] `GET /global/schema` returns flat array of property descriptors (no tenant-assigned settings)
- [ ] `GET /global/schema?moduleId=VirtoCommerce.Search` returns only Search module settings
- [ ] `GET /global/schema?keyword=cron` returns only settings matching keyword in Name/DisplayName/GroupName
- [ ] `GET /tenant/Store/schema` returns only settings registered for Store type
- [ ] Tenant schema excludes hidden settings (`IsHidden=true`)
- [ ] `IsReadOnly=true` on settings forced by `appsettings.json` override
- [ ] `AssignedToTenants` populated (e.g., `["Store"]`) for tenant-registered settings

### Values API
- [ ] `GET /global/values` returns all global values as `{ "Name": value }` dictionary
- [ ] `GET /global/values?modifiedOnly=true` returns only values differing from default
- [ ] `GET /tenant/Store/Electronics/values` returns store-scoped values
- [ ] Dictionary keys preserve original casing (no camelCase lowering)
- [ ] Tenant values exclude hidden settings

### Save API — Merge Mode (default)
- [ ] `POST /global/values` with `{ "SettingName": newValue }` persists the value
- [ ] Only specified settings are updated (other settings untouched)
- [ ] Unknown setting name returns error
- [ ] Saving tenant-unregistered setting to tenant scope returns error
- [ ] Empty/null body without `replaceAll` returns 400 Bad Request
- [ ] Value type conversion works: boolean, integer, decimal, string, JSON

### Save API — Replace Mode (`replaceAll=true`)
- [ ] `POST /global/values?replaceAll=true` with partial dict — keeps only listed modifications, resets absent ones
- [ ] `POST /global/values?replaceAll=true` with `{}` — resets ALL modified settings to defaults
- [ ] After replaceAll reset, `GET /values?modifiedOnly=true` returns empty dict for reset settings
- [ ] `POST /tenant/Store/Electronics/values?replaceAll=true` works for tenant scope
- [ ] Empty body with `replaceAll=true` does NOT return 400 (allowed)

## 2. Unified Settings Blade — Global Mode

### Layout
- [ ] Two-panel layout: tree (300px left) + properties (right)
- [ ] Default width 960px, maximizable to 1300px
- [ ] "All Settings" root node selected by default, all properties visible

### Tree Navigation
- [ ] Tree built from GroupName split on `|` with correct hierarchy
- [ ] Expand/collapse arrows on parent nodes
- [ ] Clicking a group shows only that group's properties in right panel
- [ ] Clicking "All Settings" shows all properties
- [ ] Blue dot on tree nodes with modified descendants

### Property Display
- [ ] Properties render using existing `va-generic-value-input` (text, boolean, dropdown, etc.)
- [ ] Dictionary settings show pencil icon to edit values
- [ ] Blue dot indicator on modified property labels
- [ ] Reset-to-default button appears on modified properties
- [ ] Question mark icon toggles description

### Filter & Search
- [ ] Filter button opens popup panel
- [ ] "Modified Only" toggle filters to changed settings + updates count badge
- [ ] "Module" dropdown filters by module (sorted alphabetically)
- [ ] "Clear filters" link resets all filters
- [ ] Filter badge (blue dot) appears on button when filters active
- [ ] Outside click closes filter panel
- [ ] Search input filters by: Name, translated name, GroupName, current value
- [ ] Search filters tree nodes (only matching groups + ancestors shown)
- [ ] Tree auto-expands when filters active

### Tenant Properties (Global Mode)
- [ ] Tenant-assigned settings excluded from global view by backend
- [ ] (If filter enabled) "Show tenant properties" toggle reveals them with gray badge

## 3. Dirty Tracking & Save

- [ ] Save button disabled when no changes
- [ ] Editing a property enables Save button
- [ ] Reset-to-default on a property enables Save button
- [ ] Save sends only modified settings (check Network tab — not all settings)
- [ ] After save, Save button disabled again
- [ ] Reset toolbar button reverts to loaded values
- [ ] Navigating away with unsaved changes shows confirmation dialog
- [ ] "Yes" saves and navigates, "No" discards and navigates
- [ ] Returning to settings after discard does NOT show stale dialog

## 4. JSON Export / Import

### Export
- [ ] "Export JSON" downloads `.json` file
- [ ] File contains only modified values in document envelope (`version`, `exportedAt`, `scope`, `settings`)
- [ ] File name includes scope (e.g., `settings-global.json`)

### Import (uses `replaceAll=true`)
- [ ] "Import JSON" opens file picker (`.json` only)
- [ ] Confirmation dialog shows count of settings to apply
- [ ] After confirm, settings applied via `POST /values?replaceAll=true` and blade refreshes
- [ ] Settings in the file are set, settings NOT in the file are reset to default
- [ ] Importing empty `settings: {}` resets all modified settings to defaults
- [ ] Invalid JSON file shows error notification
- [ ] Import error does not leave blade in loading state

### Edit as JSON (uses `replaceAll=true`)
- [ ] "Edit as JSON" opens child blade with CodeMirror editor
- [ ] Editor shows modified settings in document format
- [ ] "Format JSON" button formats content
- [ ] Invalid JSON shows red indicator
- [ ] Save applies changes via `POST /values?replaceAll=true` and refreshes parent blade
- [ ] Removing a property from JSON and saving resets it to default
- [ ] Clearing all settings to `{}` and saving resets everything to defaults
- [ ] Dirty tracking works (Save disabled when unchanged)
- [ ] Closing with unsaved changes shows confirmation

## 5. Deep Linking & Copy Link

- [ ] `#!/workspace/settings?group=VirtoCommerce.Search%7CIndexing` scrolls to that section
- [ ] `#!/workspace/settings?setting=VirtoCommerce.Search.IndexingJobs.CronExpression` scrolls to that property
- [ ] Target element gets yellow highlight animation (2 seconds)
- [ ] Copy link icon appears on hover over section legends
- [ ] Clicking copy icon copies absolute URL to clipboard
- [ ] Copy link icon hidden in entity mode
- [ ] Deep link works on full page refresh

## 6. Entity Mode (Store Settings)

### Data Source
- [ ] Schema loaded from API (`GET /tenant/Store/schema`)
- [ ] Values loaded from parent entity in-memory (no API call)
- [ ] Save writes back to parent entity in-memory (no API call)

### UI Differences
- [ ] OK/Cancel footer shown (not Save toolbar button)
- [ ] OK saves changes to parent entity and closes blade
- [ ] Cancel closes without changes
- [ ] Restart and Reset Cache buttons hidden
- [ ] Copy link icon hidden
- [ ] Tenant filter toggle hidden
- [ ] No navigation guard (parent handles save flow)

### Integration
- [ ] Opening store settings widget opens unified blade with correct `tenantType`/`tenantId`
- [ ] `settingNames` limits visible settings to store-registered ones
- [ ] Parent entity settings updated after OK click

## 7. Reusable Filter Panel (`<va-filter-panel>`)

- [ ] Filter button shows/hides popup
- [ ] Active filter badge appears when `hasActiveFilters()` returns true
- [ ] "Clear filters" link visible when filters active, calls `onClearFilters()`
- [ ] Search input bound to `searchText` (two-way)
- [ ] `hideSearch=true` hides search input
- [ ] Outside click closes panel
- [ ] Transcluded content renders inside panel
- [ ] Panel cleaned up on scope destroy (no event listener leaks)

## 8. Toolbar Commands

### Global Mode
- [ ] Save — enabled when dirty, disabled otherwise
- [ ] Reset — enabled when dirty, refreshes from API
- [ ] Export JSON — downloads modified settings
- [ ] Import JSON — uploads and applies
- [ ] Edit as JSON — opens editor blade
- [ ] (separator)
- [ ] Reset Cache — shows confirmation, clears cache
- [ ] Restart — shows confirmation, restarts application

### Entity Mode
- [ ] Reset — enabled when dirty
- [ ] Export JSON — works
- [ ] Import JSON — works
- [ ] Edit as JSON — works
- [ ] Save, Reset Cache, Restart — NOT shown

## 9. Backward Compatibility

- [ ] Old `api/platform/settings` endpoints still work (unchanged)
- [ ] Old settings blade accessible via `#!/workspace/modulesSettings` (if route kept)
- [ ] Existing store/payment entity settings blades work unchanged
- [ ] Module setting declarations (`SettingDescriptor`) require no changes
- [ ] No database schema changes required
