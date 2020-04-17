angular.module('platformWebApp')
    .controller('platformWebApp.importSampleDataWidgetController', ['$scope', '$window', 'platformWebApp.dialogService', function ($scope, $window, dialogService) {

        var blade = $scope.blade;

        $scope.runImport = function () {
            var dialog = {
                id: "confirmImport",
                title: "platform.dialogs.run-import.title",
                message: "platform.dialogs.run-import.message",
                callback: function (confirm) {
                    if (confirm) {
                        startImport();
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }

        function startImport() {
            var setupStep = _.findWhere(blade.currentEntities['Setup'], { 'name': 'VirtoCommerce.SetupStep' });
            var sampleDataState = _.findWhere(blade.currentEntities['Setup'], { 'name': 'VirtoCommerce.SampleDataState' });

            if (setupStep && sampleDataState) {
                setupStep.values = [{ id: "setupWizard.sampleDataInstallation", value: "setupWizard.sampleDataInstallation" }];
                sampleDataState.values = [{ id: "Undefined", value: "Undefined" }];

                blade.saveChanges().then(function () {
                    $window.location.reload();
                });
            }
        }
    }]);
