angular.module('platformWebApp')
    .controller('platformWebApp.systemInfoController', ['$scope', 'platformWebApp.diagnostics',
        function ($scope, diagnostics) {
            var blade = $scope.blade;

            blade.initializeBlade = function () {
                diagnostics.getSystemInfo({}, function (data) {
                    blade.isLoading = false;
                    blade.title = 'platform.blades.system-info.title';
                    blade.currentEntity = data;
                    blade.jsonTextInfo = JSON.stringify(data, null, "\t");
                });
            }

            $scope.runDownloading = function(data, name) {
                var a = document.createElement("a");
                var file = new Blob([data], { type: 'application/json' });
                a.href = URL.createObjectURL(file);
                a.download = name;
                a.click();
            }

            $scope.downloadInfo = function () {
                $scope.runDownloading(blade.jsonTextInfo, 'vc-platform-info.json');
            };

            $scope.downloadPackage = function () {
                var installedModules = blade.currentEntity.installedModules
                    .sort(function (a, b) {
                        return a.id.localeCompare(b.id);
                    })
                    .sort(function (a, b) {
                        if ($scope.isVirtoModule(a) && !$scope.isVirtoModule(b)) {
                            return -1;
                        }

                        if (!$scope.isVirtoModule(a) && $scope.isVirtoModule(b)) {
                            return 1;
                        }

                        return 0;
                    })
                    .map(function (x) {
                        return { Id: x.id, Version: x.version }
                    });

                var packages = {
                    ModuleSources: ["https://raw.githubusercontent.com/VirtoCommerce/vc-modules/master/modules_v3.json"],
                    PlatformVersion: blade.currentEntity.platformVersion,
                    Modules: installedModules
                }

                var packagesText = JSON.stringify(packages, null, "\t");

                $scope.runDownloading(packagesText, 'vc-package.json');
            }

            $scope.isVirtoModule = function (moduleInfo) {
                return moduleInfo.owners.some(x => x.toLowerCase().replace(/\s+/g, '') === "virtocommerce");
            }

            $scope.copyToClipboard = function () {
                var platformInfoElement = document.getElementById('platform-system-info');
                var infoText = platformInfoElement.innerText;
                var textarea = document.createElement("textarea");
                document.body.appendChild(textarea);
                textarea.value = infoText;
                textarea.select();
                document.execCommand("copy");
                document.body.removeChild(textarea);
            };

            blade.toolbarCommands = [
                {
                    name: "Download manifest", icon: 'fa fa-download',
                    executeMethod: $scope.downloadInfo,
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: "Download package", icon: 'fa fa-download',
                    executeMethod: $scope.downloadPackage,
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: "Copy", icon: 'fa fa-copy',
                    executeMethod: $scope.copyToClipboard,
                    canExecuteMethod: function () {
                        return true;
                    }
                }
            ];

            blade.headIcon = 'fa fa-info-circle';
            blade.initializeBlade();
        }]);

