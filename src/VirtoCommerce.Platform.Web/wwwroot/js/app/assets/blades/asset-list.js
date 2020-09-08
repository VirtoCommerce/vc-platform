angular.module('platformWebApp')
    .controller('platformWebApp.assets.assetListController', ['$scope', '$translate', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', '$sessionStorage', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
        function ($scope, $translate, assets, bladeNavigationService, dialogService, $storage, bladeUtils, uiGridHelper) {
            var blade = $scope.blade;
            blade.title = 'platform.blades.asset-list.title';
            if (!blade.currentEntity) {
                blade.currentEntity = {};
            }

            blade.refresh = function () {
                blade.isLoading = true;
                assets.search(
                    {
                        keyword: blade.searchKeyword,
                        folderUrl: blade.currentEntity.url
                    },
                    function (data) {
                        $scope.pageSettings.totalItems = data.totalCount;
                        _.each(data.results, function (x) {
                            x.isImage = x.contentType && x.contentType.startsWith('image/');
                            if (x.isImage) {
                                x.noCacheUrl = x.url + '?t=' + x.modifiedDate;
                            }
                        });
                        $scope.listEntries = data.results;
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
                        bladeNavigationService.closeBlade(blade,
                            function () {
                                blade.disableOpenAnimation = true;
                                bladeNavigationService.showBlade(blade, blade.parentBlade);
                            });
                    }
                }
            }

            function newFolder() {
                var tooltip = $translate.instant('platform.dialogs.create-folder.title');

                var result = prompt(tooltip + "\n\nEnter folder name:");

                if (result != null) {
                    assets.createFolder({ name: result, parentUrl: blade.currentEntity.url }, blade.refresh);
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
