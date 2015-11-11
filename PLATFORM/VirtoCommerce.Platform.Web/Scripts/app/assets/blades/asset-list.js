angular.module('platformWebApp')
.controller('platformWebApp.assets.assetListController', ['$scope', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', '$sessionStorage', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, assets, bladeNavigationService, dialogService, $storage, uiGridConstants, uiGridHelper) {
        var preventFolderListingOnce; // prevent from unwanted additional actions after command was activated from context menu

        var blade = $scope.blade;
        blade.title = 'Asset management';
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
                uiGridHelper.onDataLoaded($scope.gridOptions, data);
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
                blade.breadcrumbs = [generateBreadcrumb(null, 'all')];
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
                    bladeNavigationService.showBlade(breadcrumb.blade);
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
            if (data.type === 'folder') {
                preventFolderListingOnce = true;
            }
        };

        $scope.downloadUrl = function (data) {
            window.open(data.url, '_blank', '');
        };

        //$scope.rename = function (listItem) {
        //    if (listItem.type === 'folder') {
        //        preventFolderListingOnce = true;
        //    }
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

            preventFolderListingOnce = true;
        };

        function deleteList(selection) {
            bladeNavigationService.closeChildrenBlades(blade, function () {
                var dialog = {
                    id: "confirmDeleteItem",
                    title: "Delete confirmation",
                    message: "Are you sure you want to delete selected folders or files?",
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
                if (preventFolderListingOnce) {
                    preventFolderListingOnce = false;
                } else {
                    var newBlade = {
                        id: blade.id,
                        breadcrumbs: blade.breadcrumbs,
                        currentEntity: listItem,
                        disableOpenAnimation: true,
                        controller: blade.controller,
                        template: blade.template,
                        isClosingDisabled: true
                    };

                    bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                }
            }
        };

        blade.headIcon = 'fa-folder-o';

        blade.toolbarCommands = [
            {
                name: "Refresh", icon: 'fa fa-refresh',
                executeMethod: function () {
                    blade.refresh();
                },
                canExecuteMethod: function () {
                    return true;
                }
            },
            {
                name: "New folder", icon: 'fa fa-folder-o',
                executeMethod: function () { newFolder(); },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'asset:create'
            },
            {
                name: "Upload", icon: 'fa fa-upload',
                executeMethod: function () {
                    var newBlade = {
                        id: "assetUpload",
                        currentEntityId: blade.currentEntity.url,
                        title: 'Asset upload',
                        controller: 'platformWebApp.assets.assetUploadController',
                        template: '$(Platform)/Scripts/app/assets/blades/asset-upload.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'asset:create'
            },
            //{
            //    name: "Download", icon: 'fa fa-download',
            //    executeMethod: function () {
            //        $scope.downloadUrl(getFirstChecked());
            //    },
            //    canExecuteMethod: function () {
            //        return isSingleChecked() && getFirstChecked().type !== 'folder';
            //    }
            //},
            //{
            //    name: "Copy link", icon: 'fa fa-link',
            //    executeMethod: function () {
            //        $scope.copyUrl(getFirstChecked())
            //    },
            //    canExecuteMethod: isSingleChecked
            //},
            //{
            //    name: "Rename", icon: 'fa fa-font',
            //    executeMethod: function () {
            //        rename(getFirstChecked())
            //    },
            //    canExecuteMethod: isSingleChecked,
            //    permission: 'asset:update'
            //},
            {
                name: "Delete", icon: 'fa fa-trash-o',
                executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
                canExecuteMethod: isItemsChecked,
                permission: 'asset:delete'
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
        uiGridHelper.initialize($scope, {
            data: 'listEntries',
            rowTemplate: "<div ng-click=\"grid.appScope.selectNode(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.uid\" ui-grid-one-bind-id-grid=\"rowRenderIndex + '-' + col.uid + '-cell'\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" role=\"{{col.isRowHeader ? 'rowheader' : 'gridcell'}}\" ui-grid-cell style='cursor:pointer'></div>",
            rowHeight: 61,
            columnDefs: [
                        {
                            name: 'url', displayName: 'Pic',
                            enableColumnResizing: false, width: 60,
                            cellTemplate: 'asset-list-icon.cell.html'
                        },
                        { name: 'name', cellTooltip: true },
                        { name: 'size' },
                        { name: 'modifiedDate', displayName: 'Modified', cellTemplate: 'am-time-ago.cell.html' },
                        { name: 'actions', enableColumnResizing: false, enableSorting: false, width: 80, cellTemplate: 'asset-list-actions.cell.html' }
            ]
        });
        
        blade.refresh();
    }]);
