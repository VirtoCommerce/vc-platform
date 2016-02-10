angular.module('platformWebApp')
 .factory('platformWebApp.bladeUtils', ['platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function (bladeNavigationService, dialogService) {
     function initializePagination($scope) {
         //pagination settings
         $scope.pageSettings = {};
         $scope.pageSettings.totalItems = 0;
         $scope.pageSettings.currentPage = 1;
         $scope.pageSettings.numPages = 5;
         $scope.pageSettings.itemsPerPageCount = 20;

         $scope.$watch('pageSettings.currentPage', $scope.blade.refresh);
     }

     function showConfirmationIfNeeded(isDirty, canSave, blade, saveChangesCallback, closeCallback, title, message) {
         if (isDirty) {
             bladeNavigationService.closeChildrenBlades(blade, function () {
                 var dialog = {
                     id: "confirmCurrentBladeClose"
                 };

                 if (canSave) {
                     dialog.title = title;
                     dialog.message = message;
                 } else {
                     dialog.title = "Warning";
                     dialog.message = "Validation failed for this object. Will you continue editing and save later?";
                 }

                 dialog.callback = function (needSave) {
                     if (canSave) {
                         if (needSave) {
                             saveChangesCallback();
                         }
                         closeCallback();
                     } else if (!needSave) {
                         closeCallback();
                     }
                 };

                 dialogService.showConfirmationDialog(dialog);
             });
         }
         else {
             closeCallback();
         }
     }

     return {
         bladeNavigationService: bladeNavigationService,
         initializePagination: initializePagination,
         showConfirmationIfNeeded: showConfirmationIfNeeded
     };
 }]);
