angular.module('virtoCommerce.searchModule')
.controller('virtoCommerce.searchModule.rebuildIndexController', ['$scope', '$interval', 'platformWebApp.bladeNavigationService', 'virtoCommerce.searchModule.search', 'platformWebApp.jobs', function ($scope, $interval, bladeNavigationService, search, jobs) {
    var blade = $scope.blade;

    blade.refresh = function () {
        if (blade.currentEntity) {
            jobs.getStatus(
                { id: blade.currentEntity.id },
                function (data) {
                    blade.currentEntity = data;

                    if (data.completed) {
                        blade.isLoading = false;
                        $scope.isRebuilding = false;
                        $scope.completed = true;
                        stopRefresh();
                    }
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); }
            );
        } else {
            blade.isLoading = false;
        }
    }

    $scope.submit = function () {
        blade.isLoading = true;
        $scope.isRebuilding = true;

        search.rebuild({}, onAfterSubmitted, function (error) {
            blade.isLoading = false;
            $scope.isRebuilding = false;
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    function onAfterSubmitted(data) {
        blade.currentEntity = data;
        blade.refresh();
        blade.intervalPromise = $interval(blade.refresh, 1000);
    }

    function stopRefresh() {
        if (angular.isDefined(blade.intervalPromise)) {
            $interval.cancel(blade.intervalPromise);
        }
    };

    $scope.$on('$destroy', function () {
        // Make sure that the interval is destroyed too
        stopRefresh();
    });

    blade.title = "Rebuild Search Index";
    blade.refresh();
}]);
