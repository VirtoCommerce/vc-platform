angular.module('platformWebApp')
    .controller('platformWebApp.settingsUnifiedController', [
        '$scope', '$q', '$translate', '$transitions', 'platformWebApp.settingsV2', 'platformWebApp.settings.helper',
        'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.modulesApi',
        'platformWebApp.changeLogApi', 'platformWebApp.WaitForRestart', '$timeout',
        function ($scope, $q, $translate, $transitions, settingsV2, settingsHelper, bladeNavigationService, dialogService, modulesApi, changeLogApi, waitForRestart, $timeout) {
            var blade = $scope.blade;
            blade.updatePermission = 'platform:setting:update';
            blade.headIcon = 'fa fa-wrench';
            blade.isExpandable = false;

            // Filter state (mirrors notification journal filter pattern)
            $scope.filter = {
                showPanel: false,
                modifiedOnly: false,
                moduleId: '',
                togglePanel: function ($event) {
                    if ($event) {
                        $event.stopPropagation();
                    }
                    this.showPanel = !this.showPanel;
                },
                hasActiveFilters: function () {
                    return this.modifiedOnly || this.moduleId !== '';
                },
                clearFilters: function () {
                    this.modifiedOnly = false;
                    this.moduleId = '';
                    this.showPanel = false;
                    applyFilters();
                },
                criteriaChanged: function () {
                    applyFilters();
                }
            };

            // Close filter panel on outside click
            var closeFilterPanel = function () {
                if ($scope.filter.showPanel) {
                    $scope.$apply(function () { $scope.filter.showPanel = false; });
                }
            };
            document.addEventListener('click', closeFilterPanel);
            $scope.$on('$destroy', function () {
                document.removeEventListener('click', closeFilterPanel);
            });

            blade.searchText = '';
            blade.treeNodes = [];
            blade.selectedGroup = null;
            blade.allSchemas = [];
            blade.allValues = {};
            blade.origValues = {};
            blade.mergedSettings = [];
            blade.filteredSettings = {};
            blade.visibleGroupNames = [];
            blade.modules = [];
            blade.modifiedCount = 0;

            blade.refresh = function () {
                blade.isLoading = true;

                var schemaPromise;
                var valuesPromise;

                if (blade.tenantType && blade.tenantId) {
                    schemaPromise = settingsV2.getTenantSchema({ tenantType: blade.tenantType, tenantId: blade.tenantId }).$promise;
                    valuesPromise = settingsV2.getTenantValues({ tenantType: blade.tenantType, tenantId: blade.tenantId }).$promise;
                } else {
                    schemaPromise = settingsV2.getGlobalSchema({}).$promise;
                    valuesPromise = settingsV2.getGlobalValues({}).$promise;
                }

                $q.all([schemaPromise, valuesPromise]).then(function (results) {
                    var schemas = results[0];
                    var values = results[1];

                    // Filter by settingNames if provided (entity settings mode)
                    if (blade.settingNames && blade.settingNames.length) {
                        var nameSet = {};
                        _.each(blade.settingNames, function (n) { nameSet[n] = true; });
                        schemas = _.filter(schemas, function (s) { return nameSet[s.name]; });
                    }

                    blade.allSchemas = schemas;
                    blade.allValues = angular.copy(values);
                    blade.origValues = angular.copy(values);

                    // Extract unique modules for filter dropdown
                    blade.modules = _.chain(schemas).pluck('moduleId').compact().uniq().map(function (id) {
                        return { id: id };
                    }).value();

                    // Merge schema + values into ObjectSettingEntry-compatible objects
                    blade.mergedSettings = mergeSchemaAndValues(schemas, values);

                    // Snapshot the initial values for dirty checking (after settingsHelper transform)
                    blade.origSettingValues = {};
                    _.each(blade.mergedSettings, function (s) {
                        blade.origSettingValues[s.name] = getSettingCurrentValue(s);
                    });

                    // Translate display names
                    _.each(blade.mergedSettings, function (setting) {
                        var translateKey = `settings.${setting.name}.title`;
                        var result = $translate.instant(translateKey);
                        setting.translatedName = result !== translateKey ? result : (setting.displayName || setting.name);
                    });

                    // Build flat tree for simple ng-repeat rendering
                    buildFlatTree(blade.mergedSettings);

                    // Compute modified count
                    updateModifiedCount();

                    applyFilters();
                    blade.isLoading = false;
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
            };

            // Helper to extract the current value from a merged setting object
            function getSettingCurrentValue(setting) {
                if (setting.values && setting.values.length) {
                    return angular.copy(setting.values[0].value);
                }
                return angular.copy(setting.value);
            }

            function mergeSchemaAndValues(schemas, values) {
                return _.map(schemas, function (schema) {
                    var value = values[schema.name];
                    if (value === undefined) {
                        value = schema.defaultValue;
                    }

                    // Build ObjectSettingEntry-compatible object for va-generic-value-input
                    return {
                        name: schema.name,
                        displayName: schema.displayName,
                        groupName: schema.groupName,
                        moduleId: schema.moduleId,
                        valueType: schema.valueType, // PascalCase required: directive builds template name as 'd' + valueType + '.html'
                        defaultValue: schema.defaultValue,
                        allowedValues: schema.allowedValues ? _.map(schema.allowedValues, function (v) { return { value: v }; }) : null,
                        isRequired: schema.isRequired,
                        isReadOnly: schema.isReadOnly,
                        isDictionary: schema.isDictionary,
                        isLocalizable: schema.isLocalizable,
                        restartRequired: schema.restartRequired,
                        value: value,
                        values: settingsHelper.getSettingValues({ isDictionary: schema.isDictionary, value: value })
                    };
                });
            }

            // Build a flat tree array from settings GroupName paths.
            // Uses depth-first traversal to produce correct parent-children ordering.
            // Each node has: id, name, groupName, level, hasChildren, expanded, parent.
            function buildFlatTree(settings) {
                var idCounter = 0;

                // Step 1: build a nested tree structure
                var rootChildren = {}; // name -> { name, groupName, childMap, ... }

                _.each(settings, function (setting) {
                    var paths = (setting.groupName || 'General').split('|');
                    var currentLevel = rootChildren;

                    _.each(paths, function (segment, i) {
                        if (!currentLevel[segment]) {
                            currentLevel[segment] = {
                                name: segment,
                                groupName: paths.slice(0, i + 1).join('|'),
                                childMap: {}
                            };
                        }
                        currentLevel = currentLevel[segment].childMap;
                    });
                });

                // Step 2: depth-first flatten with correct ordering
                var flatNodes = [];

                function flatten(childMap, level, parentGroupName) {
                    var sorted = _.sortBy(_.values(childMap), 'name');
                    _.each(sorted, function (entry) {
                        var hasChildren = _.keys(entry.childMap).length > 0;
                        idCounter = idCounter + 1;
                        var node = {
                            id: 'n' + idCounter,
                            name: entry.name,
                            groupName: entry.groupName,
                            level: level,
                            hasChildren: hasChildren,
                            expanded: level === 1, // top-level expanded by default
                            parent: parentGroupName
                        };
                        flatNodes.push(node);
                        if (hasChildren) {
                            flatten(entry.childMap, level + 1, entry.groupName);
                        }
                    });
                }

                flatten(rootChildren, 1, null);

                // Step 3: prepend "All Settings" root node
                var allNode = {
                    id: '__all__',
                    name: $translate.instant('platform.blades.settings-unified.all-settings') || 'All Settings',
                    groupName: null,
                    level: 0,
                    hasChildren: false,
                    expanded: true,
                    parent: null
                };

                blade.flatTree = [allNode].concat(flatNodes);
            }

            // Check if a node is visible (all ancestors expanded + passes filter)
            $scope.isNodeVisible = function (node) {
                // "All Settings" root node: always visible
                if (node.id === '__all__') {
                    return true;
                }

                // When filters are active, hide tree nodes that have no matching settings
                var isFiltering = blade.searchText || $scope.filter.modifiedOnly || $scope.filter.moduleId;
                if (isFiltering && blade.visibleNodeGroups && node.groupName) {
                    if (!blade.visibleNodeGroups[node.groupName]) {
                        return false;
                    }
                }

                if (node.level <= 1) {
                    return true; // top-level always visible (if it passed filter)
                }

                // Walk up parents to check all are expanded
                var parentGroupName = node.parent;
                while (parentGroupName) {
                    var parentNode = _.find(blade.flatTree, function (n) { return n.groupName === parentGroupName; });
                    if (!parentNode || !parentNode.expanded) {
                        return false;
                    }
                    parentGroupName = parentNode.parent;
                }
                return true;
            };

            // Select a group in the tree
            $scope.selectGroup = function (node) {
                if (node.groupName === null) {
                    // "All Settings" clicked
                    blade.selectedGroup = null;
                } else if (node.hasChildren && node === blade.selectedGroup) {
                    // Toggle expand/collapse if clicking already-selected parent
                    node.expanded = !node.expanded;
                } else {
                    blade.selectedGroup = node;
                    if (node.hasChildren) {
                        node.expanded = true;
                    }
                }
                applyFilters();
            };

            // Check if a setting's current value differs from default
            $scope.isModified = function (setting) {
                return !settingsHelper.isDefaultValue(setting);
            };

            // Check if a tree node has modified settings in its subtree
            $scope.nodeHasModified = function (node) {
                var nodeSettings = getSettingsForGroup(node.groupName);
                return _.any(nodeSettings, function (s) { return !settingsHelper.isDefaultValue(s); });
            };

            function getSettingsForGroup(groupName) {
                var groupPrefix = groupName + '|';
                return _.filter(blade.mergedSettings, function (s) {
                    return s.groupName === groupName ||
                        (s.groupName && s.groupName.indexOf(groupPrefix) === 0);
                });
            }

            function updateModifiedCount() {
                blade.modifiedCount = _.filter(blade.mergedSettings, function (s) {
                    return !settingsHelper.isDefaultValue(s);
                }).length;
            }

            // Apply all filters (search, mode, module, selected group)
            function applyFilters() {
                var settings = blade.mergedSettings;

                // Filter by module
                if ($scope.filter.moduleId) {
                    settings = _.filter(settings, function (s) { return s.moduleId === $scope.filter.moduleId; });
                }

                // Filter by modified only
                if ($scope.filter.modifiedOnly) {
                    settings = _.filter(settings, function (s) { return !settingsHelper.isDefaultValue(s); });
                }

                // Filter by search text (name, translated name, group, and current value)
                if (blade.searchText) {
                    var search = blade.searchText.toLowerCase();
                    settings = _.filter(settings, function (s) {
                        if ((s.name && s.name.toLowerCase().indexOf(search) >= 0) ||
                            (s.translatedName && s.translatedName.toLowerCase().indexOf(search) >= 0) ||
                            (s.groupName && s.groupName.toLowerCase().indexOf(search) >= 0)) {
                            return true;
                        }
                        // Search in current value
                        var val = (s.values && s.values.length) ? s.values[0].value : s.value;
                        if (val != null) {
                            var valStr = (typeof val === 'object') ? JSON.stringify(val) : String(val);
                            if (valStr.toLowerCase().indexOf(search) >= 0) {
                                return true;
                            }
                        }
                        return false;
                    });
                }

                // Build set of visible tree node groupNames from filtered settings.
                // Includes the setting's own group and all ancestor groups.
                var visibleNodeGroups = {};
                _.each(settings, function (s) {
                    var gn = s.groupName || 'General';
                    var segments = gn.split('|');
                    for (var i = 1; i <= segments.length; i++) {
                        visibleNodeGroups[segments.slice(0, i).join('|')] = true;
                    }
                });
                blade.visibleNodeGroups = visibleNodeGroups;

                // Auto-expand tree nodes when filtering is active
                var isFiltering = blade.searchText || $scope.filter.modifiedOnly || $scope.filter.moduleId;
                if (isFiltering) {
                    _.each(blade.flatTree, function (node) {
                        if (node.hasChildren && node.groupName && visibleNodeGroups[node.groupName]) {
                            node.expanded = true;
                        }
                    });
                }

                // Filter by selected group (unless searching/filtering or when no group selected show ALL)
                if (blade.selectedGroup && !blade.searchText) {
                    var selectedPrefix = blade.selectedGroup.groupName + '|';
                    settings = _.filter(settings, function (s) {
                        // Show settings for the selected group and its descendants
                        return s.groupName === blade.selectedGroup.groupName ||
                            (s.groupName && s.groupName.indexOf(selectedPrefix) === 0);
                    });
                }

                // Group settings for fieldset rendering.
                // When a specific group is selected, use last segment (parent context is clear from tree).
                // Otherwise use full path with " > " separator to preserve hierarchy.
                var showFullPath = !blade.selectedGroup || blade.searchText || isFiltering;
                var grouped = _.groupBy(settings, function (s) {
                    if (!s.groupName) {
                        return 'General';
                    }
                    if (showFullPath) {
                        return s.groupName.replace(/\|/g, ' > ');
                    }
                    var paths = s.groupName.split('|');
                    return paths[paths.length - 1];
                });

                blade.filteredSettings = grouped;
                blade.visibleGroupNames = _.keys(grouped).sort();
            }

            // Watch search text changes
            $scope.$watch('blade.searchText', function () {
                applyFilters();
            });

            $scope.$watch('filter.modifiedOnly', function () {
                applyFilters();
            });

            $scope.$watch('filter.moduleId', function () {
                applyFilters();
            });

            // Dirty checking
            var formScope;
            $scope.setForm = function (form) { formScope = form; };

            function isDirty() {
                if (!blade.origSettingValues) {
                    return false;
                }
                return _.any(blade.mergedSettings, function (s) {
                    var currentVal = (s.values && s.values.length) ? s.values[0].value : s.value;
                    var origVal = blade.origSettingValues[s.name];
                    return !angular.equals(currentVal, origVal);
                }) && blade.hasUpdatePermission();
            }

            function canSave() {
                return isDirty() && (!formScope || formScope.$valid);
            }

            // Helper to get the saveable value from a merged setting
            function getSaveableValue(setting) {
                if (setting.isDictionary) {
                    return (setting.values && setting.values[0]) ? setting.values[0].value.id : setting.value;
                }
                return (setting.values && setting.values[0]) ? setting.values[0].value : setting.value;
            }

            // Save changes
            blade.saveChanges = function () {
                blade.isLoading = true;

                var changedValues = {};
                _.each(blade.mergedSettings, function (s) {
                    var currentVal = getSaveableValue(s);
                    var origVal = blade.origSettingValues[s.name];

                    if (!angular.equals(currentVal, origVal)) {
                        changedValues[s.name] = currentVal;
                    }
                });

                if (Object.keys(changedValues).length === 0) {
                    blade.isLoading = false;
                    return $q.resolve();
                }

                var savePromise;
                if (blade.tenantType && blade.tenantId) {
                    savePromise = settingsV2.saveTenantValues(
                        { tenantType: blade.tenantType, tenantId: blade.tenantId },
                        changedValues).$promise;
                } else {
                    savePromise = settingsV2.saveGlobalValues({}, changedValues).$promise;
                }

                return savePromise.then(function () {
                    // Update the snapshot so isDirty() returns false after save
                    _.each(blade.mergedSettings, function (s) {
                        blade.origSettingValues[s.name] = getSettingCurrentValue(s);
                    });
                    updateModifiedCount();
                    blade.isLoading = false;
                });
            };

            // Reset to default
            // Note: we cannot use settingsHelper.resetToDefaultValue(blade.filteredSettings, setting)
            // directly because filteredSettings keys are display names ("Catalog > General" or "General"),
            // but the helper looks up by setting.groupName ("Catalog|General"). The lookup always
            // fails, falling back to angular.extend which doesn't trigger ngModel $render().
            // Instead, we build a temporary wrapper keyed by the raw groupName so the helper succeeds,
            // then sync the replaced object back into mergedSettings.
            $scope.resetToDefaultValue = function (setting) {
                // Find the display key for this setting in filteredSettings
                var displayKey = null;
                _.each(blade.filteredSettings, function (groupSettings, key) {
                    if (_.any(groupSettings, function (s) { return s.name === setting.name; })) {
                        displayKey = key;
                    }
                });

                if (displayKey !== null) {
                    // Build a temporary groups object keyed by the raw groupName
                    // so the helper's lookup (groups[setting.groupName]) succeeds.
                    var tempGroups = {};
                    tempGroups[setting.groupName] = blade.filteredSettings[displayKey];
                    settingsHelper.resetToDefaultValue(tempGroups, setting);
                    // The helper replaced the object in tempGroups[setting.groupName],
                    // which is the same array as blade.filteredSettings[displayKey].
                } else {
                    // Fallback: direct mutation
                    settingsHelper.resetToDefaultValue({}, setting);
                }

                // Sync replaced objects from filteredSettings into mergedSettings
                // so isDirty()/saveChanges() see the updated value.
                _.each(blade.filteredSettings, function (groupSettings) {
                    _.each(groupSettings, function (s) {
                        var idx = _.findIndex(blade.mergedSettings, function (m) { return m.name === s.name; });
                        if (idx >= 0 && blade.mergedSettings[idx] !== s) {
                            blade.mergedSettings[idx] = s;
                        }
                    });
                });

                updateModifiedCount();
            };

            $scope.isDefaultValue = function (setting) {
                return settingsHelper.isDefaultValue(setting);
            };

            // Edit dictionary values
            $scope.editArray = function (node) {
                var newBlade = {
                    id: 'settingDetailChild',
                    currentEntityId: node.name,
                    controller: 'platformWebApp.settingDictionaryController',
                    template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            // Description toggle
            $scope.getDictionaryValues = function (setting, callback) {
                callback(setting.allowedValues);
            };

            // Build tenant scope string for export document
            function getTenantScope() {
                if (blade.tenantType) {
                    return `tenant/${blade.tenantType}/${blade.tenantId}`;
                }
                return 'global';
            }

            // Export as JSON
            $scope.exportJson = function () {
                var getPromise;
                if (blade.tenantType && blade.tenantId) {
                    getPromise = settingsV2.getTenantValues({
                        tenantType: blade.tenantType,
                        tenantId: blade.tenantId,
                        modifiedOnly: true
                    }).$promise;
                } else {
                    getPromise = settingsV2.getGlobalValues({ modifiedOnly: true }).$promise;
                }

                getPromise.then(function (values) {
                    var scopeStr = getTenantScope();
                    var doc = {
                        version: '1.0',
                        exportedAt: new Date().toISOString(),
                        scope: scopeStr,
                        settings: values
                    };

                    var json = JSON.stringify(doc, null, 2);
                    var blob = new Blob([json], { type: 'application/json' });
                    var url = URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = `settings-${scopeStr.replace(/\//g, '-')}.json`;
                    a.click();
                    URL.revokeObjectURL(url);
                });
            };

            // Import from JSON
            $scope.importJson = function () {
                var input = document.createElement('input');
                input.type = 'file';
                input.accept = '.json';
                input.onchange = function (e) {
                    var file = e.target.files[0];
                    if (!file) {
                        return;
                    }

                    var reader = new FileReader();
                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            try {
                                var doc = JSON.parse(evt.target.result);
                                var settingsToImport = doc.settings || doc;
                                var count = Object.keys(settingsToImport).length;

                                var dialog = {
                                    id: 'confirmImportSettings',
                                    title: 'platform.blades.settings-unified.import-confirm-title',
                                    message: `Apply ${count} settings from imported file?`,
                                    callback: function (confirm) {
                                        if (confirm) {
                                            blade.isLoading = true;
                                            var savePromise;
                                            if (blade.tenantType && blade.tenantId) {
                                                savePromise = settingsV2.saveTenantValues(
                                                    { tenantType: blade.tenantType, tenantId: blade.tenantId },
                                                    settingsToImport).$promise;
                                            } else {
                                                savePromise = settingsV2.saveGlobalValues({}, settingsToImport).$promise;
                                            }
                                            savePromise.then(function () {
                                                blade.refresh();
                                            });
                                        }
                                    }
                                };
                                dialogService.showConfirmationDialog(dialog);
                            } catch (err) {
                                dialogService.showNotificationDialog({
                                    id: 'importError',
                                    title: 'Import Error',
                                    message: 'Invalid JSON file: ' + err.message
                                });
                            }
                        });
                    };
                    reader.readAsText(file);
                };
                input.click();
            };

            // Edit as JSON
            $scope.editAsJson = function () {
                var newBlade = {
                    id: 'settingsJsonEditor',
                    title: 'platform.blades.settings-json-editor.title',
                    tenantType: blade.tenantType,
                    tenantId: blade.tenantId,
                    parentRefresh: blade.refresh,
                    controller: 'platformWebApp.settingsJsonEditorController',
                    template: '$(Platform)/Scripts/app/settings/blades/settings-json-editor.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            // Toolbar
            blade.toolbarCommands = [
                {
                    name: 'platform.commands.save',
                    icon: 'fas fa-save',
                    executeMethod: blade.saveChanges,
                    canExecuteMethod: canSave
                },
                {
                    name: 'platform.commands.reset',
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        blade.refresh();
                    },
                    canExecuteMethod: isDirty
                },
                {
                    name: 'platform.blades.settings-unified.export-json',
                    icon: 'fa fa-download',
                    executeMethod: function () { $scope.exportJson(); },
                    canExecuteMethod: function () { return !blade.isLoading; }
                },
                {
                    name: 'platform.blades.settings-unified.import-json',
                    icon: 'fa fa-upload',
                    executeMethod: function () { $scope.importJson(); },
                    canExecuteMethod: function () { return !blade.isLoading; },
                    permission: 'platform:setting:update'
                },
                {
                    name: 'platform.blades.settings-unified.edit-json',
                    icon: 'fa fa-code',
                    executeMethod: function () { $scope.editAsJson(); },
                    canExecuteMethod: function () { return !blade.isLoading; },
                    showSeparator: true
                },
                {
                    name: 'platform.commands.cache-reset.name',
                    title: 'platform.commands.cache-reset.title',
                    icon: 'fa fa-eraser',
                    executeMethod: function () { resetCache(); },
                    canExecuteMethod: function () { return !blade.isLoading; },
                    permission: 'cache:reset'
                },
                {
                    name: 'platform.commands.restart',
                    icon: 'fa fa-bolt',
                    executeMethod: function () { restart(); },
                    canExecuteMethod: function () { return !blade.isLoading; },
                    permission: 'platform:module:manage'
                }
            ];

            function restart() {
                var dialog = {
                    id: 'confirmRestart',
                    title: 'platform.dialogs.app-restart.title',
                    message: 'platform.dialogs.app-restart.message',
                    callback: function (confirm) {
                        if (confirm) {
                            blade.isLoading = true;
                            try {
                                modulesApi.restart(function () { });
                            } catch (err) { }
                            finally {
                                $timeout(function () { }, 3000).then(function () {
                                    return waitForRestart(1000);
                                });
                            }
                        }
                    }
                };
                dialogService.showWarningDialog(dialog);
            }

            function resetCache() {
                var confirmDialog = {
                    id: 'confirmCacheResetDialog',
                    title: 'platform.dialogs.cache-reset.title',
                    message: 'platform.dialogs.cache-reset.confirm-reset-message',
                    callback: function (confirm) {
                        if (confirm) {
                            blade.isLoading = true;
                            changeLogApi.resetPlatformCache({}, function () {
                                blade.isLoading = false;
                                dialogService.showSuccessDialog({
                                    id: 'successCacheResetDialog',
                                    title: 'platform.dialogs.cache-reset.title',
                                    message: 'platform.dialogs.cache-reset.reset-successfully-message'
                                });
                            });
                        }
                    }
                };
                dialogService.showWarningDialog(confirmDialog);
            }

            blade.onClose = function (closeCallback) {
                bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.saveChanges, closeCallback,
                    'platform.dialogs.settings-delete.title', 'platform.dialogs.settings-delete.message');
            };

            // Intercept navigation away from this workspace (e.g. main menu click).
            // Root blades with isClosingDisabled don't trigger onClose,
            // so we use $transitions.onBefore (UI-Router 1.x+) to block navigation when dirty.
            var deregisterTransitionHook = $transitions.onBefore(
                { from: 'workspace.settingsUnified' },
                function () {
                    if (!isDirty()) {
                        return true;
                    }

                    // Return a promise that resolves after user confirms
                    var deferred = $q.defer();
                    var dialog = {
                        id: 'confirmSettingsNavAway',
                        title: $translate.instant('platform.dialogs.settings-delete.title'),
                        message: $translate.instant('platform.dialogs.settings-delete.message'),
                        callback: function (userChoseYes) {
                            if (userChoseYes) {
                                blade.saveChanges().then(function () {
                                    deferred.resolve(true);
                                });
                            } else {
                                // Reset dirty state so onClose won't show dialog again
                                // when the blade is re-opened via showBlade -> closeBlade
                                _.each(blade.mergedSettings, function (s) {
                                    blade.origSettingValues[s.name] = getSettingCurrentValue(s);
                                });
                                deferred.resolve(true);
                            }
                        }
                    };
                    dialogService.showConfirmationDialog(dialog);
                    return deferred.promise;
                }
            );
            // Clean up transition hook when scope is destroyed
            $scope.$on('$destroy', function () {
                deregisterTransitionHook();
            });

            // Initialize
            blade.refresh();
        }
    ]);
