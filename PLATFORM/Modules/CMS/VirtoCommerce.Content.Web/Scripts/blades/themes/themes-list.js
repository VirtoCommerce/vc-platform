angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.themesListController', ['$scope', 'virtoCommerce.contentModule.themes', 'virtoCommerce.contentModule.contentApi', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper',
    function ($scope, themes, contentApi, stores, bladeNavigationService, dialogService, uiGridHelper) {
        $scope.uiGridConstants = uiGridHelper.uiGridConstants;
        var blade = $scope.blade;
        blade.updatePermission = 'content:update';
        blade.contentType = 'themes';
        blade.defaultThemeName = undefined;

        blade.initialize = function () {
            blade.isLoading = true;
            $scope.selectedNodeId = undefined;
            blade.chosenTheme = undefined;
            contentApi.query({ contentType: blade.contentType, storeId: blade.storeId }, function (data) {
                blade.currentEntities = data;

                stores.get({ id: blade.storeId }, function (data) {
                    blade.store = data;
                    if (_.find(blade.store.dynamicProperties, function (property) { return property.name === 'DefaultThemeName'; }) !== undefined) {
                        var defaultThemeNameProperty = _.find(blade.store.dynamicProperties, function (property) { return property.name === 'DefaultThemeName'; });

                        if (defaultThemeNameProperty !== undefined && defaultThemeNameProperty.values !== undefined && defaultThemeNameProperty.values.length > 0) {
                            blade.defaultThemeName = defaultThemeNameProperty.values[0].value;
                        }
                    }
                    blade.isLoading = false;
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }

        $scope.openBladeNew = function () {
            $scope.openDetailsBlade();
        };

        $scope.openDetailsBlade = function (node) {
            $scope.selectedNodeId = node && node.name;
            blade.chosenTheme = node;

            var newBlade = {
                id: 'themeDetail',
                isNew: !node,
                data: node,
                storeId: blade.storeId,
                baseThemes: blade.baseThemes,
                controller: 'virtoCommerce.contentModule.themeDetailController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/theme-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
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
                        themes.cloneTheme({ storeId: blade.storeId, themeName: data.name }, function (data) {
                            blade.initialize();
                        },
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
            if (_.where(blade.store.dynamicProperties, { name: "DefaultThemeName" }).length > 0) {
                angular.forEach(blade.store.dynamicProperties, function (value, key) {
                    if (value.name === "DefaultThemeName") {
                        value.values[0] = { value: data.name };
                    }
                });
            }

            stores.update({ storeId: blade.storeId }, blade.store, function (data) {
                blade.initialize();
                blade.parentBlade.refresh(blade.storeId, 'defaultTheme');

            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
                            function (data) {
                                blade.initialize();
                                blade.parentBlade.refresh(blade.storeId, 'defaultTheme');
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
            blade.chosenTheme = data;

            var newBlade = {
                id: 'themeAssetListBlade',
                contentType: blade.contentType,
                storeId: blade.storeId,
                currentEntity: data,
                themeId: data.name,
                // titleValues: { name: blade.chosenTheme.path },
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

        blade.initialize();
    }
]);
