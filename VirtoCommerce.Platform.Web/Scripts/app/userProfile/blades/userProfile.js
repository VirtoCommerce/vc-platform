angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.settings.helper', '$translate', 'tmhDynamicLocale', 'moment', 'amMoment', 'angularMomentConfig', 'platformWebApp.userProfile', 'platformWebApp.common.languages', 'platformWebApp.common.locales', 'platformWebApp.common.timeZones', 'platformWebApp.userProfileApi', function ($scope, bladeNavigationService, settings, settingsHelper, $translate, dynamicLocale, moment, momentService, momentConfig, userProfile, languages, locales, timeZones, userProfileApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';

     userProfile.load().then(function () {     
         initializeBlade();
    });

    blade.currentLanguage = languages.normalize($translate.use());
    blade.currentRegionalFormat = locales.normalize(dynamicLocale.get());
    blade.currentTimeZone = timeZones.normalize(momentConfig.timezone);

    function initializeBlade() {
        function toNativeName(list, provider) {
            return _.map(list, function (x) {
                x = provider.normalize(x);
                var value = provider.get(x);
                return {
                    name: value ? value.nativeName : x,
                    id: x
                };
            });
        };

        userProfileApi.getLocales(
           function (result) {
               result.sort();
               $scope.languages = toNativeName(result, languages);
               blade.isLoading = isLoading();
           });
        userProfileApi.getRegionalFormats(
            function (result) {
                result.sort();
                $scope.regionalFormats = toNativeName(result, locales);
                blade.isLoading = isLoading();
            });
        $scope.timeZones = timeZones.query();
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
        dynamicLocale.set(blade.currentRegionalFormat.replace("_", "-").toLowerCase());
        momentService.changeLocale(blade.currentRegionalFormat);
        userProfile.regionalFormat = blade.currentRegionalFormat;
        userProfile.save();
    }

    $scope.setTimeZone = function() {
        momentService.changeTimezone(blade.currentTimeZone);
        userProfile.timeZone = blade.currentTimeZone;
        userProfile.save();
    }
}]);
