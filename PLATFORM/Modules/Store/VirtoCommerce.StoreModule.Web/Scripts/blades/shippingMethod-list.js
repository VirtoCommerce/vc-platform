angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.shippingMethodListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    function initializeBlade(data) {
        // temp: mock data
        //data.push({ code: 'mock1', priority:2, logoUrl: 'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQ3_FeY5LAEBl0RM3Csu9eGegAsqoZkH2S4WbO7Cy0H4T9EJuIO_Q' });
        //data.push({ code: 'mock3', priority: 3, logoUrl: 'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQVmLBvDo6WwhIIFr8A0d0hfYWb1WqPWUX7XIH1lt2DVjOQGt0M' });
        //data.push({ code: 'mock4', priority: 4, logoUrl: 'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQOopZ_owqLWotyOx4QXvIwK0L2MDICd4_wE4IX4bV61fa5cvCw' });
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;

        $scope.blade.currentEntities.sort(function (a, b) {
            return a.priority > b.priority;
        });
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.code;

        var newBlade = {
            id: 'shippingMethodList',
            origEntity: node,
            title: $scope.blade.title,
            subtitle: 'Edit shipping method',
            controller: 'virtoCommerce.storeModule.shippingMethodDetailController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/shippingMethod-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
    
    $scope.sortableOptions = {
        stop: function (e, ui) {
            for (var i = 0; i < $scope.blade.currentEntities.length; i++) {
                $scope.blade.currentEntities[i].priority = i + 1;
            }
        },
        axis: 'y',
        cursor: "move"
    };

    $scope.bladeHeadIco = 'fa-archive';

    $scope.$watch('blade.parentBlade.currentEntity.shippingMethods', function (currentEntities) {
        initializeBlade(currentEntities);
    });

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.shippingMethods' gets fired
}]);