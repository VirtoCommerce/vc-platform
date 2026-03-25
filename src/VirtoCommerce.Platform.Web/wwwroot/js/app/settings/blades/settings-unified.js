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

            // ================================================================
            // Data Source abstraction — isolates Load/Save operations
            // so the rest of the controller is storage-agnostic.
            //
            // blade.isEntityMode = true:  schema from API, values from parent entity in-memory
            // blade.isEntityMode = false: schema + values from API, save via API
            // ================================================================

            var dataSource;

            function createApiDataSource() {
                return {
                    loadSchema: function () {
                        if (blade.tenantType && blade.tenantId) {
                            return settingsV2.getTenantSchema({ tenantType: blade.tenantType, tenantId: blade.tenantId }).$promise;
                        }
                        return settingsV2.getGlobalSchema({}).$promise;
                    },
                    loadValues: function () {
                        if (blade.tenantType && blade.tenantId) {
                            return settingsV2.getTenantValues({ tenantType: blade.tenantType, tenantId: blade.tenantId }).$promise;
                        }
                        return settingsV2.getGlobalValues({}).$promise;
                    },
                    saveValues: function (changedValues) {
                        var savePromise;
                        if (blade.tenantType && blade.tenantId) {
                            savePromise = settingsV2.saveTenantValues(
                                { tenantType: blade.tenantType, tenantId: blade.tenantId },
                                changedValues).$promise;
                        } else {
                            savePromise = settingsV2.saveGlobalValues({}, changedValues).$promise;
                        }
                        return savePromise;
                    }
                };
            }

            function createEntityDataSource() {
                return {
                    loadSchema: function () {
                        // Schema always from API (need metadata like valueType, allowedValues, etc.)
                        return settingsV2.getTenantSchema({ tenantType: blade.tenantType, tenantId: blade.tenantId }).$promise;
                    },
                    loadValues: function () {
                        // Values from parent entity's in-memory settings array — no API call
                        var parentSettings = getParentEntitySettings();
                        var values = {};
                        _.each(parentSettings, function (s) {
                            values[s.name] = (s.value != null) ? s.value : s.defaultValue;
                        });
                        return $q.resolve(values);
                    },
                    saveValues: function (changedValues) {
                        // Write back to parent entity's in-memory settings array — no API call
                        var parentSettings = getParentEntitySettings();
                        _.each(changedValues, function (value, name) {
                            var parentSetting = _.find(parentSettings, function (ps) { return ps.name === name; });
                            if (parentSetting) {
                                parentSetting.value = value;
                            }
                        });
                        return $q.resolve();
                    }
                };
            }

            function getParentEntitySettings() {
                if (blade.parentBlade && blade.parentBlade.currentEntity) {
                    return blade.parentBlade.currentEntity.settings || [];
                }
                return [];
            }

            // Select data source based on mode
            dataSource = blade.isEntityMode ? createEntityDataSource() : createApiDataSource();

            // ================================================================
            // Filter state (mirrors notification journal filter pattern)
            // ================================================================

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

            // ================================================================
            // Blade state
            // ================================================================

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

            // ================================================================
            // Load (uses dataSource)
            // ================================================================

            blade.refresh = function () {
                blade.isLoading = true;

                $q.all([dataSource.loadSchema(), dataSource.loadValues()]).then(function (results) {
                    initializeBlade(results[0], results[1]);
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
            };

            function initializeBlade(schemas, values) {
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
                blade.modules = _.chain(schemas).pluck('moduleId').compact().uniq().sortBy().map(function (id) {
                    return { id: id };
                }).value();

                // Merge schema + values into ObjectSettingEntry-compatible objects
                blade.mergedSettings = mergeSchemaAndValues(schemas, values);

                // Snapshot the initial values for dirty checking
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
            }

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

                    return {
                        name: schema.name,
                        displayName: schema.displayName,
                        groupName: schema.groupName,
                        moduleId: schema.moduleId,
                        valueType: schema.valueType,
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

            // ================================================================
            // Tree building
            // ================================================================

            function buildFlatTree(settings) {
                var idCounter = 0;
                var rootChildren = {};

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
                            expanded: level === 1,
                            parent: parentGroupName
                        };
                        flatNodes.push(node);
                        if (hasChildren) {
                            flatten(entry.childMap, level + 1, entry.groupName);
                        }
                    });
                }

                flatten(rootChildren, 1, null);

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

            // ================================================================
            // Tree visibility & selection
            // ================================================================

            $scope.isNodeVisible = function (node) {
                if (node.id === '__all__') {
                    return true;
                }

                var isFiltering = blade.searchText || $scope.filter.modifiedOnly || $scope.filter.moduleId;
                if (isFiltering && blade.visibleNodeGroups && node.groupName) {
                    if (!blade.visibleNodeGroups[node.groupName]) {
                        return false;
                    }
                }

                if (node.level <= 1) {
                    return true;
                }

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

            $scope.selectGroup = function (node) {
                if (node.groupName === null) {
                    blade.selectedGroup = null;
                } else if (node.hasChildren && node === blade.selectedGroup) {
                    node.expanded = !node.expanded;
                } else {
                    blade.selectedGroup = node;
                    if (node.hasChildren) {
                        node.expanded = true;
                    }
                }
                applyFilters();
            };

            $scope.isModified = function (setting) {
                return !settingsHelper.isDefaultValue(setting);
            };

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

            // ================================================================
            // Filtering
            // ================================================================

            function applyFilters() {
                var settings = blade.mergedSettings;

                if ($scope.filter.moduleId) {
                    settings = _.filter(settings, function (s) { return s.moduleId === $scope.filter.moduleId; });
                }

                if ($scope.filter.modifiedOnly) {
                    settings = _.filter(settings, function (s) { return !settingsHelper.isDefaultValue(s); });
                }

                if (blade.searchText) {
                    var search = blade.searchText.toLowerCase();
                    settings = _.filter(settings, function (s) {
                        if ((s.name && s.name.toLowerCase().indexOf(search) >= 0) ||
                            (s.translatedName && s.translatedName.toLowerCase().indexOf(search) >= 0) ||
                            (s.groupName && s.groupName.toLowerCase().indexOf(search) >= 0)) {
                            return true;
                        }
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

                var visibleNodeGroups = {};
                _.each(settings, function (s) {
                    var gn = s.groupName || 'General';
                    var segments = gn.split('|');
                    for (var i = 1; i <= segments.length; i++) {
                        visibleNodeGroups[segments.slice(0, i).join('|')] = true;
                    }
                });
                blade.visibleNodeGroups = visibleNodeGroups;

                var isFiltering = blade.searchText || $scope.filter.modifiedOnly || $scope.filter.moduleId;
                if (isFiltering) {
                    _.each(blade.flatTree, function (node) {
                        if (node.hasChildren && node.groupName && visibleNodeGroups[node.groupName]) {
                            node.expanded = true;
                        }
                    });
                }

                if (blade.selectedGroup && !blade.searchText) {
                    var selectedPrefix = blade.selectedGroup.groupName + '|';
                    settings = _.filter(settings, function (s) {
                        return s.groupName === blade.selectedGroup.groupName ||
                            (s.groupName && s.groupName.indexOf(selectedPrefix) === 0);
                    });
                }

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

            $scope.$watch('blade.searchText', function () { applyFilters(); });
            $scope.$watch('filter.modifiedOnly', function () { applyFilters(); });
            $scope.$watch('filter.moduleId', function () { applyFilters(); });

            // ================================================================
            // Dirty checking
            // ================================================================

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

            function getSaveableValue(setting) {
                if (setting.isDictionary) {
                    return (setting.values && setting.values[0]) ? setting.values[0].value.id : setting.value;
                }
                return (setting.values && setting.values[0]) ? setting.values[0].value : setting.value;
            }

            function getChangedValues() {
                var changedValues = {};
                _.each(blade.mergedSettings, function (s) {
                    var currentVal = getSaveableValue(s);
                    var origVal = blade.origSettingValues[s.name];
                    if (!angular.equals(currentVal, origVal)) {
                        changedValues[s.name] = currentVal;
                    }
                });
                return changedValues;
            }

            function updateOrigSnapshot() {
                _.each(blade.mergedSettings, function (s) {
                    blade.origSettingValues[s.name] = getSettingCurrentValue(s);
                });
            }

            // ================================================================
            // Save (uses dataSource)
            // ================================================================

            blade.saveChanges = function () {
                blade.isLoading = true;

                var changedValues = getChangedValues();
                if (Object.keys(changedValues).length === 0) {
                    blade.isLoading = false;
                    return $q.resolve();
                }

                return dataSource.saveValues(changedValues).then(function () {
                    updateOrigSnapshot();
                    updateModifiedCount();
                    blade.isLoading = false;
                });
            };

            // ================================================================
            // Reset to default
            // ================================================================

            $scope.resetToDefaultValue = function (setting) {
                var displayKey = null;
                _.each(blade.filteredSettings, function (groupSettings, key) {
                    if (_.any(groupSettings, function (s) { return s.name === setting.name; })) {
                        displayKey = key;
                    }
                });

                if (displayKey !== null) {
                    var tempGroups = {};
                    tempGroups[setting.groupName] = blade.filteredSettings[displayKey];
                    settingsHelper.resetToDefaultValue(tempGroups, setting);
                } else {
                    settingsHelper.resetToDefaultValue({}, setting);
                }

                // Sync replaced objects from filteredSettings into mergedSettings
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

            // ================================================================
            // Dictionary editor
            // ================================================================

            $scope.editArray = function (node) {
                var newBlade = {
                    id: 'settingDetailChild',
                    currentEntityId: node.name,
                    controller: 'platformWebApp.settingDictionaryController',
                    template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.getDictionaryValues = function (setting, callback) {
                callback(setting.allowedValues);
            };

            // ================================================================
            // Export / Import / Edit as JSON
            // ================================================================

            function getTenantScope() {
                if (blade.tenantType) {
                    return `tenant/${blade.tenantType}/${blade.tenantId}`;
                }
                return 'global';
            }

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
                                            dataSource.saveValues(settingsToImport).then(function () {
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

            // ================================================================
            // OK/Cancel for entity mode
            // ================================================================

            if (blade.isEntityMode) {
                $scope.saveChanges = function () {
                    if (!blade.hasUpdatePermission()) {
                        return;
                    }
                    var changedValues = getChangedValues();
                    dataSource.saveValues(changedValues);
                    $scope.bladeClose();
                };

                $scope.cancelChanges = function () {
                    $scope.bladeClose();
                };
            }

            // ================================================================
            // Toolbar
            // ================================================================

            blade.toolbarCommands = [];

            if (!blade.isEntityMode) {
                blade.toolbarCommands.push({
                    name: 'platform.commands.save',
                    icon: 'fas fa-save',
                    executeMethod: blade.saveChanges,
                    canExecuteMethod: canSave
                });
            }

            blade.toolbarCommands.push({
                name: 'platform.commands.reset',
                icon: 'fa fa-undo',
                executeMethod: function () { blade.refresh(); },
                canExecuteMethod: isDirty
            });

            blade.toolbarCommands.push({
                name: 'platform.blades.settings-unified.export-json',
                icon: 'fa fa-download',
                executeMethod: function () { $scope.exportJson(); },
                canExecuteMethod: function () { return !blade.isLoading; }
            });

            blade.toolbarCommands.push({
                name: 'platform.blades.settings-unified.import-json',
                icon: 'fa fa-upload',
                executeMethod: function () { $scope.importJson(); },
                canExecuteMethod: function () { return !blade.isLoading; },
                permission: 'platform:setting:update'
            });

            blade.toolbarCommands.push({
                name: 'platform.blades.settings-unified.edit-json',
                icon: 'fa fa-code',
                executeMethod: function () { $scope.editAsJson(); },
                canExecuteMethod: function () { return !blade.isLoading; },
                showSeparator: !blade.isEntityMode
            });

            if (!blade.isEntityMode) {
                blade.toolbarCommands.push({
                    name: 'platform.commands.cache-reset.name',
                    title: 'platform.commands.cache-reset.title',
                    icon: 'fa fa-eraser',
                    executeMethod: function () { resetCache(); },
                    canExecuteMethod: function () { return !blade.isLoading; },
                    permission: 'cache:reset'
                });

                blade.toolbarCommands.push({
                    name: 'platform.commands.restart',
                    icon: 'fa fa-bolt',
                    executeMethod: function () { restart(); },
                    canExecuteMethod: function () { return !blade.isLoading; },
                    permission: 'platform:module:manage'
                });
            }

            // ================================================================
            // Restart / Reset cache (global mode only)
            // ================================================================

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

            // ================================================================
            // Blade close / navigation guard
            // ================================================================

            blade.onClose = function (closeCallback) {
                if (blade.isEntityMode) {
                    // Entity mode: suppress confirmation — parent entity handles its own save flow
                    closeCallback();
                } else {
                    bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.saveChanges, closeCallback,
                        'platform.dialogs.settings-delete.title', 'platform.dialogs.settings-delete.message');
                }
            };

            // Navigation guard: only for global (workspace-level) mode
            if (!blade.isEntityMode) {
                var deregisterTransitionHook = $transitions.onBefore(
                    { from: 'workspace.settings' },
                    function () {
                        if (!isDirty()) {
                            return true;
                        }

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
                                    updateOrigSnapshot();
                                    deferred.resolve(true);
                                }
                            }
                        };
                        dialogService.showConfirmationDialog(dialog);
                        return deferred.promise;
                    }
                );
                $scope.$on('$destroy', function () {
                    deregisterTransitionHook();
                });
            }

            // ================================================================
            // Watch parent entity settings (entity mode only)
            // ================================================================

            if (blade.isEntityMode) {
                $scope.$watch('blade.parentBlade.currentEntity.settings', function (newSettings) {
                    if (newSettings) {
                        blade.refresh();
                    }
                });
            }

            // ================================================================
            // Initialize
            // ================================================================

            blade.refresh();
        }
    ]);
