angular.module('platformWebApp')
    .controller('platformWebApp.settingsJsonEditorController', [
        '$scope', '$timeout', 'platformWebApp.settingsV2', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
        function ($scope, $timeout, settingsV2, bladeNavigationService, dialogService) {
            var blade = $scope.blade;
            blade.headIcon = 'fa fa-code';
            blade.updatePermission = 'platform:setting:update';

            // Two modes:
            // 1. API mode (default): loads from API, saves via API with replaceAll=true
            //    Requires: blade.tenantType/tenantId (optional) + blade.parentRefresh
            // 2. In-memory mode: data provided by parent blade, saves via callback
            //    Requires: blade.settingsData (JSON string) + blade.onSaveJson(settingsDict)
            var isInMemoryMode = !!blade.settingsData;

            var cmEditor = null;
            var changeHandler = null;

            // CodeMirror options — same config as genericValueInput uses for Json valueType
            $scope.editorOptions = {
                lineWrapping: true,
                lineNumbers: true,
                mode: { name: 'javascript', json: true },
                extraKeys: {
                    "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); },
                    "Ctrl-Alt-F": function () { formatJson(); }
                },
                foldGutter: true,
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                onLoad: function (_editor) {
                    cmEditor = _editor;
                    injectFormatButton(_editor);
                }
            };

            if (typeof window.jsonlint !== 'undefined') {
                $scope.editorOptions.lint = true;
                $scope.editorOptions.gutters = ["CodeMirror-linenumbers", "CodeMirror-foldgutter", "CodeMirror-lint-markers"];
            }

            function injectFormatButton(_editor) {
                var wrapper = angular.element(_editor.getWrapperElement());
                wrapper.css('position', 'relative');

                var container = angular.element('<div class="json-controls"></div>');
                container.css({
                    position: 'absolute',
                    top: '5px',
                    right: '5px',
                    zIndex: '10',
                    display: 'flex',
                    gap: '5px'
                });

                var statusIndicator = angular.element('<div class="json-btn status-indicator"></div>');
                var commonStyle = {
                    padding: '2px 8px',
                    fontSize: '12px',
                    borderRadius: '3px',
                    height: '24px',
                    lineHeight: '20px',
                    boxSizing: 'border-box',
                    border: 'none'
                };
                statusIndicator.css(angular.extend({}, commonStyle, {
                    backgroundColor: '#4CAF50',
                    color: 'white',
                    display: 'none',
                    alignItems: 'center',
                    justifyContent: 'center'
                }));

                var formatBtn = angular.element('<button class="json-btn format-btn" title="Format JSON (Ctrl+Alt+F)">Format JSON</button>');
                formatBtn.css(angular.extend({}, commonStyle, {
                    backgroundColor: '#43b0e6',
                    color: 'white',
                    cursor: 'pointer'
                }));
                formatBtn.on('click', function () {
                    formatJson();
                    $scope.$apply();
                });

                container.append(statusIndicator);
                container.append(formatBtn);
                wrapper.prepend(container);

                changeHandler = function () {
                    updateIndicator(statusIndicator, _editor);
                };
                _editor.on('change', changeHandler);
                $timeout(function () {
                    updateIndicator(statusIndicator, _editor);
                }, 100);
            }

            function updateIndicator(indicator, editor) {
                try {
                    var content = editor.getValue();
                    if (!content) {
                        indicator.css('display', 'none');
                        return;
                    }
                    JSON.parse(content);
                    indicator.css({ backgroundColor: '#4CAF50', display: 'none' });
                    indicator.text('Valid JSON');
                } catch (e) {
                    indicator.css({ backgroundColor: '#F44336', display: 'flex' });
                    indicator.text('Invalid JSON');
                }
            }

            $scope.$on('$destroy', function () {
                if (cmEditor && changeHandler) {
                    cmEditor.off('change', changeHandler);
                }
            });

            // ================================================================
            // Load
            // ================================================================

            blade.refresh = function () {
                if (isInMemoryMode) {
                    // Data provided by parent — no API call
                    blade.origJson = blade.settingsData;
                    blade.currentJson = angular.copy(blade.origJson);
                    blade.isLoading = false;
                    return;
                }

                // API mode: load modified values from server
                blade.isLoading = true;
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
                    var doc = {
                        version: '1.0',
                        exportedAt: new Date().toISOString(),
                        scope: blade.tenantType ? 'tenant/' + blade.tenantType + '/' + blade.tenantId : 'global',
                        settings: values
                    };

                    blade.origJson = JSON.stringify(doc, null, 2);
                    blade.currentJson = angular.copy(blade.origJson);
                    blade.isLoading = false;
                });
            };

            // ================================================================
            // Save
            // ================================================================

            blade.saveChanges = function () {
                try {
                    var doc = JSON.parse(blade.currentJson);
                    var settingsToSave = doc.settings || doc;

                    if (isInMemoryMode) {
                        // In-memory mode: pass parsed settings back to parent via callback
                        if (blade.onSaveJson) {
                            blade.onSaveJson(settingsToSave);
                        }
                        blade.origJson = blade.currentJson;
                        return;
                    }

                    // API mode: save with replaceAll=true
                    blade.isLoading = true;
                    var savePromise;
                    if (blade.tenantType && blade.tenantId) {
                        savePromise = settingsV2.saveTenantValues(
                            { tenantType: blade.tenantType, tenantId: blade.tenantId, replaceAll: true },
                            settingsToSave).$promise;
                    } else {
                        savePromise = settingsV2.saveGlobalValues({ replaceAll: true }, settingsToSave).$promise;
                    }

                    savePromise.then(function () {
                        blade.origJson = blade.currentJson;
                        blade.isLoading = false;
                        if (blade.parentRefresh) {
                            blade.parentRefresh();
                        }
                    }, function (error) {
                        blade.isLoading = false;
                        bladeNavigationService.setError('Error: ' + (error.data ? error.data.message : error.status), blade);
                    });
                } catch (err) {
                    dialogService.showNotificationDialog({
                        id: 'jsonParseError',
                        title: 'JSON Error',
                        message: 'Invalid JSON: ' + err.message
                    });
                }
            };

            function isDirty() {
                return blade.currentJson !== blade.origJson && blade.hasUpdatePermission();
            }

            function canSave() {
                if (!isDirty()) {
                    return false;
                }
                try {
                    JSON.parse(blade.currentJson);
                    return true;
                } catch (e) {
                    return false;
                }
            }

            function formatJson() {
                try {
                    var parsed = JSON.parse(blade.currentJson);
                    blade.currentJson = JSON.stringify(parsed, null, 2);
                } catch (e) {
                    dialogService.showNotificationDialog({
                        id: 'jsonFormatError',
                        title: 'JSON Error',
                        message: 'Cannot format: ' + e.message
                    });
                }
            }

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
                        blade.currentJson = angular.copy(blade.origJson);
                    },
                    canExecuteMethod: isDirty
                }
            ];

            blade.onClose = function (closeCallback) {
                bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.saveChanges, closeCallback,
                    'platform.dialogs.settings-delete.title', 'platform.dialogs.settings-delete.message');
            };

            blade.refresh();
        }
    ]);
