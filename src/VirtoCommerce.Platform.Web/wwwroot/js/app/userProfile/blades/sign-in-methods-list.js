angular.module('platformWebApp')
    .controller('platformWebApp.signInMethodsListController', ['$scope', '$timeout', 'platformWebApp.signInMethodService', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService',
        function ($scope, $timeout, signInMethodService, dialogService, bladeNavigationService) {
            const blade = $scope.blade;
            blade.title = 'platform.blades.sign-in-methods-list.title';
            blade.headIcon = 'fas fa-key';

            blade.refresh = function () {
                blade.isLoading = true;

                signInMethodService.getUserSignInMethods().then(function (signInMethods) {
                    blade.isLoading = false;
                    blade.currentEntities = _.sortBy(signInMethods, 'displayName');
                }, setError);
            };

            function setError(error) {
                bladeNavigationService.setError(error, blade);
            }

            $scope.getTitle = function (signInMethod) {
                return signInMethod.title || signInMethod.displayName;
            };

            $scope.createKey = function (signInMethod) {
                blade.isLoading = true;

                signInMethodService.createKey(signInMethod).then(blade.refresh, setError);
            };

            $scope.getKeyTemplateUrl = function (signInMethod) {
                return signInMethod.keyTemplateUrl || '$(Platform)/Scripts/app/userProfile/templates/sign-in-key.html';
            };

            $scope.rename = {};

            $scope.startRenameKey = function (key) {
                $scope.rename = { key: key, name: key.name };

                $timeout(function () {
                    $('#signInKeyName').focus();
                });
            };

            $scope.renameKeyOnKeyUp = function ($event, signInMethod) {
                if ($event.key === 'Enter') {
                    $scope.renameKey(signInMethod);
                } else if ($event.key === 'Escape') {
                    $scope.cancelRenameKey();
                }
            };

            $scope.renameKey = function (signInMethod) {
                const rename = $scope.rename;
                $scope.rename = {};

                signInMethodService.renameKey(signInMethod, rename.key, rename.name).then(blade.refresh, setError);
            };

            $scope.cancelRenameKey = function () {
                $scope.rename = {};
            };

            $scope.deleteKey = function (signInMethod, key) {
                const dialog = {
                    id: 'confirmDeleteSignInKey',
                    title: 'platform.dialogs.sign-in-key-delete.title',
                    message: 'platform.dialogs.sign-in-key-delete.message',
                    callback: function (remove) {
                        if (remove) {
                            signInMethodService.deleteKey(signInMethod, key).then(blade.refresh, setError);
                        }
                    }
                };

                dialogService.showConfirmationDialog(dialog);
            };

            $scope.getNoKeysTitle = function (signInMethod) {
                return signInMethod.noKeysTitle || 'platform.blades.sign-in-methods-list.labels.no-keys';
            };

            blade.toolbarCommands = [
                {
                    name: 'platform.commands.refresh', icon: 'fa fa-refresh',
                    executeMethod: blade.refresh,
                    canExecuteMethod: function () {
                        return true;
                    }
                }
            ];

            blade.refresh();
        }]);
