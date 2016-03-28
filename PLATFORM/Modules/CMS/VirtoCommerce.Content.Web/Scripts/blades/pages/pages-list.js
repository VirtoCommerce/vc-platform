angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pagesListController', ['$scope', 'virtoCommerce.contentModule.contentApi', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeUtils', function ($scope, contentApi, bladeNavigationService, dialogService, uiGridHelper, bladeUtils) {
    var blade = $scope.blade;
    blade.updatePermission = 'content:update';

    $scope.selectedNodeId = null;

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

    $scope.selectNode = function (listItem) {
        if (listItem.type === 'folder') {
            var newBlade = {
                id: blade.id,
                contentType: blade.contentType,
                storeId: blade.storeId,
                currentEntity: listItem,
                breadcrumbs: blade.breadcrumbs,
                title: blade.title,
                subtitle: blade.subtitle,
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
        var newBlade = {
            id: 'pageDetail',
            contentType: blade.contentType,
            storeId: blade.storeId,
            folderUrl: blade.currentEntity.url,
            currentEntity: listItem,
            isNew: isNew,
            title: listItem.name,
            subtitle: 'content.blades.edit-page.subtitle',
            controller: 'virtoCommerce.contentModule.pageDetailController',
            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/page-detail.tpl.html'
        };

        if (isNew) {
            angular.extend(newBlade, {
                // currentEntity: { name: path + 'new_page.md', pageName: 'new_page', content: null, contentType: 'text/html', language: null, storeId: blade.storeId },
                title: 'content.blades.edit-page.title-new',
                subtitle: 'content.blades.edit-page.subtitle-new',
            });
        }

        //var newBlade = {
        //    body: body,
        //    metadata: metadata,
        //    title: 'content.blades.edit-page.title',
        //    titleValues: { name: data.name }            
        //};

        bladeNavigationService.showBlade(newBlade, blade);
    }

    function openBlogDetailsBlade(listItem, isNew) {
        var title = isNew ? '+++add' : 'Edit blog';
        var subTitle = isNew ? 'Create new blog' : 'Edit blog folder';

        var newBlade = {
            id: 'openBlogNew',
            contentType: blade.contentType,
            storeId: blade.storeId,
            folderUrl: blade.currentEntity.url,
            currentEntity: listItem,
            isNew: isNew,
            title: title,
            subtitle: subTitle,
            controller: 'virtoCommerce.contentModule.editBlogController',
            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-blog.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
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

    function isItemsChecked() {
        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
    }

    function isPages() {
        return blade.contentType === 'pages';
    }

    function isBlogs() {
        return blade.contentType === 'blogs';
    }

    blade.toolbarCommands = [
          {
              name: "platform.commands.refresh", icon: 'fa fa-refresh',
              executeMethod: blade.refresh,
              canExecuteMethod: function () {
                  return true;
              }
          }
    ];

    if (isPages()) {
        blade.toolbarCommands.splice(1, 0,
            {
                name: 'platform.commands.new-folder', icon: 'fa fa-folder-o',
                executeMethod: function () { newFolder(undefined); },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            },
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: function () { openDetailsBlade({}, true); },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            }
        );
    } else if (isBlogs()) {
        blade.toolbarCommands.splice(1, 0, {
            name: 'content.commands.add-blog', icon: 'fa fa-plus',
            executeMethod: function () { openBlogDetailsBlade({}, true); },
            canExecuteMethod: function () { return true; },
            permission: 'content:create'
        });
    }

    blade.toolbarCommands.push({
        name: "platform.commands.delete", icon: 'fa fa-trash-o',
        executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
        canExecuteMethod: isItemsChecked,
        permission: 'content:delete'
    });

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions,
        function (gridApi) {
            $scope.$watch('pageSettings.currentPage', gridApi.pagination.seek);
        });

        bladeUtils.initializePagination($scope, true);
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
            }
        }
    }

    blade.headIcon = 'fa-folder-o';
    blade.refresh();
}]);
