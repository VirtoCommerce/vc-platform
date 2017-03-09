angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.settings.helper', '$translate', 'platformWebApp.userProfileApi', 'platformWebApp.common.worldLanguages', function ($scope, bladeNavigationService, settings, settingsHelper, $translate, userProfileApi, worldLanguages) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';

    var userProfileSettings;
    settings.getCurrentUserProfile(function (currentUserProfileSettings) {
        settingsHelper.fixValues(currentUserProfileSettings);
        userProfileSettings = currentUserProfileSettings;
        initializeBlade();
    });

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
        settingsHelper.getSetting(userProfileSettings, "VirtoCommerce.Platform.UI.Language").value = blade.currentLanguage;
        settings.updateCurrentUserProfile(userProfileSettings);
    };
}]);
