angular.module('platformWebApp')
    .controller('platformWebApp.settingsDetailThemeLogoController', ['$scope', '$rootScope', '$q', '$http', 'FileUploader', 'platformWebApp.settings.helper', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.dialogService',
        function ($scope, $rootScope, $q, $http, FileUploader, settingsHelper, bladeNavigationService, settingsApi, dialogService) {
            let blade = $scope.blade;
            blade.updatePermission = 'platform:setting:update';

            if (!$scope.fullLogoUploader) {
                const fullLogoUploader = $scope.fullLogoUploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    autoUpload: true,
                    removeAfterUpload: true,
                    filters: [{
                        name: 'imageFilter',
                        fn: function (item) {
                            const approval = /^.*\.(png|gif|svg)$/.test(item.name);
                            if (!approval) {
                                const dialog = {
                                    title: "Filetype error",
                                    message: "Only PNG, GIF or SVG files are allowed.",
                                }
                                dialogService.showErrorDialog(dialog);
                            }
                            return approval;
                        }
                    }]
                });

                fullLogoUploader.url = 'api/assets?folderUrl=customization';

                fullLogoUploader.onSuccessItem = function (_, uploadedImages) {
                    blade.currentEntity.topPanelLogo_full_custom.url = uploadedImages[0].url;
                };

                fullLogoUploader.onErrorItem = function (element, response, status, _) {
                    bladeNavigationService.setError(element._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };
            }

            if (!$scope.miniLogoUploader) {
                const miniLogoUploader = $scope.miniLogoUploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    autoUpload: true,
                    removeAfterUpload: true,
                    filters: [{
                        name: 'imageFilter',
                        fn: function (item) {
                            const approval = /^.*\.(png|gif|svg)$/.test(item.name);
                            if (!approval) {
                                const dialog = {
                                    title: "Filetype error",
                                    message: "Only PNG, GIF or SVG files are allowed.",
                                }
                                dialogService.showErrorDialog(dialog);
                            }
                            return approval;
                        }
                    }]
                });

                miniLogoUploader.url = 'api/assets?folderUrl=customization';

                miniLogoUploader.onSuccessItem = function (_, uploadedImages) {
                    blade.currentEntity.topPanelLogo_mini_custom.url = uploadedImages[0].url;
                };

                miniLogoUploader.onErrorItem = function (element, response, status, headers) {
                    bladeNavigationService.setError(element._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };
            }

            blade.refresh = function () {
                initializeBlade();
            }

            function initializeBlade() {
                blade.isLoading = true;

                settingsApi.getUiCustomizationSetting(function (uiCustomizationSetting) {
                    blade.isLoading = false;
                    blade.uiCustomizationSetting = uiCustomizationSetting;
                    const value = uiCustomizationSetting.value || uiCustomizationSetting.defaultValue;
                    if (value) {
                        const uiCustomization = { ...$rootScope.uiCustomization, ...angular.fromJson(value) };
                        if (uiCustomization.topPanelLogo_full_custom && uiCustomization.topPanelLogo_full_custom.url) {
                            isImageExists(uiCustomization.topPanelLogo_full_custom.url).then((fullLogoIsExists) => {
                                if (!fullLogoIsExists) {
                                    uiCustomization.topPanelLogo_full_custom.url = "";
                                }
                                blade.currentEntity = angular.copy(uiCustomization);
                                blade.origEntity = uiCustomization;
                            });
                        }
                        else {
                            uiCustomization.topPanelLogo_full_custom = {
                                url: "",
                            };
                            blade.currentEntity = angular.copy(uiCustomization);
                            blade.origEntity = uiCustomization;
                        }
                        if (uiCustomization.topPanelLogo_mini_custom && uiCustomization.topPanelLogo_mini_custom.url) {
                            isImageExists(uiCustomization.topPanelLogo_mini_custom.url).then((miniLogoIsExists) => {
                                if (!miniLogoIsExists) {
                                    uiCustomization.topPanelLogo_mini_custom.url = "";
                                }
                                blade.currentEntity = angular.copy(uiCustomization);
                                blade.origEntity = uiCustomization;
                            });
                        }
                        else {
                            uiCustomization.topPanelLogo_mini_custom = {
                                url: "",
                            };
                            blade.currentEntity = angular.copy(uiCustomization);
                            blade.origEntity = uiCustomization;
                        }
                    }
                });
            }

            let formScope;
            $scope.setForm = function (form) { formScope = form; }

            function isImageExists(image_url) {
                let deferred = $q.defer();
                $http({ method: "GET", url: image_url }).then(
                    function success() {
                        deferred.resolve(true);
                    },
                    function error() {
                        deferred.resolve(false);
                    }
                );
                return deferred.promise;
            }

            function isDirty() {
                return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
            }

            function canResetToDefault() {
                return blade.hasUpdatePermission();
            }

            function canSave() {
                return isDirty() && formScope && formScope.$valid;
            }

            blade.saveChanges = function () {
                blade.isLoading = true;

                let deferred = $q.defer();

                blade.uiCustomizationSetting.value = blade.currentEntity;
                let objects = [angular.copy(blade.uiCustomizationSetting)];
                settingsHelper.toApiFormat(objects);

                // update UI customization setting
                settingsApi.update({}, objects, function () {
                    // update rootscope for seamless login page update
                    $rootScope.uiCustomization = blade.currentEntity;

                    blade.origEntity = blade.currentEntity;
                    blade.parentBlade.refresh(true);

                    deferred.resolve();
                });

                return deferred.promise;
            };

            blade.toolbarCommands = [
                {
                    name: "platform.commands.save", icon: 'fas fa-save',
                    executeMethod: blade.saveChanges,
                    canExecuteMethod: canSave
                },
                {
                    name: "platform.commands.set-to-default", icon: 'fa fa-undo',
                    executeMethod: function () {
                        blade.currentEntity.topPanelLogo_full_custom.url = "";
                        blade.currentEntity.topPanelLogo_mini_custom.url = "";
                    },
                    canExecuteMethod: canResetToDefault
                }
            ];

            blade.onClose = function (closeCallback) {
                bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.saveChanges, closeCallback, "platform.dialogs.settings-delete.title", "platform.dialogs.settings-delete.message");
            };

            $scope.getDictionaryValues = function (setting, callback) {
                callback(setting.allowedValues);
            };

            // actions on load
            blade.refresh();
        }]);
