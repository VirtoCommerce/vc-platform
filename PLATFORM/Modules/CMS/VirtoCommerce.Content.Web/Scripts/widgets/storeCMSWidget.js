angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.storeCMSWidgetController', ['$state', '$scope', 'virtoCommerce.contentModule.contentApi', 'platformWebApp.bladeNavigationService', function ($state, $scope, contentApi, bladeNavigationService) {
    var blade = $scope.widget.blade;

    function initialize() {
        $scope.statistics = {
            activeThemeName: '...',
            themesCount: '...',
            pagesCount: '...',
            blogsCount: '...',
            listLinksCount: '...'
        };
        contentApi.getStatistics({ storeId: blade.currentEntityId }, function (data) {
            angular.extend($scope.statistics, data);
        });
    }

    $scope.openBlade = function () {
        $state.go('workspace.content', { storeId: blade.currentEntityId });
    };

    initialize();
}]);
