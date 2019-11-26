angular.module('platformWebApp')
    .controller('platformWebApp.versionInfoController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', '$timeout', 'platformWebApp.bladeNavigationService', '$http', 'platformWebApp.modules',
        function ($scope, validators, dialogService, contentApi, $timeout, bladeNavigationService, $http, modules) {
            var blade = $scope.blade;
            var codemirrorEditor; 

            blade.initializeBlade = function () {
                modules.query({}, function (results) {
                    blade.isLoading = false;
                    blade.title = "Installed modules";
                    blade.currentEntity = JSON.stringify(results, null, "\t");                    
                    if (codemirrorEditor) {
                         codemirrorEditor.refresh();
                         codemirrorEditor.focus();
                    }                                                          
                })                                                      
            };            

            $scope.downloadManifest = function () {
                var a = document.createElement("a");
                var file = new Blob([JSON.stringify(blade.currentEntity, null, "\t")], { type: 'application/json' });
                a.href = URL.createObjectURL(file);
                a.download = 'manifest.json';
                a.click();                
            };
         
            blade.onClose = function (closeCallback) {
                //bladeNavigationService.closeBlade(blade);
                //bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "content.dialogs.asset-save.title", "content.dialogs.asset-save.message");
            };

            blade.toolbarCommands = [
                {
                    name: "Download VersionInfo", icon: 'fa fa-save',
                    executeMethod: $scope.downloadManifest,
                    canExecuteMethod: function () {
                        return true;
                    }                    
                }
            ];

            // Codemirror configuration
            $scope.editorOptions = {
                lineWrapping: true,
                lineNumbers: true,
                readOnly: true,
                parserfile: "javascript.js",
                extraKeys: { "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); } },
                foldGutter: true,
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                onLoad: function (_editor) {
                    codemirrorEditor = _editor;
                },
                value: blade.currentEntity,                
                mode: { name: "javascript", json: true }
            };

            blade.headIcon = 'fa-file-o';
            blade.initializeBlade();
        }])


    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('workspace.versionInfo', {
                url: '/versionInfo',
                templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                controller: ['platformWebApp.bladeNavigationService', function (bladeNavigationService) {
                    var blade = {
                        id: 'versionInfo',
                        controller: 'platformWebApp.versionInfoController',
                        template: '$(Platform)/Scripts/app/versionInfo/version-info.tpl.html'
                    };
                    bladeNavigationService.showBlade(blade);
                }]
            });
    }]);
