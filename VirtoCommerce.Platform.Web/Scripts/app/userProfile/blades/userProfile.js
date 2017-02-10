angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.authService', 'platformWebApp.localization', 'platformWebApp.common.worldLanguages', '$translate', function ($scope, bladeNavigationService, authService, localization, worldLanguages, $translate) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';
    blade.currentLanguage = localization.get({ id: authService.userId });

    function initializeBlade() {
        localization.query(
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
        localization.update({ id: authService.userId }, blade.currentLanguage);
        $translate.use(blade.currentLanguage);
    };

    initializeBlade();
}]);