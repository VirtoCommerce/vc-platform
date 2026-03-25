# `<va-filter-panel>` Directive — Developer Guide

## Overview

`<va-filter-panel>` is a reusable AngularJS directive that provides a consistent filter UI across all Virto Commerce blades. It renders a filter toggle button with an active badge, a popup panel for filter controls, and an optional search input — all with built-in outside-click-to-close behavior.

Module-specific filter controls (checkboxes, dropdowns, date pickers, etc.) are injected via **transclusion**, so the directive handles the common shell while each blade defines its own filter fields.

## Quick Start

### 1. Add to your blade template

```html
<div class="blade-static">
    <div class="form-group" style="display: flex; align-items: center;">
        <va-filter-panel
            has-active-filters="filter.hasActiveFilters()"
            on-clear-filters="filter.clearFilters()"
            search-text="filter.keyword">
            <!-- Your filter rows go here (transcluded) -->
            <div class="va-filter-panel__row">
                <span class="va-filter-panel__label">Status</span>
                <select class="va-filter-panel__select"
                        ng-model="filter.status"
                        ng-change="filter.criteriaChanged()">
                    <option value="">All</option>
                    <option value="Active">Active</option>
                    <option value="Inactive">Inactive</option>
                </select>
            </div>
        </va-filter-panel>
    </div>
</div>
```

### 2. Define filter state in your controller

```javascript
$scope.filter = {
    keyword: '',
    status: '',
    hasActiveFilters: function () {
        return this.status !== '';
    },
    clearFilters: function () {
        this.status = '';
        this.criteriaChanged();
    },
    criteriaChanged: function () {
        blade.refresh();
    }
};
```

That's it. The directive handles panel toggle, badge indicator, outside-click-to-close, search input, and the "Clear filters" link. You only write the filter-specific logic.

## API Reference

### Scope Bindings

| Binding | Type | Required | Description |
|---------|------|----------|-------------|
| `has-active-filters` | `&` (expression) | Yes | Expression returning `true` when any filter is active. Drives the badge dot and "Clear filters" link visibility. |
| `on-clear-filters` | `&` (callback) | Yes | Called when user clicks "Clear filters". Reset your filter model here. |
| `search-text` | `=?` (two-way) | No | Two-way binding for the search keyword. Omit if you handle search differently. |
| `hide-search` | `=?` (bool) | No | Set to `true` to hide the built-in search input. Default: `false`. |
| `search-placeholder` | `@?` (string) | No | Custom placeholder text for the search input. Default: localized "Search keyword..." |
| `filter-title` | `@?` (string) | No | Tooltip text for the filter button. |

### Transclusion

Content placed inside `<va-filter-panel>...</va-filter-panel>` is transcluded into the popup panel body, above the "Clear filters" link. The transcluded content inherits **your controller's scope** (standard AngularJS transclusion behavior), so you can use `filter.myProp`, `blade.myData`, etc. directly — no `$parent` needed.

### Built-In Behavior

| Feature | Description |
|---------|-------------|
| **Panel toggle** | Click the filter button to open/close the popup |
| **Active badge** | Blue dot appears on the button when `hasActiveFilters()` returns `true` |
| **Outside click** | Clicking anywhere outside the panel closes it (via `$document` event listener, cleaned up on `$destroy`) |
| **Clear filters** | "Clear filters" link appears when filters are active; calls `onClearFilters()` and closes the panel |
| **Search input** | Built-in search input with clear button, bound to `searchText` |

## CSS Classes

All styles live in `_va-filter-panel.sass` (imported in the platform theme). Use these classes in your transcluded rows:

| Class | Usage |
|-------|-------|
| `.va-filter-panel__row` | Wrap each filter control row |
| `.va-filter-panel__label` | Label text inside a row |
| `.va-filter-panel__select` | Styled `<select>` dropdown |
| `.va-filter-panel__date-input` | Styled date `<input>` |

For boolean toggles, use the platform's existing switch pattern:

```html
<label class="form-label __switch" style="margin: 0;">
    <input type="checkbox" ng-model="filter.myToggle" ng-change="filter.criteriaChanged()">
    <span class="switch"></span>
</label>
```

## Examples

### Minimal — Single Dropdown Filter

```html
<va-filter-panel
    has-active-filters="filter.hasActiveFilters()"
    on-clear-filters="filter.clearFilters()"
    search-text="blade.searchText">
    <div class="va-filter-panel__row">
        <span class="va-filter-panel__label">Category</span>
        <select class="va-filter-panel__select"
                ng-model="filter.category"
                ng-change="filter.criteriaChanged()">
            <option value="">All</option>
            <option ng-repeat="c in blade.categories" value="{{c.id}}">{{c.name}}</option>
        </select>
    </div>
</va-filter-panel>
```

```javascript
$scope.filter = {
    category: '',
    hasActiveFilters: function () { return this.category !== ''; },
    clearFilters: function () { this.category = ''; applyFilters(); },
    criteriaChanged: function () { applyFilters(); }
};
```

### Settings Blade — Modified Only + Module Filter

This is how the platform's unified settings blade uses the directive:

```html
<va-filter-panel
    has-active-filters="filter.hasActiveFilters()"
    on-clear-filters="filter.clearFilters()"
    search-text="blade.searchText"
    filter-title="{{ 'platform.blades.settings-unified.filter' | translate }}">
    <div class="va-filter-panel__row">
        <span class="va-filter-panel__label">Show modified only ({{blade.modifiedCount}})</span>
        <label class="form-label __switch" style="margin: 0;">
            <input type="checkbox" ng-model="filter.modifiedOnly" ng-change="filter.criteriaChanged()">
            <span class="switch"></span>
        </label>
    </div>
    <div class="va-filter-panel__row">
        <span class="va-filter-panel__label">Module</span>
        <select class="va-filter-panel__select"
                ng-model="filter.moduleId"
                ng-change="filter.criteriaChanged()">
            <option value="">All Modules</option>
            <option ng-repeat="m in blade.modules" value="{{m.id}}">{{m.id}}</option>
        </select>
    </div>
</va-filter-panel>
```

```javascript
$scope.filter = {
    modifiedOnly: false,
    moduleId: '',
    hasActiveFilters: function () {
        return this.modifiedOnly || this.moduleId !== '';
    },
    clearFilters: function () {
        this.modifiedOnly = false;
        this.moduleId = '';
        applyFilters();
    },
    criteriaChanged: function () {
        applyFilters();
    }
};
```

### Notification Journal — Status + Date Range + Custom Dates

This example shows how the Notifications module can migrate from the current `.journal-filter-*` implementation to `<va-filter-panel>`:

**Template** (`notifications-journal.tpl.html`):

```html
<div class="blade-static">
    <div class="form-group" style="display: flex; align-items: center;">
        <va-filter-panel
            has-active-filters="filter.hasActiveFilters()"
            on-clear-filters="filter.clearFilters()"
            search-text="filter.keyword"
            filter-title="{{ 'notifications.blades.notifications-journal.labels.filter-button-hint' | translate }}">

            <!-- Search in body toggle -->
            <div class="va-filter-panel__row">
                <span class="va-filter-panel__label">
                    {{ 'notifications.blades.notifications-journal.labels.search-in-body' | translate }}
                </span>
                <label class="form-label __switch" style="margin: 0;">
                    <input type="checkbox"
                           ng-model="filter.searchInBody"
                           ng-change="filter.criteriaChanged()">
                    <span class="switch"></span>
                </label>
            </div>

            <!-- Status dropdown -->
            <div class="va-filter-panel__row">
                <span class="va-filter-panel__label">
                    {{ 'notifications.blades.notifications-journal.labels.filter-status' | translate }}
                </span>
                <select class="va-filter-panel__select"
                        ng-model="filter.status"
                        ng-change="filter.criteriaChanged()"
                        ng-options="s.value as (s.label | translate) for s in filter.statuses">
                </select>
            </div>

            <!-- Date range dropdown -->
            <div class="va-filter-panel__row">
                <span class="va-filter-panel__label">
                    {{ 'notifications.blades.notifications-journal.labels.filter-date' | translate }}
                </span>
                <select class="va-filter-panel__select"
                        ng-model="filter.dateRange"
                        ng-change="filter.dateRangeChanged()"
                        ng-options="r.value as (r.label | translate) for r in filter.dateRanges">
                </select>
            </div>

            <!-- Custom date range inputs -->
            <div ng-if="filter.showCustomInputs" class="va-filter-panel__row" style="flex-direction: column; align-items: stretch;">
                <div style="display: flex; justify-content: space-between; padding: 4px 0;">
                    <span class="va-filter-panel__label">
                        {{ 'notifications.blades.notifications-journal.labels.filter-from' | translate }}
                    </span>
                    <input type="date" class="va-filter-panel__date-input" ng-model="filter.customStartDate" />
                </div>
                <div style="display: flex; justify-content: space-between; padding: 4px 0;">
                    <span class="va-filter-panel__label">
                        {{ 'notifications.blades.notifications-journal.labels.filter-to' | translate }}
                    </span>
                    <input type="date" class="va-filter-panel__date-input" ng-model="filter.customEndDate" />
                </div>
                <button class="btn __primary" style="width: 100%; margin-top: 8px;"
                        ng-click="filter.applyCustomRange()"
                        ng-disabled="!filter.customStartDate && !filter.customEndDate">
                    {{ 'notifications.blades.notifications-journal.labels.filter-apply' | translate }}
                </button>
            </div>

            <!-- Applied custom range display -->
            <div ng-if="filter.customRangeApplied && !filter.showCustomInputs" class="va-filter-panel__row">
                <span class="va-filter-panel__label">
                    {{filter.formatDate(filter.startDate)}} &mdash; {{filter.formatDate(filter.endDate)}}
                </span>
                <a href="" style="color: #1a73e8; font-size: 12px;" ng-click="filter.editCustomRange()">
                    {{ 'notifications.blades.notifications-journal.labels.filter-edit' | translate }}
                </a>
            </div>

        </va-filter-panel>
    </div>
</div>
```

**Controller** (`notifications-journal.js`) — simplified filter object (remove `showPanel`, `togglePanel`, and outside-click handler):

```javascript
var filter = $scope.filter = {};
filter.searchInBody = false;
filter.keyword = '';
filter.status = null;
filter.dateRange = 'last24h';
filter.startDate = new Date(new Date().getTime() - 24 * 60 * 60 * 1000);
filter.endDate = null;
filter.customStartDate = null;
filter.customEndDate = null;
filter.customRangeApplied = false;
filter.showCustomInputs = false;

filter.statuses = [
    { value: null, label: 'notifications.blades.notifications-journal.labels.filter-status-all' },
    { value: 'Sent', label: 'notifications.blades.notifications-journal.labels.success' },
    { value: 'Pending', label: 'notifications.blades.notifications-journal.labels.processing' },
    { value: 'Error', label: 'notifications.blades.notifications-journal.labels.error' }
];

filter.dateRanges = [
    { value: null, label: 'notifications.blades.notifications-journal.labels.filter-date-any' },
    { value: 'today', label: 'notifications.blades.notifications-journal.labels.filter-date-today' },
    { value: 'last24h', label: 'notifications.blades.notifications-journal.labels.filter-date-last24h' },
    { value: 'last7d', label: 'notifications.blades.notifications-journal.labels.filter-date-last7d' },
    { value: 'last30d', label: 'notifications.blades.notifications-journal.labels.filter-date-last30d' },
    { value: 'custom', label: 'notifications.blades.notifications-journal.labels.filter-date-custom' }
];

filter.hasActiveFilters = function () {
    return filter.searchInBody || filter.startDate || filter.endDate || filter.status;
};

filter.clearFilters = function () {
    filter.searchInBody = false;
    filter.startDate = null;
    filter.endDate = null;
    filter.status = null;
    filter.dateRange = null;
    filter.customStartDate = null;
    filter.customEndDate = null;
    filter.customRangeApplied = false;
    filter.showCustomInputs = false;
    filter.criteriaChanged();
};

filter.criteriaChanged = function () {
    if ($scope.pageSettings.currentPage > 1) {
        $scope.pageSettings.currentPage = 1;
    } else {
        blade.refresh();
    }
};

// dateRangeChanged(), applyCustomRange(), editCustomRange(), formatDate()
// remain unchanged — these are notification-specific logic, not panel logic.
```

**What to delete** after migration:

- Remove `.journal-filter-*` CSS from `Content/css/styles.css` (lines 1-157)
- Remove `filter.showPanel`, `filter.togglePanel` from the controller
- Remove the `angular.element(document).on('click', closePanel)` block from the controller

### Filter-Only (No Search)

```html
<va-filter-panel
    has-active-filters="filter.hasActiveFilters()"
    on-clear-filters="filter.clearFilters()"
    hide-search="true">
    <div class="va-filter-panel__row">
        <span class="va-filter-panel__label">Type</span>
        <select class="va-filter-panel__select" ng-model="filter.type" ng-change="filter.criteriaChanged()">
            <option value="">All</option>
            <option value="typeA">Type A</option>
        </select>
    </div>
</va-filter-panel>
```

## Migration Checklist

When migrating an existing blade to `<va-filter-panel>`:

1. **Template**: Replace inline filter button/panel/search HTML with `<va-filter-panel>` + transcluded rows
2. **CSS classes**: Change `your-prefix-filter-panel__row` to `va-filter-panel__row`, `__label` to `va-filter-panel__label`, `__select` to `va-filter-panel__select`, etc.
3. **Controller**: Remove `filter.showPanel`, `filter.togglePanel`, and the `document.addEventListener('click', ...)` / `$destroy` cleanup block
4. **CSS file**: Delete duplicate `.your-prefix-filter-*` styles — the directive's `_va-filter-panel.sass` provides them
5. **Keep**: All filter-specific logic (`hasActiveFilters`, `clearFilters`, `criteriaChanged`, custom date handling, etc.)

## Files

| File | Location |
|------|----------|
| `filterPanel.js` | `wwwroot/js/common/directives/` |
| `filterPanel.tpl.html` | `wwwroot/js/common/directives/` |
| `_va-filter-panel.sass` | `wwwroot/css/themes/main/sass/modules/` |
