angular.module('platformWebApp')
.directive('vaLoginToolbar', ['$document', '$timeout', 'authService', function ($document, $timeout, authService) {
    return {
        templateUrl: 'Scripts/common/security/login/loginToolbar.tpl.html',
        restrict: 'E',
        replace: true,
        scope: true,
        link: function ($scope, $element, $attrs, $controller) {
            //$scope.isAuthenticated = authService.isAuthenticated;
            $scope.logout = authService.logout;
            $scope.$watch(function () {
                return authService.userLogin;
            }, function (userLogin) {
                $scope.userLogin = userLogin;
                $scope.fullName = authService.fullName;
            });

            // menu stuff
            var onDocumentClick = function (event) {
                //$scope.isMenuVisible = false;
                $scope.$apply("isMenuVisible = false");
                $document.off("click", onDocumentClick);
            };
            
            $scope.showMenu = function () {
                $document.off("click", onDocumentClick);
                $scope.isMenuVisible = !$scope.isMenuVisible;
                if ($scope.isMenuVisible) {
                    $timeout(function () {
                        $document.on("click", onDocumentClick);
                    });
                }
            }
        }
    }
}])