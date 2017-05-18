angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.settings.helper', '$translate', 'tmhDynamicLocale', 'platformWebApp.userProfile', 'platformWebApp.common.worldLanguages', 'platformWebApp.common.worldLocales', 'platformWebApp.userProfileApi', function ($scope, bladeNavigationService, settings, settingsHelper, $translate, dynamicLocale, userProfile, worldLanguages, worldLocales, userProfileApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';

     userProfile.load().then(function () {     
         initializeBlade();
    });

    blade.currentLanguage = $translate.use();
    blade.currentRegionalFormat = dynamicLocale.get();

    function initializeBlade() {
        userProfileApi.getLocales(
           function (result) {
               result.sort();
               $scope.languages = _.map(result, function (x) {
                   var foundLang = worldLanguages.getLanguageByCode(x);
                   return {
                       title: foundLang ? foundLang.nativeName : x,
                       value: x
                   };
               });
               blade.isLoading = isLoading();
           });
        userProfileApi.getRegionalFormats(
            function (result) {
                result.sort();
                $scope.regionalFormats = _.filter(_.map(result, function(x) {
                    var foundRegFormat = worldLocales.getLocaleByCode(x);
                    return {
                        title: foundRegFormat ? foundRegFormat.nativeName : undefined,
                        value: x
                    };
                }), function(x) { return !!x.title; });
                blade.isLoading = isLoading();
            });
    };

    function isLoading() {
        return angular.isUndefined($scope.languages) || angular.isUndefined($scope.regionalFormats);
    }

    $scope.setLanguage = function () {
        $translate.use(blade.currentLanguage);
        userProfile.language = blade.currentLanguage;
        userProfile.save();
    };

    $scope.setRegionalFormat = function() {
        dynamicLocale.set(blade.currentRegionalFormat);
        userProfile.regionalFormat = blade.currentRegionalFormat;
        userProfile.save();
    }
}]);
