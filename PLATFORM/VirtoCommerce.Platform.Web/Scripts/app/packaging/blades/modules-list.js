angular.module('platformWebApp')
.controller('platformWebApp.modulesListController', ['$scope', 'filterFilter', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.modules', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, filterFilter, bladeNavigationService, dialogService, modules, uiGridConstants, uiGridHelper) {
        $scope.uiGridConstants = uiGridConstants;
        var blade = $scope.blade;

        blade.refresh = function () {
            blade.isLoading = true;

            modules.getModules({}, function (results) {
                blade.isLoading = false;
                blade.currentEntities = results;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        blade.selectNode = function (id) {
            $scope.selectedNodeId = id;

            var newBlade = {
                id: 'moduleDetails',
                title: 'platform.blades.module-detail.title',
                currentEntityId: id,
                controller: 'platformWebApp.moduleDetailController',
                template: '$(Platform)/Scripts/app/packaging/blades/module-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, blade);
        }

        function closeChildrenBlades() {
            angular.forEach(blade.childrenBlades.slice(), function (child) {
                bladeNavigationService.closeBlade(child);
            });
        }

        blade.headIcon = 'fa-cubes';

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () {
                    return true;
                }
            },
            {
                name: "platform.commands.install", icon: 'fa fa-plus',
                executeMethod: function () {
                    openAddEntityBlade();
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'platform:module:manage'
            }
        ];

        function openAddEntityBlade() {
            closeChildrenBlades();

            var newBlade = {
                id: "moduleWizard",
                title: "platform.blades.module-detail.title-install",
                // subtitle: '',
                mode: 'install',
                controller: 'platformWebApp.installWizardController',
                template: '$(Platform)/Scripts/app/packaging/blades/module-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions,
            function (gridApi) {
                gridApi.grid.registerRowsProcessor($scope.singleFilter, 90);
            });
        };

        $scope.singleFilter = function (renderableRows) {
            //var matcher = new RegExp(blade.searchText);
            var visibleCount = 0;
            renderableRows.forEach(function (row) {
                row.visible = _.any(filterFilter([row.entity], blade.searchText));
                if (row.visible) visibleCount++;
                //var match = false;
                //['title', 'version', 'authors'].forEach(function (field) {                    
                //    if (row.entity[field].match(matcher)) {
                //        match = true;
                //    }
                //});
                //if (!match) {
                //    row.visible = false;
                //}
            });

            $scope.filteredEntitiesCount = visibleCount;
            return renderableRows;
        };


        blade.refresh();
    }]);
