angular.module('platformWebApp')
.constant("platformWebApp.fallbackLanguage", "en")
.constant("platformWebApp.fallbackRegionalFormat", "en")
.constant("platformWebApp.fallbackTimeZone", "Etc/Utc")
.constant("platformWebApp.fallbackTimeAgoSettings", { useTimeAgo: true, thresholdUnit: 'Never', threshold: null })
.constant("platformWebApp.fallbackTimeFormat", { showMeridian: true})
// Service provider get/set function pairs for language, regional format, time zone and time ago settings
    .factory('platformWebApp.i18n', ['platformWebApp.fallbackLanguage', 'platformWebApp.fallbackRegionalFormat', 'platformWebApp.fallbackTimeAgoSettings', 'platformWebApp.common.languages', 'platformWebApp.common.locales', 'platformWebApp.common.timeZones', 'platformWebApp.userProfileApi', '$translate', 'tmhDynamicLocale', 'moment', 'amMoment', 'angularMomentConfig', 'amTimeAgoConfig', 'platformWebApp.fallbackTimeFormat',
        function (fallbackLanguage, fallbackRegionalFormat, fallbackTimeAgoSettings, languages, locales, timeZones, userProfileApi, $translate, dynamicLocale, moment, momentService, momentConfig, timeAgoConfig, fallbackTimeFormat) {
        var changeLanguage = function (language) {
            userProfileApi.getLocales(function(availableLanguages) {
                availableLanguages.sort();
                if (!language) {
                    // Use current browser language, if no language specified
                    language = $translate.resolveClientLocale(); //.match(/^[a-zA-Z]{2}/)[0];
                }
                if (!languages.contains(language)) {
                    language = fallbackLanguage;
                }
                language = languages.normalize(language);
                $translate.use(language);
            });
        }

        var changeRegionalFormat = function(locale) {
            userProfileApi.getRegionalFormats(function(availableLocales) {
                availableLocales.sort();
                if (!locale) {
                    // Get regional format from current browser locale, if no regional format specified
                    locale = $translate.resolveClientLocale();
                }
                if (!locales.contains(locale)) {
                    locale = fallbackRegionalFormat;
                }
                locale = locales.normalize(locale);
                // angular locale
                dynamicLocale.set(locale.replace(/_/g, "-").toLowerCase());
                momentService.changeLocale(locale);
            });
        }

        var changeTimeZone = function (timeZone) {
            if (!timeZone) {
                timeZone = moment.tz.guess();
            }
            momentService.changeTimezone(timeZone);
        }

        var changeTimeAgoSettings = function (timeAgoSettings) {
            if (timeAgoSettings == null || timeAgoSettings.useTimeAgo == null) {
                timeAgoSettings = fallbackTimeAgoSettings;
            }
            if (!timeAgoSettings.useTimeAgo) {
                // We can't just 'turn off' time ago, so (and it's default behavior) set it to 1 millisecond
                timeAgoConfig.fullDateThresholdUnit = null;
                timeAgoConfig.fullDateThreshold = 1;
            } else {
                // see above
                timeAgoConfig.fullDateThresholdUnit = timeAgoSettings.thresholdUnit && timeAgoSettings.thresholdUnit !== 'Never' ? timeAgoSettings.thresholdUnit.toLowerCase() : null;
                // To avoid situation when settings set threshold unit, but threshold value is undefined, set threshold value to 1 if it's not specified
                timeAgoConfig.fullDateThreshold = timeAgoSettings.thresholdUnit && timeAgoSettings.thresholdUnit !== 'Never' ? timeAgoSettings.threshold || 1 : null;
            }
        }
        var changeTimeSettings = function (timeSettings) {
            if (timeSettings)
                fallbackTimeFormat = timeSettings;
        }
            return {
            getLanguage: function() { return languages.normalize($translate.use()) },
            getRegionalFormat: function () { return locales.normalize(dynamicLocale.get()) },
            getTimeZone: function () { return momentConfig.timezone },
            getTimeAgoSettings: function () {
                var result = {};
                // Always use time ago:          threshold unit = null, threshold value = null                           | null
                // Never use time ago:           threshold unit = null, threshold value = positive number (1 by default) | x milliseconds
                // Use time ago after threshold: threshold unit = y,    threshold value = x                              | x of y
                result.useTimeAgo = !(timeAgoConfig.fullDateThresholdUnit == null && timeAgoConfig.fullDateThreshold != null);
                result.thresholdUnit = timeAgoConfig.fullDateThresholdUnit ? timeAgoConfig.fullDateThresholdUnit.capitalize() : (result.useTimeAgo ? 'Never' : null);
                result.threshold = result.useTimeAgo && result.thresholdUnit !== 'Never' ? timeAgoConfig.fullDateThreshold : null;
                return result;
            },
            getTimeSettings: function () {
                var result = {};
                result.showMeridian = fallbackTimeFormat.showMeridian;
                return result;
            },
            changeLanguage: changeLanguage,
            changeRegionalFormat: changeRegionalFormat,
            changeTimeZone: changeTimeZone,
            changeTimeAgoSettings: changeTimeAgoSettings,
            changeTimeSettings: changeTimeSettings
        }
    }
])
// Configure fallbacks for language, regional format, time zone and time ago settings
    .config(['$provide', 'platformWebApp.fallbackLanguage', 'platformWebApp.fallbackRegionalFormat', 'platformWebApp.fallbackTimeZone', 'platformWebApp.fallbackTimeAgoSettings', 'tmhDynamicLocaleProvider', '$translateProvider', 'angularMomentConfig', 'amTimeAgoConfig', 'platformWebApp.fallbackTimeFormat',
        function ($provide, fallbackLanguage, fallbackRegionalFormat, fallbackTimeZone, fallbackTimeAgoSettings, dynamicLocaleProvider, $translateProvider, momentConfig, timeAgoConfig, fallbackTimeFormat) {

    // https://angular-translate.github.io/docs/#/guide
    $translateProvider.useUrlLoader('api/platform/localization')
        .useLoaderCache(true)
        .useSanitizeValueStrategy('escapeParameters')
        .preferredLanguage(fallbackLanguage)
        .fallbackLanguage(fallbackLanguage)
        .useLocalStorage();


    // https://github.com/lgalfaso/angular-dynamic-locale/#usage
    dynamicLocaleProvider.localeLocationPattern('$(Platform)/Scripts/i18n/angular/angular-locale_{{locale}}.js');
    dynamicLocaleProvider.defaultLocale(fallbackRegionalFormat);
    dynamicLocaleProvider.useCookieStorage();

    momentConfig.timeZone = fallbackTimeZone;

    if (!fallbackTimeAgoSettings.useTimeAgo) {
        timeAgoConfig.fullDateThresholdUnit = null;
        timeAgoConfig.fullDateThreshold = 1;
    } else {
        timeAgoConfig.fullDateThresholdUnit = fallbackTimeAgoSettings.thresholdUnit;
        timeAgoConfig.fullDateThreshold = fallbackTimeAgoSettings.threshold;
    }
    
    $provide.decorator('tmhDynamicLocale', ['$delegate', '$rootScope', '$translate', 'platformWebApp.fallbackRegionalFormat', 'platformWebApp.angularToMomentFormatConverter', function ($delegate, $rootScope, $translate, fallbackRegionalFormat, dateFormatConverter) {
        var service = $delegate;
        // Change time ago full date format when regional formatting changed
        $rootScope.$on('$localeChangeSuccess', function () {
            timeAgoConfig.fullDateFormat = dateFormatConverter.convert('short');
        });
        // There is no option to set fallback regional format and use it when we can't use specified.
        // This decorator subscribes to angular dynamic locale change error and tries to use fallback regional format
        $rootScope.$on('$localeChangeError', function(locale) {
            if (locale !== fallbackRegionalFormat) {
                service.set(fallbackRegionalFormat);
            }
        });
        return service;
    }]);
}])
// Configure defaults for language, regional format, time zone and time ago settings. Values from browser will be used if it's possible.
.run(['platformWebApp.i18n', function(i18n) {
    i18n.changeLanguage();
    i18n.changeRegionalFormat();
    i18n.changeTimeZone();
    i18n.changeTimeAgoSettings();
    i18n.changeTimeSettings();
}]);
