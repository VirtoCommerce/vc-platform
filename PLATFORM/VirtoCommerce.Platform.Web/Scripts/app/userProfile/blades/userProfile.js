angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', '$translate', 'platformWebApp.userProfileApi', 'platformWebApp.common.worldLanguages', function ($scope, bladeNavigationService, $translate, userProfileApi, worldLanguages) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';
    blade.currentLanguage = $translate.use();

    function initializeBlade() {
        userProfileApi.query(
           function (result) {
               blade.isLoading = false;
               result.sort();
               $scope.managerLanguages = _.map(result, function (x) {
                   var foundLang = worldLanguages.getLanguageByCode(x);
                   return {
                       title: foundLang ? foundLang.nativeName : x,
                       value: x
                   };
               });
           },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    $scope.setLanguage = function () {
        $translate.use(blade.currentLanguage);        
    };

    initializeBlade();
}]);
