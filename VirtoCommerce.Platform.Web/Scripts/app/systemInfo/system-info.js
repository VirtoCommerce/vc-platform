angular.module('platformWebApp')
    .controller('platformWebApp.systemInfoController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', '$timeout', 'platformWebApp.bladeNavigationService', '$http', 'platformWebApp.modules',
        function ($scope, validators, dialogService, contentApi, $timeout, bladeNavigationService, $http, modules) {
            var blade = $scope.blade;           

            blade.initializeBlade = function () {
                modules.query({}, function (results) {
                    blade.isLoading = false;
                    blade.title = "VirtoCommerce Info";
                    blade.currentEntity = $scope.stringify(results);                  
                });                                                      
            };

            $scope.stringify = function (data) {
                return JSON.stringify(data, null, "\t");
            }

            $scope.downloadInfo = function () {
                var a = document.createElement("a");
                var file = new Blob([blade.currentEntity], { type: 'application/json' });
                a.href = URL.createObjectURL(file);
                a.download = 'systemInfo.json';
                a.click();                
            };

            $scope.copyToClipboard = function () {
                var textarea = document.createElement("textarea");
                document.body.appendChild(textarea);
                textarea.value = blade.currentEntity;
                textarea.select();
                document.execCommand("copy");
                document.body.removeChild(textarea);
            };
         
            blade.onClose = function (closeCallback) {
                //bladeNavigationService.closeBlade(blade);
                //bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "content.dialogs.asset-save.title", "content.dialogs.asset-save.message");
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
        }])


    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('workspace.systemInfo', {
                url: '/systeminfo',
                templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                controller: ['platformWebApp.bladeNavigationService', function (bladeNavigationService) {
                    var blade = {
                        id: 'versionInfo',
                        controller: 'platformWebApp.systemInfoController',
                        template: '$(Platform)/Scripts/app/systemInfo/system-info.tpl.html'
                    };
                    bladeNavigationService.showBlade(blade);
                }]
            });
    }]);
