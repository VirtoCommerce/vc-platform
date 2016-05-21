angular.module('platformWebApp')
.controller('platformWebApp.assets.assetListController', ['$scope', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', '$sessionStorage', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
    function ($scope, assets, bladeNavigationService, dialogService, $storage, bladeUtils, uiGridHelper) {
        var blade = $scope.blade;
        blade.title = 'platform.blades.asset-list.title';
        if (!blade.currentEntity) {
            blade.currentEntity = {};
        }

        blade.refresh = function () {
            blade.isLoading = true;
            assets.query(
                {
                    keyword: blade.searchKeyword,
                    folderUrl: blade.currentEntity.url
                },
            function (data) {
                $scope.pageSettings.totalItems = data.length;
                _.each(data, function (x) { x.isImage = x.contentType && x.contentType.startsWith('image/'); });
                $scope.listEntries = data;
                blade.isLoading = false;

                //Set navigation breadcrumbs
                setBreadcrumbs();
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

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

        function newFolder(value, prefix) {
            var result = prompt(prefix ? prefix + "\n\nEnter folder name:" : "Enter folder name:", value);
            if (result != null) {
                if (blade.currentEntity.url) {
                    assets.createFolder({ name: result, parentUrl: blade.currentEntity.url },
                            blade.refresh,
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                } else {
                    if (result.length < 3 || result.length > 63 || !result.match(/^[a-z0-9]+(-[a-z0-9]+)*$/)) {
                        newFolder(result, "A folder name must conform to the following naming rules:\n  Folder name must be from 3 through 63 characters long.\n  Folder name must start with a letter or number, and can contain only letters, numbers, and the dash (-) character.\n  Every dash (-) character must be immediately preceded and followed by a letter or number; consecutive dashes are not permitted.\n  All letters in a folder name must be lowercase.");
                    } else {
                        assets.createFolder({ name: result, parentUrl: blade.currentEntity.url },
                            blade.refresh,
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            }
        }

        $scope.copyUrl = function (data) {
            window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
        };

        $scope.downloadUrl = function (data) {
            window.open(data.url, '_blank');
        };

        //$scope.rename = function (listItem) {
        //    rename(listItem);
        //};

        //function rename(listItem) {
        //    var result = prompt("Enter new name", listItem.name);
        //    if (result) {
        //        listItem.name = result;
        //    }
        //}

        function isItemsChecked() {
            return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
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
                            assets.remove({ urls: listEntryIds },
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
                    breadcrumbs: blade.breadcrumbs,
                    currentEntity: listItem,
                    disableOpenAnimation: true,
                    controller: blade.controller,
                    template: blade.template,
                    isClosingDisabled: blade.isClosingDisabled
                };

                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            }
        };

        blade.headIcon = 'fa-folder-o';

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
                executeMethod: function () { newFolder(); },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'platform:asset:create'
            },
            {
                name: "platform.commands.upload", icon: 'fa fa-upload',
                executeMethod: function () {
                    var newBlade = {
                        id: "assetUpload",
                        currentEntityId: blade.currentEntity.url,
                        title: 'platform.blades.asset-upload.title',
                        controller: 'platformWebApp.assets.assetUploadController',
                        template: '$(Platform)/Scripts/app/assets/blades/asset-upload.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'platform:asset:create'
            },
            //{
            //    name: "Rename", icon: 'fa fa-font',
            //    executeMethod: function () {
            //        rename(getFirstChecked())
            //    },
            //    canExecuteMethod: isSingleChecked,
            //    permission: 'platform:asset:update'
            //},
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
                canExecuteMethod: isItemsChecked,
                permission: 'platform:asset:delete'
            }
            //{
            //    name: "Cut",
            //    icon: 'fa fa-cut',
            //    executeMethod: function () {
            //    },
            //    canExecuteMethod: isItemsChecked,
            //    permission: 'asset:delete'
            //},
            //{
            //    name: "Paste",
            //    icon: 'fa fa-clipboard',
            //    executeMethod: function () {
            //        blade.isLoading = true;
            //        assets.move({
            //            folder: blade.currentEntity.url,
            //            listEntries: $storage.catalogClipboardContent
            //        }, function () {
            //            delete $storage.catalogClipboardContent;
            //            blade.refresh();
            //        }, function (error) {
            //            bladeNavigationService.setError('Error ' + error.status, blade);
            //        });
            //    },
            //    canExecuteMethod: function () {
            //        return $storage.catalogClipboardContent;
            //    },
            //    permission: 'asset:delete'
            //}
        ];

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions,
            function (gridApi) {
                $scope.$watch('pageSettings.currentPage', gridApi.pagination.seek);
            });
        };
        bladeUtils.initializePagination($scope, true);

        blade.refresh();
    }]);
