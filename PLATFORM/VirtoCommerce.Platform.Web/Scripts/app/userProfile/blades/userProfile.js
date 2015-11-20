angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', '$translate', 'platformWebApp.settings', 'virtoCommerce.coreModule.common.countries', function ($scope, bladeNavigationService, $translate, settings, countries) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';
    blade.currentLanguage = $translate.use();

    function initializeBlade() {
        settings.getValues({ id: 'VirtoCommerce.Platform.General.ManagerLanguages' },
           function (result) {
               blade.isLoading = false;
               result.sort();
               $scope.managerLanguages = _.map(result, function (x) {
                   var foundLang = countries.getLanguageByCode(x);
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
