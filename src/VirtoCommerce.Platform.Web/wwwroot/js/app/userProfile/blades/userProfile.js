angular.module('platformWebApp')
    .controller('platformWebApp.userProfile.userProfileController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.settings.helper',
        'platformWebApp.i18n', 'platformWebApp.userProfile', 'platformWebApp.common.languages', 'platformWebApp.common.locales', 'platformWebApp.common.timeZones', 'platformWebApp.userProfileApi',
        function ($rootScope, $scope, bladeNavigationService, settings, settingsHelper, i18n, userProfile, languages, locales, timeZones, userProfileApi) {
            var blade = $scope.blade;
            blade.headIcon = 'fa-user';
            blade.title = 'platform.blades.user-profile.title';

            blade.currentLanguage = i18n.getLanguage();
            blade.currentRegionalFormat = i18n.getRegionalFormat();
            blade.currentTimeZone = i18n.getTimeZone();
            blade.currentTimeAgoSettings = i18n.getTimeAgoSettings();
            blade.currentTimeSettings = i18n.getTimeSettings();

            userProfile.load().then(function () {
                initializeBlade();
            });

            var getNameByCode = function (entities, entity) {
                return entities && entity ? _.find(entities, { id: entity }).name : entity;
            };

            function initializeBlade() {
                $scope.userProfile = userProfile;

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
                }

                userProfileApi.getLocales(function (result) {
                    result.sort();
                    $scope.languages = toNativeName(result, languages);
                    blade.currentLanguage = getNameByCode($scope.languages, blade.currentLanguage);
                    blade.isLoading = isLoading();
                });

                userProfileApi.getRegionalFormats(function (result) {
                    result.sort();
                    $scope.regionalFormats = toNativeName(result, locales);
                    blade.currentRegionalFormat = getNameByCode($scope.regionalFormats, blade.currentRegionalFormat);
                    blade.isLoading = isLoading();
                });

                $scope.timeZones = timeZones.query();
                blade.currentTimeZone = getNameByCode($scope.timeZones, blade.currentTimeZone);
                blade.currentTimeAgoSettings = userProfile.timeAgoSettings;
                blade.currentTimeSettings = userProfile.timeSettings;
            }

            function isLoading() {
                return angular.isUndefined($scope.userProfile) || angular.isUndefined($scope.languages) || angular.isUndefined($scope.regionalFormats);
            }

            // Localization and regional formats updated asynchronously
            $rootScope.$on('$translateChangeSuccess', function () {
                blade.currentLanguage = getNameByCode($scope.languages, i18n.getLanguage());
            });

            $scope.$on('$localeChangeSuccess', function () {
                blade.currentRegionalFormat = getNameByCode($scope.regionalFormats, i18n.getRegionalFormat());
                // Update time zones list because it contains numbers, which must be formatted with current regional format
                $scope.timeZones = timeZones.query();
                blade.currentTimeZone = getNameByCode($scope.timeZones, i18n.getTimeZone());
            });

            // Do not update user profile while blade is loading (change events occurs because user profile loaded)

            $scope.setLanguage = function () {
                if (!isLoading()) {
                    i18n.changeLanguage(userProfile.language);
                    userProfile.save();
                }
            };

            $scope.setRegionalFormat = function () {
                if (!isLoading()) {
                    i18n.changeRegionalFormat(userProfile.regionalFormat);
                    userProfile.save();
                }
            }

            $scope.setTimeZone = function () {
                if (!isLoading()) {
                    i18n.changeTimeZone(userProfile.timeZone);
                    blade.currentTimeZone = getNameByCode($scope.timeZones, i18n.getTimeZone());
                    userProfile.save();
                }
            }

            $scope.setTimeAgoSettings = function () {
                if (!isLoading()) {
                    i18n.changeTimeAgoSettings(blade.currentTimeAgoSettings);
                    // We want to use fixed by i18n service value and it's impossible to set undefined values for time ago settings in blade UI
                    // Extend time ago settings instead of directly set to prevent overwrite threshold units
                    angular.extend(blade.currentTimeAgoSettings, i18n.getTimeAgoSettings());
                    angular.extend(userProfile.timeAgoSettings, blade.currentTimeAgoSettings);
                    userProfile.save();
                }
            }
            $scope.setTimeSettings = function () {
                if (!isLoading()) {
                    i18n.changeTimeSettings(blade.currentTimeSettings);
                    angular.extend(blade.currentTimeSettings, i18n.getTimeSettings());
                    angular.extend(userProfile.timeSettings, blade.currentTimeSettings);
                    userProfile.save();
                }
            }
        }]);
