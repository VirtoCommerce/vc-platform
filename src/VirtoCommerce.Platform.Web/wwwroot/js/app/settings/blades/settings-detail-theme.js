angular.module('platformWebApp')
    .controller('platformWebApp.settingsDetailThemeController', ['$scope', '$rootScope', '$q', 'FileUploader', 'platformWebApp.settings.helper', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings',
        function ($scope, $rootScope, $q, FileUploader, settingsHelper, bladeNavigationService, settingsApi) {
            var blade = $scope.blade;
            blade.updatePermission = 'platform:setting:update';

            if (!$scope.uploader) {
                var uploader = $scope.uploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    autoUpload: true,
                    removeAfterUpload: true
                });

                uploader.url = 'api/assets?folderUrl=customizatinon';

                uploader.onSuccessItem = function (fileItem, images, status, headers) {
                    angular.forEach(images, function (image) {
                        blade.currentEntity.background.url = image.url;
                    });
                };

                uploader.onErrorItem = function (element, response, status, headers) {
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
                        uiCustomization.background = uiCustomization.background ? uiCustomization.background : blade.defaultUiCustomization.background;
                        uiCustomization.pattern = uiCustomization.pattern ? uiCustomization.pattern : blade.defaultUiCustomization.pattern;

                        blade.currentEntity = angular.copy(uiCustomization);
                        blade.origEntity = uiCustomization;
                    }
                });
            }

            var formScope;
            $scope.setForm = function (form) { formScope = form; }

            function isDirty() {
                return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
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
                        blade.currentEntity = angular.copy(blade.origEntity);
                    },
                    canExecuteMethod: isDirty
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
