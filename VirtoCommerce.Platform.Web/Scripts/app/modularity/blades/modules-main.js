angular.module('platformWebApp')
    .controller('platformWebApp.modulesMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', 'platformWebApp.moduleHelper', function ($scope, bladeNavigationService, modules, moduleHelper) {
        var blade = $scope.blade;
        var nodeUpdate, nodeAvailable, nodeInstalled;
        $scope.selectedNodeId = null;

        blade.reload = function () {
            modules.reload().$promise.then(blade.refresh);
        };

        blade.refresh = function () {
            blade.isLoading = true;

            return modules.query().$promise.then(function (results) {
                moduleHelper.moduleBundles = [];
                moduleHelper.allmodules = results;
                _.each(results, function (x) {
                    x.description = x.description || '';
                    x.tags = x.tags || '';
                    if (x.tags) {
                        x.tagsArray = x.tags.split(' ');
                    }
                    if (x.owners) {
                        x.$owner = x.owners.join(', ');
                    }
                    x.$isOwnedByVirto = _.contains(x.owners, 'Virto Commerce');
                });

                var newResults = [];
                var grouped = _.groupBy(results, 'id');
                _.each(grouped, function (vals, key) {
                    var latest = _.last(vals);
                    newResults.push(latest);

                    // pre-calculate $alternativeVersion: set latest OR installed version here
                    var foundInstalledModule;
                    if (foundInstalledModule = _.findWhere(vals, { isInstalled: true })) {
                        _.each(vals, function (m) {
                            if (m === foundInstalledModule) {
                                if (m !== latest)
                                    m.$alternativeVersion = latest.version;
                            } else {
                                m.$alternativeVersion = foundInstalledModule.version;
                            }
                        });
                    }

                    // prepare bundled (grouped) data source of available modules
                    if (!latest.isInstalled && !latest.$alternativeVersion) {
                        if (_.any(latest.groups)) {
                            _.each(latest.groups, function (x, index) {
                                var clone = angular.copy(latest);
                                clone.$group = x;
                                moduleHelper.moduleBundles.push(clone);
                            });
                        } else {
                            var clone = angular.copy(latest);
                            clone.$group = 'platform.blades.modules-list.labels.ungrouped';
                            moduleHelper.moduleBundles.push(clone);
                        }
                    }
                });

                nodeUpdate.entities = _.filter(newResults, function (x) { return !x.isInstalled && x.$alternativeVersion; });
                nodeAvailable.entities = moduleHelper.availableModules = _.filter(newResults, function (x) { return !x.isInstalled && !x.$alternativeVersion; });
                nodeInstalled.entities = _.where(results, { isInstalled: true });
                nodeWithErrors.entities = _.filter(results, function (x) { return x.isInstalled && _.any(x.validationErrors); });
                if (_.any(nodeWithErrors.entities) && !nodeWithErrors.isAddedToList) {
                    nodeWithErrors.isAddedToList = true;
                    blade.currentEntities.splice(3, 0, nodeWithErrors);
                }

                openFirstBladeInitially();
                blade.isLoading = false;

                // return results for current child list
                return _.findWhere(blade.currentEntities, { mode: $scope.selectedNodeId }).entities;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        blade.openBlade = function (data) {
            $scope.selectedNodeId = data.mode;

            var newBlade = {
                id: 'modulesList',
                mode: data.mode,
                currentEntities: data.entities,
                title: data.name,
                subtitle: 'platform.blades.modules-list.subtitle',
                controller: 'platformWebApp.modulesListController',
                template: '$(Platform)/Scripts/app/modularity/blades/modules-list.tpl.html'
            };

            if (data.mode === 'withErrors') {
                angular.extend(newBlade, {
                    mode: 'installed'
                });
            }

            if (data.mode === 'advanced') {
                angular.extend(newBlade, {
                    controller: 'platformWebApp.moduleDetailController',
                    template: '$(Platform)/Scripts/app/modularity/blades/module-detail.tpl.html'
                });
            }

            bladeNavigationService.showBlade(newBlade, blade);
        };

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.reload,
                canExecuteMethod: function () { return true; }
            }
        ];

        blade.headIcon = 'fa-cubes';

        blade.currentEntities = [
            nodeUpdate = { name: 'platform.blades.modules-main.labels.updates', mode: 'update' },
            nodeAvailable = { name: 'platform.blades.modules-main.labels.available', mode: 'available' },
            nodeInstalled = { name: 'platform.blades.modules-main.labels.installed', mode: 'installed' },
            { name: 'platform.blades.modules-main.labels.advanced', mode: 'advanced' }
        ];

        var nodeWithErrors = { name: 'platform.blades.modules-main.labels.withErrors', mode: 'withErrors' };

        var openFirstBladeInitially = _.once(function () { blade.openBlade(blade.currentEntities[0]); });

        blade.refresh();
    }]);
