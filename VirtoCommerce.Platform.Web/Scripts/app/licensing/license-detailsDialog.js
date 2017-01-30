angular.module('platformWebApp')
.controller('platformWebApp.licenseDetailsDialogController', ['$scope', '$modalInstance', 'dialog', function ($scope, $modalInstance, dialog) {
    angular.extend($scope, dialog);

    $scope.currentEntity = { type: "Community deployment", name: "john doe company", expirationDate: new Date().setMonth(new Date().getMonth() + 1) };

    //$scope.no = function () {
    //    $modalInstance.close(false);
    //};

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}]);
