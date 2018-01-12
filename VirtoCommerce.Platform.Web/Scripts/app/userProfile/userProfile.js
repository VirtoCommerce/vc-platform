angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider
        .state('workspace.userProfile', {
            url: '/userProfile',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var blade = {
                    id: 'userProfile',
                    controller: 'platformWebApp.userProfile.userProfileController',
                    template: '$(Platform)/Scripts/app/userProfile/blades/userProfile.tpl.html',
                    isClosingDisabled: true
                };
                bladeNavigationService.showBlade(blade);
            }]
        });
}])
.factory('platformWebApp.userProfile', ['platformWebApp.userProfileApi', 'platformWebApp.settings.helper', 'platformWebApp.common.languages', 'platformWebApp.common.locales', function (userProfileApi, settingsHelper, languages, locales) {
    var onChangeCallbacks = [];

    var result = {
        language: undefined,
        regionalFormat: undefined,
        timeZone: undefined,
        timeAgoSettings: {
            useTimeAgo: undefined,
            threshold: undefined,
            thresholdUnit: undefined,
            thresholdUnits: undefined
        },
        timeSettings: {
            showMeridian: undefined
        },
        mainMenuState: {},
        load: function () {
            return userProfileApi.get(function (profile) {
                settingsHelper.fixValues(profile.settings);
                profile.language = languages.normalize(settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.Language").value);
                profile.regionalFormat = locales.normalize(settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.RegionalFormat").value);
                profile.timeZone = settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.TimeZone").value;
                profile.timeSettings = {};
                profile.timeSettings.showMeridian = settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.ShowMeridian").value;
                profile.timeAgoSettings = {};
                profile.timeAgoSettings.useTimeAgo = settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.UseTimeAgo").value;
                profile.timeAgoSettings.threshold = settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.FullDateThreshold").value;
                var fullDateThresholdUnitSetting = settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.FullDateThresholdUnit");
                profile.timeAgoSettings.thresholdUnit = fullDateThresholdUnitSetting.value;
                profile.timeAgoSettings.thresholdUnits = fullDateThresholdUnitSetting.allowedValues;
                profile.mainMenuState = settingsHelper.getSetting(profile.settings, "VirtoCommerce.Platform.UI.MainMenu.State").value;
                if (profile.mainMenuState) {
                    profile.mainMenuState = angular.fromJson(profile.mainMenuState);
                }
                angular.extend(result, profile);
            }).$promise;
        },
        save: function() {
            var oldState = angular.copy(this);
            var mainMenuStateSetting = settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.MainMenu.State");
            mainMenuStateSetting.value = angular.toJson(this.mainMenuState);
            settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.Language").value = languages.normalize(result.language);
            settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.RegionalFormat").value = locales.normalize(result.regionalFormat);
            settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.TimeZone").value = result.timeZone;
            settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.ShowMeridian").value = result.timeSettings.showMeridian;
            settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.UseTimeAgo").value = result.timeAgoSettings.useTimeAgo;
            settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.FullDateThreshold").value = result.timeAgoSettings.threshold;
            settingsHelper.getSetting(this.settings, "VirtoCommerce.Platform.UI.FullDateThresholdUnit").value = result.timeAgoSettings.thresholdUnit;
            return userProfileApi.save(result).$promise.then(function() {
                onChangeCallbacks.forEach(function(callback) {
                    callback(this, oldState);
                });
            });
        },
        registerOnChangeCallback : function(callback) {
            onChangeCallbacks.push(callback);
        }
    }
    return result;
}])
.run(['platformWebApp.mainMenuService', '$state', function (mainMenuService, $state) {
    var menuItem = {
        path: 'configuration/userProfile',
        icon: 'fa  fa-user',
        title: 'platform.menu.user-profile',
        priority: 99,
        action: function () { $state.go('workspace.userProfile'); }
    };
    mainMenuService.addMenuItem(menuItem);
}]);
