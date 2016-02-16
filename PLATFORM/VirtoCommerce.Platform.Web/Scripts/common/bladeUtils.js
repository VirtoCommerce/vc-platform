angular.module('platformWebApp')
 .factory('platformWebApp.bladeUtils', ['platformWebApp.bladeNavigationService', function (bladeNavigationService) {
     function initializePagination($scope) {
         //pagination settings
         $scope.pageSettings = {};
         $scope.pageSettings.totalItems = 0;
         $scope.pageSettings.currentPage = 1;
         $scope.pageSettings.numPages = 5;
         $scope.pageSettings.itemsPerPageCount = 20;

         $scope.$watch('pageSettings.currentPage', $scope.blade.refresh);
     }
     
     return {
         bladeNavigationService: bladeNavigationService,
         initializePagination: initializePagination
     };
 }]);
