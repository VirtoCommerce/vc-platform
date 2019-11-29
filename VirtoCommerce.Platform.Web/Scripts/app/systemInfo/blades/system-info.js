angular.module('platformWebApp')
    .controller('platformWebApp.systemInfoController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', '$timeout', 'platformWebApp.bladeNavigationService', '$http', 'platformWebApp.diagnostics',
        function ($scope, validators, dialogService, contentApi, $timeout, bladeNavigationService, $http, diagnostics) {
            var blade = $scope.blade;

            function stringify(data) {
                return JSON.stringify(data, null, "\t");
            }

            blade.initializeBlade = function () {
                diagnostics.getSystemInfo({}, function (results) {
                    blade.isLoading = false;
                    blade.title = "Platform Info";
                    blade.currentEntity = results;
                    blade.content = stringify(results);
                });
            };           

            $scope.downloadInfo = function () {
                var a = document.createElement("a");
                var file = new Blob([blade.content], { type: 'application/json' });
                a.href = URL.createObjectURL(file);
                a.download = 'vc-system-info.json';
                a.click();
            };

            $scope.copyToClipboard = function () {
                var textarea = document.createElement("textarea");
                document.body.appendChild(textarea);
                textarea.value = blade.content;
                textarea.select();
                document.execCommand("copy");
                document.body.removeChild(textarea);
            };          

            blade.toolbarCommands = [
                {
                    name: "Download Info", icon: 'fa fa-download',
                    executeMethod: $scope.downloadInfo,
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: "Copy Info", icon: 'fa fa-copy',
                    executeMethod: $scope.copyToClipboard,
                    canExecuteMethod: function () {
                        return true;
                    }
                }
            ];

            blade.headIcon = 'fa-file-o';
            blade.initializeBlade();
        }]);

