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
            };           

            $scope.downloadInfo = function () {
                var a = document.createElement("a");
                var file = new Blob([blade.jsonTextInfo], { type: 'application/json' });
                a.href = URL.createObjectURL(file);
                a.download = 'vc-platform-info.json';
                a.click();
            };

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

