angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.settings.helper', '$translate', 'platformWebApp.userProfile', 'platformWebApp.common.worldLanguages', 'platformWebApp.userProfileApi', function ($scope, bladeNavigationService, settings, settingsHelper, $translate, userProfile, worldLanguages, userProfileApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';

     userProfile.load().then(function () {     
         initializeBlade();
    });

    blade.currentLanguage = $translate.use();

    function initializeBlade() {
        userProfileApi.getLocales(
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
           });
    };

    $scope.setLanguage = function () {
        $translate.use(blade.currentLanguage);
        userProfile.language = blade.currentLanguage;
        userProfile.save();
    };
}]);
