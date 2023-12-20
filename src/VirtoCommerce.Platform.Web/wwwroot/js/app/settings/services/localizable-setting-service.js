angular.module("platformWebApp")
    .factory('platformWebApp.localizableSettingService', [
        '$rootScope', 'platformWebApp.i18n', 'platformWebApp.localizableSettingsApi',
        function ($rootScope, i18n, localizableSettingsApi) {
            var names = [];
            var settings = {};
            var languages = [];

            $rootScope.$on('loginStatusChanged', function (event, authContext) {
                if (authContext.isAuthenticated) {
                    loadSettings();
                }
            });

            return {
                isLocalizable: isLocalizable,
                translate: translate,
                getValues: getValues,
                getItemsAndLanguagesAsync: getItemsAndLanguagesAsync,
                saveItemsAsync: saveItemsAsync,
                deleteItemsAsync: deleteItemsAsync,
            };

            function loadSettings() {
                return localizableSettingsApi.getSettingsAndLanguages().$promise.then(function (response) {
                    names.length = 0;
                    settings = {};
                    response.settings.forEach(function (setting) {
                        settings[setting.name] = setting.items;
                        if (setting.isLocalizable) {
                            names.push(setting.name);
                        }
                    });
                    languages = response.languages;
                    return response;
                });
            }

            function isLocalizable(settingName) {
                return names.includes(settingName);
            }

            function translate(key, settingName, language) {
                const values = getValues(settingName, language);

                if (!values) {
                    return key
                }

                const item = values.find(x => x.key === key);

                return item ? item.value : key;
            }

            function getValues(settingName, language) {
                const items = settings[settingName];

                if (!items) {
                    return [];
                }

                if (!language) {
                    language = i18n.getLanguage();
                }

                const languageLowerCase = language.toLowerCase();

                if (languages.includes(languageLowerCase)) {
                    return _.map(items, function (item) {
                        const localizedValue = item.localizedValues.find(function (x) {
                            return x.languageCode === languageLowerCase;
                        });

                        const value = localizedValue && localizedValue.value ? localizedValue.value : item.alias;

                        return {
                            key: item.alias,
                            value: value
                        };
                    });
                }

                const languagePrefix = languageLowerCase + '-';

                if (languages.some(function (x) { return x.startsWith(languagePrefix) })) {
                    return _.map(items, function (item) {
                        const localizedValue = item.localizedValues.find(function (x) {
                            return x.languageCode.startsWith(languagePrefix);
                        });

                        const value = localizedValue && localizedValue.value ? localizedValue.value : item.alias;

                        return {
                            key: item.alias,
                            value: value
                        };
                    });
                }

                return _.map(items, function (item) {
                    return {
                        key: item.alias,
                        value: item.alias
                    };
                });
            }

            function getItemsAndLanguagesAsync(settingName) {
                return loadSettings().then(function () {
                    var items = settings[settingName];

                    return {
                        items: items ? angular.copy(items) : [],
                        languages: angular.copy(languages),
                    };
                });
            }

            function saveItemsAsync(settingName, items) {
                return localizableSettingsApi.saveItems({ name: settingName }, items).$promise;
            }

            function deleteItemsAsync(settingName, aliases) {
                return localizableSettingsApi.deleteItems({ name: settingName, values: aliases }).$promise;
            }
        }])
    ;
