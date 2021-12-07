angular.module('platformWebApp')
    .controller('platformWebApp.settingsDetailThemeLogoController', ['$scope', '$rootScope', '$q', '$http', 'FileUploader', 'platformWebApp.settings.helper', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings',
        function ($scope, $rootScope, $q, $http, FileUploader, settingsHelper, bladeNavigationService, settingsApi) {
            var blade = $scope.blade;
            blade.updatePermission = 'platform:setting:update';

            if (!$scope.defaultLogoUploader) {
                var defaultLogoUploader = $scope.defaultLogoUploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    autoUpload: true,
                    removeAfterUpload: true
                });

                defaultLogoUploader.url = 'api/assets?folderUrl=customization';

                defaultLogoUploader.onSuccessItem = function (fileItem, images, status, headers) {
                    angular.forEach(images, function (image) {
                        blade.currentEntity.topPanelLogo_default.url = image.url;
                    });
                };

                defaultLogoUploader.onErrorItem = function (element, response, status, headers) {
                    bladeNavigationService.setError(element._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };
            }

            if (!$scope.miniLogoUploader) {
                var miniLogoUploader = $scope.miniLogoUploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    autoUpload: true,
                    removeAfterUpload: true
                });

                miniLogoUploader.url = 'api/assets?folderUrl=customization';

                miniLogoUploader.onSuccessItem = function (fileItem, images, status, headers) {
                    angular.forEach(images, function (image) {
                        blade.currentEntity.topPanelLogo_mini.url = image.url;
                    });
                };

                miniLogoUploader.onAfterAddingFile = function(item) {
                    const fileExtension = '.' + item.file.name.split('.').pop();
                    item.file.name = "topPanelLogo_mini" + fileExtension;
                };

                miniLogoUploader.onErrorItem = function (element, response, status, headers) {
                    bladeNavigationService.setError(element._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };
            }

            blade.refresh = function () {
                initializeBlade(angular.copy(blade.data));
            }

            function initializeBlade(settings) {
                blade.isLoading = true;
                var setting = _.first(settings);
                Object.assign(blade, setting.settingValues);
                if (setting.groupName) {
                    var paths = setting.groupName.split('|');
                    blade.groupName = paths.pop();
                }

                settings = _.groupBy(settings, 'groupName');
                blade.groupNames = _.keys(settings);

                settingsApi.getUiCustomizationSetting(function (uiCustomizationSetting) {
                    blade.isLoading = false;

                    blade.uiCustomizationSetting = uiCustomizationSetting;

                    var value = uiCustomizationSetting.value || uiCustomizationSetting.defaultValue;
                    if (value) {
                        var uiCustomization = angular.fromJson(value);
                        if (uiCustomization.topPanelLogo_default && uiCustomization.topPanelLogo_default.url) {
                            isImageExists(uiCustomization.topPanelLogo_default.url).then((defaultLogoIsExists) => {
                                uiCustomization.topPanelLogo_default.url = defaultLogoIsExists
                                    ? uiCustomization.topPanelLogo_default.url
                                    : blade.defaultUiCustomization.topPanelLogo_default.url;
                                blade.currentEntity = angular.copy(uiCustomization);
                                blade.origEntity = uiCustomization;
                            });
                        }
                        else {
                            uiCustomization.topPanelLogo_default = {
                                url: blade.defaultUiCustomization.topPanelLogo_default.url,
                            };
                            blade.currentEntity = angular.copy(uiCustomization);
                            blade.origEntity = uiCustomization;
                        }
                        if (uiCustomization.topPanelLogo_mini && uiCustomization.topPanelLogo_mini.url) {
                            isImageExists(uiCustomization.topPanelLogo_mini.url).then((miniLogoIsExists) => {
                                uiCustomization.topPanelLogo_mini.url = miniLogoIsExists
                                    ? uiCustomization.topPanelLogo_mini.url
                                    : blade.defaultUiCustomization.topPanelLogo_mini.url;
                                blade.currentEntity = angular.copy(uiCustomization);
                                blade.origEntity = uiCustomization;
                            });
                        }
                        else {
                            uiCustomization.topPanelLogo_mini = {
                                url: blade.defaultUiCustomization.topPanelLogo_mini.url,
                            };
                            blade.currentEntity = angular.copy(uiCustomization);
                            blade.origEntity = uiCustomization;
                        }
                    }
                });
            }

            var formScope;
            $scope.setForm = function (form) { formScope = form; }

            async function isImageExists(image_url) {
                var deferred = $q.defer();
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

                var deferred = $q.defer();

                blade.uiCustomizationSetting.value = blade.currentEntity;
                var objects = [angular.copy(blade.uiCustomizationSetting)];
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

            blade.headIcon = 'fa fa-wrench';
            blade.toolbarCommands = [
                {
                    name: "platform.commands.save", icon: 'fas fa-save',
                    executeMethod: blade.saveChanges,
                    canExecuteMethod: canSave
                },
                {
                    name: "platform.commands.reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        blade.currentEntity.topPanelLogo_default.url = '/images/logo.svg';
                        blade.currentEntity.topPanelLogo_mini.url = '/images/logo-small.svg';
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
