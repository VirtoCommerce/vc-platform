angular.module('platformWebApp.common.confirmDialog', [
])
.controller('confirmDialogController', ['$scope', '$modalInstance', 'message', 'title', function ($scope, $modalInstance, message, title) {
    $scope.message = message;
    $scope.title = title;

    $scope.yes = function () {
        $modalInstance.close(true);
    };

    $scope.no = function () {
        $modalInstance.close(false);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}])
.factory('dialogService', ['$rootScope', '$modal', function ($rootScope, $modal) {
    var dialogService = {
        dialogs: [],
        currentDialog: undefined
    };

    function findDialog(id) {
        var found;
        angular.forEach(dialogService.dialogs, function (dialog) {
            if (dialog.id == id) {
                found = dialog;
            }
        });

        return found;
    }

    function showDialog(dialog, templateUrl) {
        var dlg = findDialog(dialog.id);

        if (angular.isUndefined(dlg)) {
            dlg = dialog;

            dlg.instance = $modal.open({
                templateUrl: templateUrl,
                controller: 'confirmDialogController',
                resolve: {
                    message: function () {
                        return dialog.message;
                    },
                    title: function () {
                        return dialog.title;
                    }
                }
            });

            dlg.instance.result.then(function () //success
            {
                var idx = dialogService.dialogs.indexOf(dlg);
                dialogService.dialogs.splice(idx, 1);
            }, function (reason) //dismiss
            {
                var idx = dialogService.dialogs.indexOf(dlg);
                dialogService.dialogs.splice(idx, 1);
            });

            dialogService.dialogs.push(dlg);
        }
        //else
        //{
        //    angular.extend(dlg, dialog);
        //}

        dlg.instance.result.then(dialog.callback);
    };

    dialogService.showConfirmationDialog = function (dialog) {
        showDialog(dialog, 'Scripts/common/confirmDialog/confirmDialog.tpl.html');
    };

    dialogService.showNotificationDialog = function (dialog) {
        showDialog(dialog, 'Scripts/common/confirmDialog/notifyDialog.tpl.html');
    };

    return dialogService;

}])
