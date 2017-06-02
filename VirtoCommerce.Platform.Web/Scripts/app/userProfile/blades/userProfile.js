angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.settings.helper',
    'platformWebApp.i18n', 'platformWebApp.userProfile', 'platformWebApp.common.languages', 'platformWebApp.common.locales', 'platformWebApp.common.timeZones', 'platformWebApp.userProfileApi',
    function ($rootScope, $scope, bladeNavigationService, settings, settingsHelper, i18n, userProfile, languages, locales, timeZones, userProfileApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';

     userProfile.load().then(function () {     
         initializeBlade();
    });

    blade.currentLanguage = i18n.getLanguage();
    blade.currentRegionalFormat = i18n.getRegionalFormat();
    blade.currentTimeZone = i18n.getTimeZone();
    blade.timeAgoSettings = i18n.getTimeAgoSettings();

    function initializeBlade() {
        // Display languages and locales in native name format (i.e. English, but русский (Russian))
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
        blade.fullDateThresholdUnits = userProfile.timeAgoSettings.thresholdUnits;
    };

    function isLoading() {
        return angular.isUndefined($scope.languages) || angular.isUndefined($scope.regionalFormats) || angular.isUndefined(blade.currentTimeZone) || angular.isUndefined(blade.timeAgoSettings);
    }

    // Localization and regional formats updated asynchronously
    $rootScope.$on('$translateChangeSuccess', function() {
        blade.currentLanguage = i18n.getLanguage();
    });
    $scope.$on('$localeChangeSuccess', function() {
        blade.currentRegionalFormat = i18n.getRegionalFormat();
    });

    // Update blade fields after user profile fields, because we want to keep undefined on user profile, but always show value on blade

    $scope.setLanguage = function () {
        i18n.changeLanguage(blade.currentLanguage);
        userProfile.language = blade.currentLanguage;
        // Fallback for situation when user alreasy use fallback language but want to reset it
        blade.currentLanguage = i18n.getLanguage();
        userProfile.save();
    };

    $scope.setRegionalFormat = function () {
        i18n.changeRegionalFormat(blade.currentRegionalFormat);
        userProfile.regionalFormat = blade.currentRegionalFormat;
        // Fallback for situation when user alreasy use fallback language but want to reset it
        blade.currentRegionalFormat = i18n.getRegionalFormat();
        userProfile.save();
    }

    $scope.setTimeZone = function () {
        i18n.changeTimeZone(blade.currentTimeZone);
        userProfile.timeZone = blade.currentTimeZone;
        // Because time zone change operation is synchronous, we just get value and set it to field
        blade.currentTimeZone = i18n.getTimeZone();
        userProfile.save();
    }

    $scope.setTimeAgoSettings = function () {
        i18n.changeTimeAgoSettings(blade.timeAgoSettings);
        // Because time ago setting change operation is synchronous, we just get value and set it to field
        // Update blade field before user profile, because we want to use fixed by i18n service value and it's impossible to set undefined values for time ago settings in blade UI
        blade.timeAgoSettings = i18n.getTimeAgoSettings();
        userProfile.timeAgoSettings = blade.timeAgoSettings;
        userProfile.save();
    }
}]);
