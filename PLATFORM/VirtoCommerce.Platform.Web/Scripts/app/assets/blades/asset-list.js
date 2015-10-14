angular.module('platformWebApp')
.controller('platformWebApp.assets.assetListController', ['$scope', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', '$sessionStorage', function ($scope, assets, bladeNavigationService, dialogService, $storage) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    var selectedNode = null;
    var preventFolderListingOnce; // prevent from unwanted additional actions after command was activated from context menu

    var blade = $scope.blade;
    blade.currentEntity = {};
    blade.title = 'Assets';

    blade.refresh = function () {
        //blade.isLoading = true;
        //assets.search(
        //    {
        //        keyword: $scope.filter.searchKeyword,
        //        folder: blade.currentEntity.id,
        //        start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
        //        count: $scope.pageSettings.itemsPerPageCount
        //    },
        //function (data) {
        blade.isLoading = false;
        $scope.listEntries = [
            { id: 'folder1', assetType: 'Folder', name: '151014094211-shakshuka-tease-medium-plus-169.jpg', lastModified: new Date() },
            { id: 'file1', assetType: 'File', name: '151014094211-shakshuka-tease-medium-plus-169.jpg', url: 'http://i2.cdn.turner.com/cnnnext/dam/assets/151014094211-shakshuka-tease-medium-plus-169.jpg', mimeType: 'image/jpeg', isImage: true, size: '23151', lastModified: new Date() }
        ];
        //    $scope.listEntries = data.assets;
        //    $scope.pageSettings.totalItems = data.totalCount;
        //    $scope.pageSettings.selectedAll = false;

        //    if (selectedNode != null) {
        //        //select the node in the new list
        //        angular.forEach($scope.listEntries, function (node) {
        //            if (selectedNode.id === node.id) {
        //                selectedNode = node;
        //            }
        //        });
        //    }

        //    //Set navigation breadcrumbs
        setBreadcrumbs();
        //}, function (error) {
        //    bladeNavigationService.setError('Error ' + error.status, blade);
        //});        
    };

    //Breadcrumbs
    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            //Clone array (angular.copy leaves the same reference)
            var breadcrumbs = blade.breadcrumbs.slice(0);

            //prevent duplicate items
            if (_.all(breadcrumbs, function (x) { return x.id !== blade.currentEntity.id; })) {
                var breadCrumb = generateBreadcrumb(blade.currentEntity.id, blade.currentEntity.displayName);
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
                breadcrumb.blade.disableOpenAnimation = true;
                bladeNavigationService.showBlade(breadcrumb.blade);
                breadcrumb.blade.refresh();
            }
        }
    }

    $scope.copyUrl = function (data) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
    };

    $scope.rename = function (listItem) {
        if (listItem.assetType === 'Folder') {
            preventFolderListingOnce = true;
        }
        rename(listItem);
    };

    function rename(listItem) {
        var result = prompt("Enter new name", listItem.name);
        if (result) {
            listItem.name = result;
        }
    }

    function isItemsChecked() {
        return _.any($scope.listEntries, function (x) { return x.selected; });
    }

    function isSingleChecked() {
        return _.where($scope.listEntries, { selected: true }).length == 1;
    }

    function getFirstChecked() {
        return _.findWhere($scope.listEntries, { selected: true });
    }

    $scope.delete = function () {
        if (isItemsChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNothingSelected",
                title: "Message",
                message: "Nothing selected. Check some folders or files first."
            };
            dialogService.showNotificationDialog(dialog);
        }

        preventFolderListingOnce = true;
    };

    function deleteChecked() {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var dialog = {
                id: "confirmDeleteItem",
                title: "Delete confirmation",
                message: "Are you sure you want to delete selected folders or files?",
                callback: function (remove) {
                    if (remove) {
                        var selection = _.where($scope.listEntries, { selected: true });
                        var listEntryIds = _.pluck(selection, 'id');
                        assets.remove({ ids: listEntryIds },
                            blade.refresh,
                        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        });
    }

    blade.setSelectedNode = function (listItem) {
        selectedNode = listItem;
        $scope.selectedNodeId = selectedNode.id;
    };

    $scope.selectNode = function (listItem) {
        listItem.selected = !listItem.selected;
        blade.setSelectedNode(listItem);

        if (listItem.assetType === 'Folder') {
            if (preventFolderListingOnce) {
                preventFolderListingOnce = false;
            } else {
                var newBlade = {
                    id: 'assetList',
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
            executeMethod: function () {

            },
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
                    currentEntityId: blade.currentEntity.id,
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
        {
            name: "Download", icon: 'fa fa-download',
            executeMethod: function () {
                // getFirstChecked().url
            },
            canExecuteMethod: isSingleChecked
        },
        {
            name: "Copy link", icon: 'fa fa-link',
            executeMethod: function () {
                $scope.copyUrl(getFirstChecked())
            },
            canExecuteMethod: isSingleChecked
        },
        {
            name: "Rename", icon: 'fa fa-font',
            executeMethod: function () {
                rename(getFirstChecked())
            },
            canExecuteMethod: isSingleChecked,
            permission: 'asset:update'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: deleteChecked,
            canExecuteMethod: isItemsChecked,
            permission: 'asset:delete'
        },
        {
            name: "Cut",
            icon: 'fa fa-cut',
            executeMethod: function () {
                $storage.catalogClipboardContent = _.where($scope.items, { selected: true });
            },
            canExecuteMethod: isItemsChecked,
            permission: 'asset:delete'
        },
        {
            name: "Paste",
            icon: 'fa fa-clipboard',
            executeMethod: function () {
                blade.isLoading = true;
                assets.move({
                    folder: blade.currentEntity.id,
                    listEntries: $storage.catalogClipboardContent
                }, function () {
                    delete $storage.catalogClipboardContent;
                    blade.refresh();
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
            },
            canExecuteMethod: function () {
                return $storage.catalogClipboardContent;
            },
            permission: 'asset:delete'
        }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.listEntries, function (item) {
            item.selected = selected;
        });
    };

    $scope.$watch('pageSettings.currentPage', blade.refresh);

    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);
