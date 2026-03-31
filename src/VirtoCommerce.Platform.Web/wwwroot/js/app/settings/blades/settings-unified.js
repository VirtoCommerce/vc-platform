angular.module('platformWebApp')
    .controller('platformWebApp.settingsUnifiedController', [
        '$scope', '$q', '$translate', '$transitions', '$state', '$stateParams', 'platformWebApp.settingsV2', 'platformWebApp.settings.helper',
        'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.modulesApi',
        'platformWebApp.changeLogApi', 'platformWebApp.WaitForRestart', '$timeout',
        function ($scope, $q, $translate, $transitions, $state, $stateParams, settingsV2, settingsHelper, bladeNavigationService, dialogService, modulesApi, changeLogApi, waitForRestart, $timeout) {
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
                            return settingsV2.getTenantSchema({ tenantType: blade.tenantType }).$promise;
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
                        if (blade.tenantType && blade.tenantId) {
                            return settingsV2.saveTenantValues(
                                { tenantType: blade.tenantType, tenantId: blade.tenantId },
                                changedValues).$promise;
                        }
                        return settingsV2.saveGlobalValues({}, changedValues).$promise;
                    }
                };
            }

            function createEntityDataSource() {
                return {
                    loadSchema: function () {
                        return settingsV2.getTenantSchema({ tenantType: blade.tenantType }).$promise;
                    },
                    loadValues: function () {
                        var parentSettings = getParentEntitySettings();
                        var values = {};
                        _.each(parentSettings, function (s) {
                            values[s.name] = (s.value != null) ? s.value : s.defaultValue;
                        });
                        return $q.resolve(values);
                    },
                    saveValues: function (changedValues) {
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

            dataSource = blade.isEntityMode ? createEntityDataSource() : createApiDataSource();

            // ================================================================
            // Filter state (panel toggle & outside-click handled by va-filter-panel directive)
            // ================================================================

            var filter = $scope.filter = {
                modifiedOnly: false,
                moduleId: '',
                showTenantProperties: false,
                hasActiveFilters: function () {
                    return filter.modifiedOnly || filter.moduleId !== '' || filter.showTenantProperties;
                },
                clearFilters: function () {
                    filter.modifiedOnly = false;
                    filter.moduleId = '';
                    filter.showTenantProperties = false;
                    applyFilters();
                },
                criteriaChanged: function () {
                    applyFilters();
                }
            };

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

            var refreshGeneration = 0; // race condition guard

            blade.refresh = function () {
                blade.isLoading = true;
                var thisGeneration = ++refreshGeneration;

                $q.all([dataSource.loadSchema(), dataSource.loadValues()]).then(function (results) {
                    // Ignore stale results if a newer refresh was started
                    if (thisGeneration !== refreshGeneration) {
                        return;
                    }
                    initializeBlade(results[0], results[1]);
                }, function (error) {
                    if (thisGeneration !== refreshGeneration) {
                        return;
                    }
                    var msg = (error.data && error.data.message) ? error.data.message : `Error ${error.status}`;
                    bladeNavigationService.setError(msg, blade);
                });
            };

            function initializeBlade(schemas, values) {
                // Strip $resource metadata from values
                values = angular.copy(values);

                if (blade.settingNames && blade.settingNames.length) {
                    var nameSet = {};
                    _.each(blade.settingNames, function (n) { nameSet[n] = true; });
                    schemas = _.filter(schemas, function (s) { return nameSet[s.name]; });
                }

                blade.allSchemas = schemas;
                blade.allValues = angular.copy(values);
                blade.origValues = angular.copy(values);

                blade.modules = _.chain(schemas).pluck('moduleId').compact().uniq().sortBy().map(function (id) {
                    return { id: id };
                }).value();

                blade.mergedSettings = mergeSchemaAndValues(schemas, values);

                // Snapshot using getSaveableValue so format matches getChangedValues comparison
                blade.origSettingValues = {};
                _.each(blade.mergedSettings, function (s) {
                    blade.origSettingValues[s.name] = angular.copy(getSaveableValue(s));
                });

                _.each(blade.mergedSettings, function (setting) {
                    var translateKey = 'settings.' + setting.name + '.title';
                    var result = $translate.instant(translateKey);
                    setting.translatedName = result !== translateKey ? result : (setting.displayName || setting.name);
                });

                buildFlatTree(blade.mergedSettings);
                updateModifiedCount();
                applyFilters();
                blade.isLoading = false;

                scrollToDeepLink();
            }

            function getSaveableValue(setting) {
                if (setting.isDictionary) {
                    if (setting.values && setting.values[0] && setting.values[0].value) {
                        return setting.values[0].value.id;
                    }
                    return setting.value;
                }
                return (setting.values && setting.values[0]) ? setting.values[0].value : setting.value;
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
                        assignedToTenants: schema.assignedToTenants || [],
                        value: value,
                        values: settingsHelper.getSettingValues({ isDictionary: schema.isDictionary, value: value })
                    };
                });
            }

            // ================================================================
            // Tree building
            // ================================================================

            // Lookup map: groupName -> node (for O(1) parent lookups in isNodeVisible)
            var nodeByGroupName = {};

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
                        idCounter += 1;
                        var node = {
                            id: `n${idCounter}`,
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
                    name: '', // rendered via translate filter in template to avoid $translate.instant() race condition on F5
                    groupName: null,
                    level: 0,
                    hasChildren: false,
                    expanded: true,
                    parent: null
                };

                blade.flatTree = [allNode].concat(flatNodes);

                // Build O(1) lookup map for parent traversal
                nodeByGroupName = {};
                _.each(blade.flatTree, function (node) {
                    if (node.groupName) {
                        nodeByGroupName[node.groupName] = node;
                    }
                });
            }

            // ================================================================
            // Tree visibility & selection
            // ================================================================

            $scope.isNodeVisible = function (node) {
                if (node.id === '__all__') {
                    return true;
                }

                var isFiltering = blade.searchText || filter.modifiedOnly || filter.moduleId;
                if (isFiltering && blade.visibleNodeGroups && node.groupName) {
                    if (!blade.visibleNodeGroups[node.groupName]) {
                        return false;
                    }
                }

                if (node.level <= 1) {
                    return true;
                }

                // Walk up parents using O(1) lookup map
                var parentGroupName = node.parent;
                while (parentGroupName) {
                    var parentNode = nodeByGroupName[parentGroupName];
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

            // Pre-computed map: groupName -> hasModified (updated in updateModifiedCount)
            var modifiedGroupMap = {};

            $scope.nodeHasModified = function (node) {
                return !!modifiedGroupMap[node.groupName];
            };

            function updateModifiedCount() {
                var count = 0;
                modifiedGroupMap = {};

                _.each(blade.mergedSettings, function (s) {
                    if (!settingsHelper.isDefaultValue(s)) {
                        count++;
                        // Mark this group and all ancestor groups as having modified settings
                        var gn = s.groupName || 'General';
                        var segments = gn.split('|');
                        for (var i = 1; i <= segments.length; i++) {
                            modifiedGroupMap[segments.slice(0, i).join('|')] = true;
                        }
                    }
                });

                blade.modifiedCount = count;
            }

            // ================================================================
            // Filtering
            // ================================================================

            function applyFilters() {
                var settings = blade.mergedSettings;

                // In global mode, hide tenant-assigned properties unless filter is on
                if (!blade.isEntityMode && !filter.showTenantProperties) {
                    settings = _.filter(settings, function (s) {
                        return !s.assignedToTenants || s.assignedToTenants.length === 0;
                    });
                }

                if (filter.moduleId) {
                    settings = _.filter(settings, function (s) { return s.moduleId === filter.moduleId; });
                }

                if (filter.modifiedOnly) {
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

                var isFiltering = blade.searchText || filter.modifiedOnly || filter.moduleId;
                if (isFiltering) {
                    _.each(blade.flatTree, function (node) {
                        if (node.hasChildren && node.groupName && visibleNodeGroups[node.groupName]) {
                            node.expanded = true;
                        }
                    });
                }

                // Apply selected group filter (works alongside search — search narrows within the group)
                if (blade.selectedGroup) {
                    var selectedPrefix = blade.selectedGroup.groupName + '|';
                    settings = _.filter(settings, function (s) {
                        return s.groupName === blade.selectedGroup.groupName ||
                            (s.groupName && s.groupName.indexOf(selectedPrefix) === 0);
                    });
                }

                var grouped = _.groupBy(settings, function (s) {
                    return s.groupName ? s.groupName.replace(/\|/g, ' > ') : 'General';
                });

                blade.filteredSettings = grouped;
                blade.visibleGroupNames = _.keys(grouped).sort();

                // Build reverse map: display group name -> raw groupName (for deep link data attributes)
                blade.filteredSettingsGroupMap = {};
                _.each(settings, function (s) {
                    var displayKey = s.groupName ? s.groupName.replace(/\|/g, ' > ') : 'General';
                    blade.filteredSettingsGroupMap[displayKey] = s.groupName;
                });
            }

            // Use $watch only for blade.searchText (typed by user, no ng-change available).
            // All other filter changes go through filter.criteriaChanged() via ng-change in template.
            $scope.$watch('blade.searchText', function () {
                applyFilters();
            });

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
                    var currentVal = getSaveableValue(s);
                    var origVal = blade.origSettingValues[s.name];
                    return !angular.equals(currentVal, origVal);
                }) && blade.hasUpdatePermission();
            }

            function canSave() {
                return isDirty() && (!formScope || formScope.$valid);
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
                    blade.origSettingValues[s.name] = angular.copy(getSaveableValue(s));
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
                }, function (error) {
                    blade.isLoading = false;
                    var msg = (error && error.data && error.data.message) ? error.data.message : 'Save failed';
                    bladeNavigationService.setError(msg, blade);
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
                    // isApiSave tells the dictionary controller to load/save via the v1 API
                    // (settingsApi.get/update). Without this flag, it watches
                    // parentBlade.currentEntities which doesn't exist in the unified blade.
                    isApiSave: true,
                    controller: 'platformWebApp.settingDictionaryController',
                    template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.getDictionaryValues = function (setting, callback) {
                callback(setting.allowedValues);
            };

            // ================================================================
            // Deep linking & Copy link
            // ================================================================

            function scrollToDeepLink() {
                if (!$stateParams.group && !$stateParams.setting) {
                    return;
                }
                $timeout(function () {
                    var scrollTarget = null;
                    var settingParam = $stateParams.setting;
                    var groupParam = $stateParams.group;

                    if (settingParam) {
                        // Use CSS.escape to prevent selector injection from URL params
                        scrollTarget = document.querySelector(`[data-setting-name="${CSS.escape(settingParam)}"]`);
                    } else if (groupParam) {
                        scrollTarget = document.querySelector(`[data-group-name="${CSS.escape(groupParam)}"]`);
                    }
                    if (scrollTarget) {
                        scrollTarget.scrollIntoView({ behavior: 'smooth', block: 'start' });
                        angular.element(scrollTarget).addClass('__highlight');
                        $timeout(function () {
                            angular.element(scrollTarget).removeClass('__highlight');
                        }, 2000);
                    }
                }, 300);
            }

            $scope.copyGroupLink = function (groupName) {
                if (!groupName) {
                    return;
                }
                var url = $state.href('workspace.settings', { group: groupName }, { absolute: true });
                if (navigator.clipboard && navigator.clipboard.writeText) {
                    navigator.clipboard.writeText(url).catch(function () {
                        fallbackCopyToClipboard(url);
                    });
                } else {
                    fallbackCopyToClipboard(url);
                }
            };

            function fallbackCopyToClipboard(text) {
                var textarea = document.createElement('textarea');
                textarea.value = text;
                textarea.style.position = 'fixed';
                textarea.style.opacity = '0';
                document.body.appendChild(textarea);
                textarea.select();
                try {
                    document.execCommand('copy');
                } catch (e) {
                    // silent fallback failure
                }
                document.body.removeChild(textarea);
            }

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
                    // angular.copy strips $resource's $$-prefixed properties ($promise, $resolved)
                    // that create circular references and would cause JSON.stringify to throw.
                    var cleanValues = angular.copy(values);
                    var scopeStr = getTenantScope();
                    var doc = {
                        version: '1.0',
                        exportedAt: new Date().toISOString(),
                        scope: scopeStr,
                        settings: cleanValues
                    };

                    var json = JSON.stringify(doc, null, 2);
                    var blob = new Blob([json], { type: 'application/json' });
                    var url = URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = `settings-${scopeStr.replace(/\//g, '-')}.json`;
                    a.click();
                    URL.revokeObjectURL(url);
                }, function (error) {
                    var msg = (error && error.data && error.data.message) ? error.data.message : 'Export failed';
                    bladeNavigationService.setError(msg, blade);
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
                                    message: $translate.instant('platform.blades.settings-unified.import-confirm-message', { count: count }),
                                    callback: function (confirm) {
                                        if (confirm) {
                                            blade.isLoading = true;
                                            // Import uses replaceAll=true: the file is the complete set of modifications
                                            var importPromise;
                                            if (blade.tenantType && blade.tenantId) {
                                                importPromise = settingsV2.saveTenantValues(
                                                    { tenantType: blade.tenantType, tenantId: blade.tenantId, replaceAll: true },
                                                    settingsToImport).$promise;
                                            } else {
                                                importPromise = settingsV2.saveGlobalValues({ replaceAll: true }, settingsToImport).$promise;
                                            }
                                            importPromise.then(function () {
                                                blade.refresh();
                                            }, function () {
                                                blade.isLoading = false;
                                            });
                                        }
                                    }
                                };
                                dialogService.showConfirmationDialog(dialog);
                            } catch (err) {
                                dialogService.showNotificationDialog({
                                    id: 'importError',
                                    title: 'Import Error',
                                    message: `Invalid JSON file: ${err.message}`
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
                    controller: 'platformWebApp.settingsJsonEditorController',
                    template: '$(Platform)/Scripts/app/settings/blades/settings-json-editor.tpl.html'
                };

                if (blade.isEntityMode) {
                    // In-memory mode: serialize current settings to JSON, save via callback.
                    // Include all non-default properties (matching the blue dot indicator)
                    // plus any properties the user changed in the current session.
                    var currentValues = {};
                    _.each(blade.mergedSettings, function (s) {
                        var val = getSaveableValue(s);
                        var isNonDefault = !settingsHelper.isDefaultValue(s);
                        var isDirtyInSession = !angular.equals(val, blade.origSettingValues[s.name]);
                        if (isNonDefault || isDirtyInSession) {
                            currentValues[s.name] = val;
                        }
                    });
                    var doc = {
                        version: '1.0',
                        scope: 'entity',
                        settings: currentValues
                    };
                    newBlade.settingsData = JSON.stringify(doc, null, 2);
                    newBlade.onSaveJson = function (settingsDict) {
                        // Apply parsed JSON values back into mergedSettings.
                        // Properties in the dict get their new value.
                        // Properties NOT in the dict are reset to defaultValue.
                        // We replace each object in mergedSettings (not just mutate)
                        // to force va-generic-value-input's ngModel.$render().
                        for (var i = 0; i < blade.mergedSettings.length; i++) {
                            var s = blade.mergedSettings[i];
                            var newValue;
                            if (settingsDict.hasOwnProperty(s.name)) {
                                newValue = settingsDict[s.name];
                            } else {
                                // Not in JSON — reset to default
                                newValue = s.defaultValue;
                            }

                            // Clone + replace to trigger UI re-render
                            var updated = angular.copy(s);
                            updated.allowedValues = s.allowedValues; // preserve object identity for ui-select
                            updated.value = newValue;
                            updated.values = settingsHelper.getSettingValues({ isDictionary: updated.isDictionary, value: newValue });
                            blade.mergedSettings[i] = updated;
                        }
                        updateModifiedCount();
                        applyFilters();

                        // Sync replaced objects into filteredSettings for display
                        _.each(blade.filteredSettings, function (groupSettings, key) {
                            for (var j = 0; j < groupSettings.length; j++) {
                                var replacement = _.find(blade.mergedSettings, function (m) { return m.name === groupSettings[j].name; });
                                if (replacement && replacement !== groupSettings[j]) {
                                    groupSettings[j] = replacement;
                                }
                            }
                        });
                    };
                } else {
                    // API mode: editor loads/saves via REST API
                    newBlade.tenantType = blade.tenantType;
                    newBlade.tenantId = blade.tenantId;
                    newBlade.parentRefresh = blade.refresh;
                }

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
                    dataSource.saveValues(changedValues).then(function () {
                        $scope.bladeClose();
                    });
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

            if (!blade.isEntityMode) {
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
            }

            // Edit as JSON available in both modes (API mode: loads from API; entity mode: in-memory)
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
                            } catch (err) {
                                // restart may fail transiently
                            }
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
                    closeCallback();
                } else {
                    bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.saveChanges, closeCallback,
                        'platform.dialogs.settings-delete.title', 'platform.dialogs.settings-delete.message');
                }
            };

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
                var entityWatchInitialized = false;
                $scope.$watch('blade.parentBlade.currentEntity.settings', function (newSettings, oldSettings) {
                    if (!entityWatchInitialized) {
                        entityWatchInitialized = true;
                        return;
                    }
                    if (newSettings && newSettings !== oldSettings) {
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
