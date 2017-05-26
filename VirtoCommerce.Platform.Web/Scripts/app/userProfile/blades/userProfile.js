angular.module('platformWebApp')
.controller('platformWebApp.userProfile.userProfileController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.settings.helper', '$translate', 'tmhDynamicLocale', 'moment', 'amMoment', 'amTimeAgoConfig', 'angularMomentConfig', 'platformWebApp.userProfile', 'platformWebApp.common.languages', 'platformWebApp.common.locales', 'platformWebApp.common.timeZones', 'platformWebApp.userProfileApi', function ($scope, bladeNavigationService, settings, settingsHelper, $translate, dynamicLocale, moment, momentService, timeAgoConfig, momentConfig, userProfile, languages, locales, timeZones, userProfileApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-user';
    blade.title = 'platform.blades.user-profile.title';

     userProfile.load().then(function () {     
         initializeBlade();
    });

    blade.currentLanguage = languages.normalize($translate.use());
    blade.currentRegionalFormat = locales.normalize(dynamicLocale.get());
    blade.currentTimeZone = timeZones.normalize(momentConfig.timezone);
    blade.useTimeAgo = userProfile.useTimeAgo;
    blade.currentFullDateThreshold = userProfile.fullDateThresholdUnit ? userProfile.fullDateThreshold : undefined;
    blade.currentFullDateThresholdUnit = userProfile.fullDateThresholdUnit;
    $scope.fullDateThresholdUnits = userProfile.fullDateThresholdUnits;

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
        dynamicLocale.set(blade.currentRegionalFormat.replace(/_/g, "-").toLowerCase());
        momentService.changeLocale(blade.currentRegionalFormat);
        userProfile.regionalFormat = blade.currentRegionalFormat;
        userProfile.save();
        $scope.setUseTimeAgo();
    }

    $scope.setTimeZone = function() {
        momentService.changeTimezone(blade.currentTimeZone);
        userProfile.timeZone = blade.currentTimeZone;
        userProfile.save();
    }

    var setFullDateThreshold = function (value) {
        blade.currentFullDateThreshold = blade.currentFullDateThresholdUnit ? value : undefined;
        timeAgoConfig.fullDateThreshold = value;
        userProfile.fullDateThreshold = value;
    }

    var setFullDateThresholdUnit = function (value) {
        blade.currentFullDateThresholdUnit = value;
        timeAgoConfig.fullDateThresholdUnit = value && value != 'Never' ? value.toLowerCase() : null;
        userProfile.fullDateThresholdUnit = value;
        userProfile.save();
    }

    $scope.setUseTimeAgo = function () {
        if (!blade.useTimeAgo) {
            // 1 millisecond threshold, it's not possible just 'off' time ago
            setFullDateThresholdUnit(null);
            setFullDateThreshold(1);
        } else {
            setFullDateThresholdUnit('Never');
            setFullDateThreshold(null);
        }
        userProfile.useTimeAgo = blade.useTimeAgo;
        userProfile.save();
    }

    $scope.setFullDateThreshold = function () {
        setFullDateThreshold(blade.currentFullDateThreshold);
        userProfile.save();
    }

    $scope.setFullDateThresholdUnit = function () {
        if (blade.currentFullDateThresholdUnit == 'Never') {
            setFullDateThreshold(null);
        }
        setFullDateThresholdUnit(blade.currentFullDateThresholdUnit);
        userProfile.save();
    }
}]);
