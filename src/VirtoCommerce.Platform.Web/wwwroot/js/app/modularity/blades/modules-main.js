angular.module('platformWebApp')
    .controller('platformWebApp.modulesMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', 'platformWebApp.moduleHelper', function ($scope, bladeNavigationService, modules, moduleHelper) {
        var blade = $scope.blade;
        var nodeUpdate, nodeExisting, nodeInstalled;
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

                    // pre-calculate $alternativeVersion: set latest OR installed version here
                    var foundInstalledModule;
                    if (foundInstalledModule = _.findWhere(vals, { isInstalled: true })) {
                        newResults.push(foundInstalledModule);
                        _.each(vals, function (m) {
                            if (m === foundInstalledModule) {
                                if (m !== latest) {
                                    // to display the new available version of the installed module
                                    m.$alternativeVersion = latest.version;
                                }
                            } else {
                                // It is necessary to detect the update on the module details page (the latest non-installed version contains the current installed version here)
                                m.$installedVersion = foundInstalledModule.version;
                            }
                        });
                    }
                    else {
                        newResults.push(latest);
                    }

                    // prepare bundled (grouped) data source of existing modules
                    var newLatest = newResults.at(-1);
                    if (_.any(newLatest.groups)) {
                        _.each(newLatest.groups, function (x, index) {
                            var groupClone = angular.copy(newLatest);
                            groupClone.$group = x;
                            moduleHelper.moduleBundles.push(groupClone);
                        });
                    } else {
                        var ungroupClone = angular.copy(newLatest);
                        ungroupClone.$group = 'platform.blades.modules-list.labels.ungrouped';
                        moduleHelper.moduleBundles.push(ungroupClone);
                    }
                });

                nodeExisting.entities = moduleHelper.existingModules = newResults;
                nodeInstalled.entities = _.where(results, { isInstalled: true });
                nodeUpdate.entities = _.filter(results, function (x) { return x.isInstalled && x.$alternativeVersion; });
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

        blade.headIcon = 'fa fa-cubes';

        blade.currentEntities = [
            nodeExisting = { name: 'platform.blades.modules-main.labels.browse', mode: 'browse' },
            nodeInstalled = { name: 'platform.blades.modules-main.labels.installed', mode: 'installed' },
            nodeUpdate = { name: 'platform.blades.modules-main.labels.updates', mode: 'update' }
        ];

        if ($scope.allowInstallModules) {
            blade.currentEntities.push({ name: 'platform.blades.modules-main.labels.advanced', mode: 'advanced' });
        }

        var nodeWithErrors = { name: 'platform.blades.modules-main.labels.withErrors', mode: 'withErrors' };

        var openFirstBladeInitially = _.once(function () { blade.openBlade(blade.currentEntities[0]); });

        blade.refresh();
    }]);
