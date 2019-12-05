angular.module('platformWebApp')
    .controller('platformWebApp.systemInfoController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', '$timeout', 'platformWebApp.bladeNavigationService', '$http', 'platformWebApp.diagnostics',
        function ($scope, validators, dialogService, contentApi, $timeout, bladeNavigationService, $http, diagnostics) {
            var blade = $scope.blade;

            function convertToText(data) {
                return JSON.stringify(data, null, "\t");
            }

            // convert platform info to user-friendly text presentation
            function convertToPlainText(data) {
                var platformInfo = 'Platform Version\r\n' + data.platformVersion;
                
                var licenseInfo;
                if (!data.license) {
                    licenseInfo = "Community license";
                } else {
                    licenseInfo = data.license.type + "license\r\n"
                                + data.license.customerName
                                + 'Expiration Date: ' + data.license.expirationDate;
                }

                var modulesInfo = "Installed modules\r\n";
                for (var i = 0; i < data.installedModules.length; i++) {
                    modulesInfo += data.installedModules[i].id + ' ' + data.installedModules[i].version +'\r\n';
                }

                var result = platformInfo + '\r\n\r\n'
                           + licenseInfo  + '\r\n\r\n'
                           + modulesInfo;
                return result;                               
            }

            blade.initializeBlade = function () {
                diagnostics.getSystemInfo({}, function (data) {
                    blade.isLoading = false;
                    blade.title = 'platform.blades.system-info.title';
                    blade.currentEntity = data;
                    blade.jsonTextInfo = convertToText(data);
                    blade.plainTextInfo = convertToPlainText(data);
                });
            };           

            $scope.downloadInfo = function () {
                var a = document.createElement("a");
                var file = new Blob([blade.jsonTextInfo], { type: 'application/json' });
                a.href = URL.createObjectURL(file);
                a.download = 'vc-platform-info.json';
                a.click();
            };

            $scope.copyToClipboard = function () {
                var textarea = document.createElement("textarea");
                document.body.appendChild(textarea);
                textarea.value = blade.plainTextInfo;
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
                    name: "Copy", icon: 'fa fa-copy',
                    executeMethod: $scope.copyToClipboard,
                    canExecuteMethod: function () {
                        return true;
                    }
                }
            ];

            blade.headIcon = 'fa-info-circle';
            blade.initializeBlade();
        }]);

