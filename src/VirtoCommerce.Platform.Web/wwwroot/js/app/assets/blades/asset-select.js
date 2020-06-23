angular.module('platformWebApp')
    .controller('platformWebApp.assets.assetSelectController', ['$scope', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
        function ($scope, assetsApi, bladeNavigationService, bladeUtils, uiGridHelper) {
            var blade = $scope.blade;
            blade.template = '$(Platform)/Scripts/app/assets/blades/asset-select.tpl.html';

            blade.headIcon = 'fa-folder-o';

            if (!blade.currentEntity) {
                blade.currentEntity = {};
            }
            if (blade.folder) {
                blade.currentEntity.url = '/' + blade.folder;
            }

            blade.refresh = function () {
                blade.isLoading = true;
                assetsApi.search(
                    {
                        keyword: blade.searchKeyword,
                        folderUrl: blade.currentEntity.url
                    },
                    function (data) {
                        $scope.pageSettings.totalItems = data.totalCount;
                        _.each(data.results, function (x) { x.isImage = x.contentType && x.contentType.startsWith('image/'); });
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
                    var name = "all";
                    if (blade.folder)
                        name = blade.folder;

                    blade.breadcrumbs = [generateBreadcrumb(blade.currentEntity.url, name)];
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

            function isItemsChecked() {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            }

            function getSelectedAssets() {
                return $scope.gridApi.selection.getSelectedRows();
            }

            $scope.selectNode = function (listItem) {
                if (listItem.type === 'folder') {
                    var newBlade = {
                        id: blade.id,
                        title: blade.title,
                        breadcrumbs: blade.breadcrumbs,
                        currentEntity: listItem,
                        disableOpenAnimation: true,
                        controller: blade.controller,
                        template: blade.template,
                        isClosingDisabled: blade.isClosingDisabled,
                        onSelect: blade.onSelect
                    };

                    bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                }
            };

            blade.toolbarCommands = [
                {
                    name: 'platform.commands.confirm',
                    icon: 'fa fa-check',
                    executeMethod: function () { $scope.saveChanges(); },
                    canExecuteMethod: function () {
                        return isItemsChecked();
                    }
                }
            ];

            $scope.saveChanges = function () {
                if (blade.onSelect)
                    blade.onSelect(getSelectedAssets());

                $scope.bladeClose();
            };

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
