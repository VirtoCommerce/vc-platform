angular.module('platformWebApp')
.controller('platformWebApp.modulesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.modules', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, bladeNavigationService, dialogService, modules, uiGridConstants, uiGridHelper) {
        var blade = $scope.blade;

        blade.refresh = function () {
            blade.isLoading = true;

            modules.getModules({}, function (results) {
                blade.isLoading = false;
                blade.currentEntities = results;
                uiGridHelper.onDataLoaded($scope.gridOptions, blade.currentEntities);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        blade.selectNode = function (id) {
            $scope.selectedNodeId = id;

            var newBlade = {
                id: 'moduleDetails',
                title: 'Module information',
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
                name: "Refresh", icon: 'fa fa-refresh',
                executeMethod: function () {
                    blade.refresh();
                },
                canExecuteMethod: function () {
                    return true;
                }
            },
            {
                name: "Install", icon: 'fa fa-plus',
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
                title: "Module install",
                // subtitle: '',
                mode: 'install',
                controller: 'platformWebApp.installWizardController',
                template: '$(Platform)/Scripts/app/packaging/blades/module-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }

        // ui-grid 
        uiGridHelper.initialize($scope, {
            rowTemplate: "modules-list.row.html",
            columnDefs: [
                        {
                            displayName: 'Icon', name: 'iconUrl',
                            cellTemplate: 'modules-list-icon.cell.html'                            
                        },
                        { displayName: 'Module', name: 'title' },
                        { name: 'version' },
                        {
                            displayName: 'Author', name: 'authors',
                            cellTemplate: 'modules-list-authors.cell.html'
                        }
            ]
        });
        

        blade.refresh();
    }]);
