angular.module('platformWebApp')
.controller('virtoCommerce.contentModule.assetListController', ['$scope', 'virtoCommerce.contentModule.contentApi', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeUtils',
    function ($scope, contentApi, bladeNavigationService, dialogService, uiGridHelper, bladeUtils) {
        var blade = $scope.blade;

        blade.refresh = function () {
            blade.isLoading = true;
            contentApi.query(
                {
                    contentType: blade.contentType,
                    storeId: blade.storeId,
                    keyword: blade.searchKeyword,
                    folderUrl: blade.currentEntity.url
                },
            function (data) {
                $scope.pageSettings.totalItems = data.length;
                _.each(data, function (x) {
                    x.isImage = x.mimeType && x.mimeType.startsWith('image/');
                    x.isOpenable = x.mimeType && (x.mimeType.startsWith('application/j') || x.mimeType.startsWith('text/'));
                });
                $scope.listEntries = data;
                blade.isLoading = false;

                //Set navigation breadcrumbs
                setBreadcrumbs();
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        function newFolder(value, prefix) {
            var result = prompt(prefix ? prefix + "\n\nEnter folder name:" : "Enter folder name:", value);
            if (result != null) {
                contentApi.createFolder(
                        { contentType: blade.contentType, storeId: blade.storeId },
                        { name: result, parentUrl: blade.currentEntity.url },
                        blade.refresh,
                        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
        }

        $scope.copyUrl = function (data) {
            window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
        };

        $scope.downloadUrl = function (data) {
            window.open(data.url, '_blank');
        };

        $scope.rename = function (listItem) {
            var result = prompt("Enter new name", listItem.name);
            if (result) {
                contentApi.move({
                    contentType: blade.contentType,
                    storeId: blade.storeId,
                    oldUrl: listItem.url,
                    newUrl: listItem.url.substring(0, listItem.url.length - listItem.name.length) + result
                }, blade.refresh,
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
        };

        function isItemsChecked() {
            return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
        }

        function isSingleChecked() {
            return isItemsChecked() && $scope.gridApi.selection.getSelectedRows().length === 1;
        }

        $scope.delete = function (data) {
            deleteList([data]);
        };

        function deleteList(selection) {
            bladeNavigationService.closeChildrenBlades(blade, function () {
                var dialog = {
                    id: "confirmDeleteItem",
                    title: "platform.dialogs.folders-delete.title",
                    message: "platform.dialogs.folders-delete.message",
                    callback: function (remove) {
                        if (remove) {
                            var listEntryIds = _.pluck(selection, 'url');
                            contentApi.delete({
                                contentType: blade.contentType,
                                storeId: blade.storeId,
                                urls: listEntryIds
                            },
                                blade.refresh,
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                        }
                    }
                }
                dialogService.showConfirmationDialog(dialog);
            });
        }

        $scope.selectNode = function (listItem) {
            if (listItem.type === 'folder') {
                var newBlade = {
                    id: blade.id,
                    contentType: blade.contentType,
                    storeId: blade.storeId,
                    themeId: blade.themeId,
                    currentEntity: listItem,
                    breadcrumbs: blade.breadcrumbs,
                    controller: blade.controller,
                    template: blade.template,
                    disableOpenAnimation: true,
                    isClosingDisabled: blade.isClosingDisabled
                };
                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            } else {
                blade.selectedNodeId = listItem.url;
                openDetailsBlade(listItem, false);
            }
        };

        function openDetailsBlade(listItem, isNew) {
            if (isNew || listItem.isOpenable) {
                var newBlade = {
                    id: 'assetDetail',
                    contentType: blade.contentType,
                    storeId: blade.storeId,
                    themeId: blade.themeId,
                    folderUrl: blade.currentEntity.url,
                    currentEntity: listItem,
                    isNew: isNew,
                    title: listItem.name,
                    subtitle: 'content.blades.edit-asset.subtitle',
                    subtitleValues: { name: listItem.name },
                    controller: 'virtoCommerce.contentModule.assetDetailController',
                    template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/asset-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            }
        }

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () {
                    return true;
                }
            },
            {
                name: "platform.commands.new-folder", icon: 'fa fa-folder-o',
                executeMethod: function () { newFolder(undefined); },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            },
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: function () {
                    openDetailsBlade({}, true);
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'content:create'
            },
            {
                name: "platform.commands.upload", icon: 'fa fa-upload',
                executeMethod: function () {
                    var newBlade = {
                        id: "assetUpload",
                        contentType: blade.contentType,
                        storeId: blade.storeId,
                        currentEntityId: blade.currentEntity.url,
                        title: 'platform.blades.asset-upload.title',
                        controller: 'virtoCommerce.contentModule.assetUploadController',
                        template: '$(Platform)/Scripts/app/assets/blades/asset-upload.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'content:create'
            },
            //{
            //    name: "Rename", icon: 'fa fa-font',
            //    executeMethod: function () {
            //        $scope.rename($scope.gridApi.selection.getSelectedRows()[0]);
            //    },
            //    canExecuteMethod: isSingleChecked,
            //    permission: 'content:update'
            //},
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
                canExecuteMethod: isItemsChecked,
                permission: 'content:delete'
            }
        ];

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions,
            function (gridApi) {
                $scope.$watch('pageSettings.currentPage', gridApi.pagination.seek);
            });
        };
        bladeUtils.initializePagination($scope, true);

        //Breadcrumbs
        function setBreadcrumbs() {
            if (blade.breadcrumbs) {
                //Clone array (angular.copy leaves the same reference)
                var breadcrumbs = blade.breadcrumbs.slice(0);

                //prevent duplicate items
                if (blade.currentEntity.url && _.all(breadcrumbs, function (x) { return x.id !== blade.currentEntity.url; })) {
                    var breadCrumb = generateBreadcrumb(blade.currentEntity.url, blade.currentEntity.name);
                    breadcrumbs.push(breadCrumb);
                }
                blade.breadcrumbs = breadcrumbs;
            } else {
                blade.breadcrumbs = [generateBreadcrumb(blade.currentEntity.url, 'all')];
            }
        }

        function generateBreadcrumb(id, name) {
            return {
                id: id,
                name: name,
                blade: blade,
                navigate: function (breadcrumb) {
                    breadcrumb.blade.searchKeyword = null;
                    breadcrumb.blade.disableOpenAnimation = true;
                    bladeNavigationService.showBlade(breadcrumb.blade, breadcrumb.blade.parentBlade);
                    // breadcrumb.blade.refresh();
                }
            }
        }

        blade.headIcon = 'fa-folder-o';
        blade.title = blade.currentEntity.name;
        blade.refresh();
    }
]);
