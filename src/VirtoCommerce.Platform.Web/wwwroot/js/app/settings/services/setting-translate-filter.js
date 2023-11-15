angular.module("platformWebApp")
    .filter('settingTranslate', ['platformWebApp.localizableSettingService', function (localizableSettingService) {
        return localizableSettingService.translate;
    }]);
