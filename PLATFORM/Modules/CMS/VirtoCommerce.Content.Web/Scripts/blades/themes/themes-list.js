angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.themesListController', ['$rootScope', '$scope', 'virtoCommerce.contentModule.themes', 'virtoCommerce.contentModule.contentApi', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper',
    function ($rootScope, $scope, themes, contentApi, stores, bladeNavigationService, dialogService, uiGridHelper) {
        $scope.uiGridConstants = uiGridHelper.uiGridConstants;
        var blade = $scope.blade;
        blade.updatePermission = 'content:update';
        blade.contentType = 'themes';
        blade.defaultThemeName = undefined;

        blade.refresh = function () {
            blade.isLoading = true;
            $scope.selectedNodeId = undefined;
            contentApi.query({ contentType: blade.contentType, storeId: blade.storeId }, function (data) {
                blade.currentEntities = data;

                stores.get({ id: blade.storeId }, function (data) {
                    blade.store = data;
                    var prop = _.findWhere(blade.store.dynamicProperties, { name: 'DefaultThemeName' });
                    blade.defaultThemeName = prop && _.any(prop.values) && prop.values[0].value;

                    blade.isLoading = false;
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }

        $scope.openBladeNew = function () {
            $scope.openDetailsBlade();
        };

        $scope.openDetailsBlade = function (node) {
            $scope.selectedNodeId = node && node.name;

            var newBlade = {
                id: 'themeDetail',
                isNew: !node,
                isActivateAfterSave: !_.any(blade.currentEntities),
                store: blade.store,
                data: node,
                storeId: blade.storeId,
                baseThemes: blade.baseThemes,
                controller: 'virtoCommerce.contentModule.themeDetailController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/theme-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.clone = function (data) {
            $scope.selectedNodeId = data.name;
            var dialog = {
                id: "confirm",
                title: "content.dialogs.theme-clone.title",
                message: "content.dialogs.theme-clone.message",
                callback: function (userFonfirmed) {
                    if (userFonfirmed) {
                        blade.isLoading = true;
                        themes.cloneTheme({ storeId: blade.storeId, themeName: data.name },
                            blade.refresh,
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            };
            dialogService.showConfirmationDialog(dialog);
        };

        $scope.preview = function (data) {
            $scope.selectedNodeId = data.name;
            if (blade.store.url) {
                window.open(blade.store.url + '?previewtheme=' + data.name, '_blank');
            }
            else {
                var dialog = {
                    id: "noUrlInStore",
                    title: "content.dialogs.set-store-url.title",
                    message: "content.dialogs.set-store-url.message",
                    callback: function () { }
                }
                dialogService.showNotificationDialog(dialog);
            }
        };

        $scope.setActive = function (data) {
            $scope.selectedNodeId = data.name;
            blade.isLoading = true;

            var prop = _.findWhere(blade.store.dynamicProperties, { name: 'DefaultThemeName' });
            prop.values = [{ value: data.name }];

            blade.store.$update(function () {
                blade.refresh();
                blade.parentBlade.refresh(blade.storeId, 'defaultTheme', data.name);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        };

        $scope.delete = function (data) {
            $scope.selectedNodeId = data.name;
            bladeNavigationService.closeChildrenBlades(blade, function () {
                var dialog = {
                    id: "confirmDelete",
                    title: "content.dialogs.theme-delete.title",
                    message: blade.currentEntities.length > 1 ? "content.dialogs.theme-delete.message" : "content.dialogs.theme-delete.message-last-one",
                    messageValues: { name: data.name },
                    callback: function (remove) {
                        if (remove) {
                            blade.isLoading = true;
                            contentApi.delete({
                                contentType: blade.contentType,
                                storeId: blade.storeId,
                                urls: [data.url]
                            },
                            function () {
                                if (data.name === blade.defaultThemeName) {
                                    var prop = _.findWhere(blade.store.dynamicProperties, { name: 'DefaultThemeName' });
                                    prop.values = [{ value: '' }];

                                    blade.store.$update(function () {
                                        blade.refresh();
                                        $rootScope.$broadcast("cms-statistics-changed", blade.storeId);
                                    },
                                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                                } else {
                                    blade.refresh();
                                    $rootScope.$broadcast("cms-statistics-changed", blade.storeId);
                                }
                            },
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                        }
                    }
                };
                dialogService.showConfirmationDialog(dialog);
            });
        };

        blade.toolbarCommands = [
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: $scope.openBladeNew,
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'content:create'
            },
            {
                name: "platform.commands.upload", icon: 'fa fa-upload',
                executeMethod: function () {
                    var newBlade = {
                        id: "themeUpload",
                        isActivateAfterSave: !_.any(blade.currentEntities),
                        store: blade.store,
                        storeId: blade.storeId,
                        controller: 'virtoCommerce.contentModule.themeUploadController',
                        template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/theme-upload.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            },
            //{
            //    name: "platform.commands.clone", icon: 'fa fa-files-o',
            //    executeMethod: function () {
            //        $scope.clone(blade.chosenTheme);
            //    },
            //    canExecuteMethod: function () { return blade.chosenTheme; },
            //    permission: 'content:create'
            //},
            //{
            //    name: "content.commands.edit-css-html", icon: 'fa fa-code',
            //    executeMethod: function () {
            //        $scope.openDetailsBlade();
            //    },
            //    canExecuteMethod: function () {
            //        return !angular.isUndefined(blade.chosenTheme);
            //    },
            //    permission: blade.updatePermission
            //},
            //{
            //    name: "content.commands.preview-theme", icon: 'fa fa-eye',
            //    executeMethod: function () {
            //        $scope.preview(blade.chosenTheme);
            //    },
            //    canExecuteMethod: function () {
            //        return blade.chosenTheme;
            //    }
            //},
            //{
            //    name: "content.commands.set-active", icon: 'fa fa-pencil-square-o',
            //    executeMethod: function () {
            //        $scope.setActive(blade.chosenTheme);
            //    },
            //    canExecuteMethod: function () {
            //        return blade.chosenTheme && blade.defaultThemeName !== blade.chosenTheme.name;
            //    },
            //    permission: blade.updatePermission
            //},
            //{
            //    name: "content.commands.delete-theme", icon: 'fa fa-trash-o',
            //    executeMethod: function () {
            //        $scope.delete(blade.chosenTheme);
            //    },
            //    canExecuteMethod: function () {
            //        return blade.chosenTheme;
            //    },
            //    permission: 'content:delete'
            //}
        ];

        blade.selectNode = function (data) {
            $scope.selectedNodeId = data.name;

            var newBlade = {
                id: 'themeAssetListBlade',
                contentType: blade.contentType,
                storeId: blade.storeId,
                currentEntity: data,
                themeId: data.name,
                subtitle: 'content.blades.asset-list.subtitle',
                controller: 'virtoCommerce.contentModule.assetListController',
                template: '$(Platform)/Scripts/app/assets/blades/asset-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions);
        };

        blade.refresh();
    }
]);
